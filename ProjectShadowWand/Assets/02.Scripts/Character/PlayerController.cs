using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    //[SerializeField] CharacterAudio audioPlayer = null;

    [Header("이동 관련")]

    [SerializeField, Tooltip("플레이어의 이동속도")]
    float movementSpeed = 0.0f;

    [SerializeField, Tooltip("플레이어의 최대 이동속도. \n가속도를 더하는 형식이라 필요한 것 뿐입니다.")]
    float maxMovementSpeed = 0.0f;

    [SerializeField] float jumpForce = 0.0f;
    [SerializeField,Tooltip("속도가 어느정도 되어야 캐릭터를 뒤집을 것인지 정합니다.")]
    float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 1.0f;
    [SerializeField] float fallGravityScale = 1.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private EdgeCollider2D playerSideCollider;

    private BlockType blockType;
    private LayerMask groundMask;
    private LayerMask wallMask;
    private LayerMask movingGroundMask;

    [SerializeField]
    private Vector2 movementInput;
    private Vector2 prevVelocity;
    [SerializeField] private Vector2 updatingVelocity;

    private bool jumpInput;
    private bool isFlipped;
    private bool isJumping;
    private bool isFalling;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;

    public bool CanMove { get; set; }

    void Start()
    {

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        updatingVelocity = playerRigidbody.velocity;
        groundMask = LayerMask.GetMask("Ground");
        wallMask = LayerMask.GetMask("Wall");
        movingGroundMask = LayerMask.GetMask("MovingGround");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");

        CanMove = true;
        EdgeColliderTest();
    }

    void EdgeColliderTest()
    {
        var offset = playerCollider.offset;
        var addPaddingX = 0.02f;
        var addRadius = 0.02f;
        playerSideCollider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] tempPoints =
        {
            new Vector2(playerCollider.bounds.extents.x+offset.x+addPaddingX-addRadius, playerCollider.bounds.extents.y+offset.y-addRadius),
            new Vector2(playerCollider.bounds.extents.x+offset.x+addPaddingX-addRadius, -playerCollider.bounds.extents.y+offset.y+addRadius),
            new Vector2(-playerCollider.bounds.extents.x-offset.x-addPaddingX+addRadius, playerCollider.bounds.extents.y+offset.y-addRadius),
            new Vector2(-playerCollider.bounds.extents.x-offset.x-addPaddingX+addRadius, -playerCollider.bounds.extents.y+offset.y+addRadius)
        };
        playerSideCollider.edgeRadius = 0.02f;
        playerSideCollider.points = tempPoints;
    }

    void Update()
    {
        var keyboard = Keyboard.current;

        if (!CanMove || keyboard == null)
            return;

        // Horizontal movement
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;

        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
            moveHorizontal = -1.0f;
        else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
            moveHorizontal = 1.0f;

        else if (keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed)
            moveVertical = -10.0f;

        movementInput = new Vector2(moveHorizontal, moveVertical);

        // Jumping input
        if (!isJumping && keyboard.spaceKey.wasPressedThisFrame)
            jumpInput = true;
    }

    void FixedUpdate()
    {
        UpdateGroundCheck();
        UpdateVelocity();
        UpdateDirection();
        UpdateJump();
        UpdateGravityScale();

        prevVelocity = playerRigidbody.velocity;
    }

    private void UpdateGroundCheck()
    {
        // 터칭 레이어로 체크
        if (playerCollider.IsTouchingLayers(groundMask))
            blockType = BlockType.GROUND;
        else if (playerCollider.IsTouchingLayers(wallMask))
            blockType = BlockType.WALL;
        else if (playerCollider.IsTouchingLayers())
            blockType = BlockType.MOVING_GROUND;
        else
            blockType = BlockType.NONE;

        //// Update animator
        animator.SetBool(animatorGroundedBool, blockType != BlockType.NONE);
    }

    private void UpdateVelocity()
    {
        updatingVelocity = playerRigidbody.velocity;

        updatingVelocity += movementInput * movementSpeed * Time.fixedDeltaTime;

        var saveMoveInputX = movementInput.x;

        // Clamp horizontal speed.
        if (movementInput.x != 0.0f)
        {
            updatingVelocity.x = Mathf.Clamp(updatingVelocity.x, -maxMovementSpeed, maxMovementSpeed);
        }


        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;


        // Assign back to the body.
        playerRigidbody.velocity = updatingVelocity;

        // Update animator running speed

        animator.SetFloat(animatorRunningSpeed, Mathf.Abs(saveMoveInputX));

        // Play audio
        //audioPlayer.PlaySteps(blockType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && playerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput && blockType != BlockType.NONE)
        {
            // Jump using impulse force
            playerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
            //audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && blockType != BlockType.NONE)
        {
            // 땅과 충돌했을 때 리지드바디가 멈추기 때문에, 벨로시티를 재설정
            if (resetSpeedOnLand)
            {
                prevVelocity.y = playerRigidbody.velocity.y;
                playerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;

            // Play audio
            //audioPlayer.PlayLanding(blockType);
        }
    }

    private void UpdateDirection()
    {
        //스케일 변경으로 flip
        if (playerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = Vector3.one;
        }
        else if (playerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = flippedScale;
        }
    }

    private void UpdateGravityScale()
    {
        // 정해놓은 그라비티 스케일로 설정
        var gravityScale = groundedGravityScale;

        if (blockType == BlockType.NONE)
        {
            //만약 땅에 닿아있는 상태가 아닐때 : 점프중이라면 점프 그라비티 스케일로, 아니라면 추락 그라비티 스케일로 변경
            gravityScale = playerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;
        }

        playerRigidbody.gravityScale = gravityScale;
    }
}
