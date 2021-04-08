using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecondPlayerController : Character
{

    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    //[SerializeField] CharacterAudio audioPlayer = null;

    [Header("그 외")]

    [Tooltip("GroundCheck에 쓸 컨택트필터입니다.")]
    public ContactFilter2D contactFilter_Ground;
    [SerializeField, Tooltip("점프키를 꾹 눌렀을 때, 해당하는 중력값으로 변합니다.")]
    float jumpGravityScale = 1.0f;

    //[SerializeField, Tooltip("떨어지고 있는 상태라면 해당하는 중력값으로 변합니다.")]
    //float fallGravityScale = 1.0f;

    [SerializeField, Tooltip("기본적인 중력값입니다. ")]
    float groundedGravityScale = 1.0f;

    public bool resetSpeedOnLand = false;

    [HideInInspector] public Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private EdgeCollider2D playerSideCollider;

    public eBlockType blockType;
    private int noPlayerMask;
    private LayerMask groundMask;
    private LayerMask wallMask;
    private LayerMask movingGroundMask;
    private LayerMask rainMask;
    [HideInInspector] public Vector2 prevVelocity;
    private Vector2 updatingVelocity;

    public bool jumpInput;
    public bool isJumping;
    public bool isGrounded;
    public bool isFalling;

    [HideInInspector] public int animatorGroundedBool;
    [HideInInspector] public int animatorWalkingBool;
    [HideInInspector] public int animatorJumpTrigger;
    //public int animatorFallingBool;

    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [HideInInspector] public InputManager inputManager;

    [HideInInspector]
    public float saveMoveInputX;


    [HideInInspector]
    public bool CanMove = true;


    //    public float angle;
    //    private float limitAngle = 0.1f; //기준치


    private bool isDie = false;

    #region 추가된 것

    [Tooltip("캐릭터가 오른쪽을 바라보고 있는가")]
    private bool isRight = false;

    [Tooltip("점프를 해야하는가")]
    private bool shouldJump = false;

    [Tooltip("UpdateVelocity에서 들어갈 추가적인 힘입니다.")]
    private Vector2 extraForce;

    [Tooltip("사다리 등을 올라가는 속도입니다.")]
    public float climbSpeed;

    [Tooltip("사다리에 올라탄 상태인가를 뜻합니다.")]
    public bool onLadder = false; //퍼블릭으로 변경함

    [Tooltip("사다리에 올라탄 상태에서 특정 방향으로 점프했는가를 뜻합니다.")]
    public bool onLadderJump = false;

    [Tooltip("사다리를 오르고 있는 상태인가를 뜻합니다.")]
    public bool isClimbLadder = false;

    #endregion


    private static SecondPlayerController instance;
    public static SecondPlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SecondPlayerController>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }

        Init();
    }
    private void Init()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        Init_ContactFilter();
    }

    private void Init_ContactFilter()
    {
        contactFilter_Ground.useLayerMask = true;
        contactFilter_Ground.useNormalAngle = true;
    }

    private void Start()
    {

    }

    void Update()
    {
        CheckMoveInput();
        CheckJumpInput();
    }
    void FixedUpdate()
    {
        GroundCheck();
        UpdateMoveVelocity();
        UpdateJumpVelocity();
        UpdateGravityScale();
    }



    private void CheckMoveInput()
    {
        //무브먼트 인풋을 0으로 초기화
        movementInput = Vector2.zero;

        if (InputManager.Instance.buttonMoveRight.isPressed) //오른쪽 이동
        {
            movementInput = Vector2.right;
            isRight = true;

        }
        else if (InputManager.Instance.buttonMoveLeft.isPressed) //왼쪽 이동
        {
            movementInput = Vector2.left;
            isRight = false;
        }

        //사다리에 올라탄 상태일때
        if (onLadder)
        {
            if (InputManager.Instance.buttonUp.isPressed)//위쪽 이동
            {
                movementInput.y = 1f;
                isClimbLadder = true;
            }
            else if (InputManager.Instance.buttonDown.isPressed) //아래쪽 이동
            {
                movementInput.y = -1f;
                isClimbLadder = true;
            }
        }

    }

    private void CheckJumpInput()
    {
        if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame)
        {
            if (isGrounded) //땅에 닿아있을 때와 사다리를 타는 상태일 때만 점프를 할 수 있습니다.
            {
                shouldJump = true;

            }
            if (isClimbLadder)
            {
                onLadderJump = true;
                shouldJump = true;
            }

        }
        #region 중력조절
        if (InputManager.Instance.buttonMoveJump.isPressed)
        {
            playerRigidbody.gravityScale = jumpGravityScale;
        }

        if (InputManager.Instance.buttonMoveJump.wasReleasedThisFrame)
        {
            playerRigidbody.gravityScale = groundedGravityScale;
        }
        #endregion
    }


    private void UpdateGravityScale()
    {
        if (isClimbLadder)
        {
            playerRigidbody.gravityScale = 0f;
            playerRigidbody.isKinematic = true;
        }
        else
        {
            playerRigidbody.isKinematic = false;
            if (isGrounded)
            {
                playerRigidbody.gravityScale = groundedGravityScale;
            }
            else
            {
                playerRigidbody.gravityScale = groundedGravityScale;
            }
        }



    }
    /// <summary>
    /// 이동 관련 벨로시티 업데이트.
    /// </summary>
    private void UpdateMoveVelocity()
    {
        if (isClimbLadder == false)
        {
            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, playerRigidbody.velocity.y + extraForce.y);
        }
        else
        {
            Debug.Log("사다리 이동");
            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, (movementInput.y * climbSpeed) + extraForce.y);
        }

    }

    /// <summary>
    /// 점프 관련 벨로시티 업데이트.
    /// </summary>
    private void UpdateJumpVelocity()
    {
        if (shouldJump)
        {
            if (onLadderJump)
            {
                Debug.Log("레더 점프");

                shouldJump = false;
                isClimbLadder = false;
                onLadderJump = false;

                // UpdateGravityScale();

                playerRigidbody.isKinematic = false;
                playerRigidbody.gravityScale = jumpGravityScale;
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);


            }
            else
            {
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);
                shouldJump = false;
            }

        }
    }

    /// <summary>
    /// 플레이어가 땅에 닿았는지 체크합니다.
    /// </summary>
    private void GroundCheck()
    {
        if (playerCollider.IsTouching(contactFilter_Ground))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex && CanMove == true)
        {
            ProcessDie();
        }
        else
        {

        }

    }

    public void ProcessDie()
    {
        if (isDie == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            CanMove = false;
            SiyeonManager.Instance.SetActiveTrueRestartUI();
            Time.timeScale = 0f;
        }

    }

    public void ProcessRaise()
    {
        playerRigidbody.rotation = 0f;
        CanMove = true;
    }
    public void SetIsLadder(bool _isLadder)
    {
        if (_isLadder == true)
        {
        }
        else
        {
            isClimbLadder = false;
        }
        onLadder = _isLadder;
    }

    /// <summary>
    /// 이동키 입력 등을 체크
    /// </summary>
    private void CheckInput()
    {


    }
    private void UpdateGroundCheck()
    {
        if (playerCollider.IsTouching(contactFilter_Ground))
        {
            isGrounded = true;
            blockType = eBlockType.GROUND;
        }
        else
        {
            isGrounded = false;
            blockType = eBlockType.NONE;
        }

        animator.SetBool(animatorGroundedBool, isGrounded);
    }

    //private void UpdateVelocity()
    //{
    //    updatingVelocity = playerRigidbody.velocity;

    //    updatingVelocity += movementInput * movementSpeed * Time.fixedDeltaTime;

    //    saveMoveInputX = movementInput.x;


    //    updatingVelocity.x = Mathf.Clamp(updatingVelocity.x, -maxMovementSpeed, maxMovementSpeed);

    //    movementInput = Vector2.zero;


    //    playerRigidbody.velocity = updatingVelocity;


    //    if (playerStateMachine.GetCurrentStateName() != "PlayerState_Default" && !isJumping && !isFalling)
    //    {
    //        playerStateMachine.ChangeState(eState.PLAYER_DEFAULT);
    //    }

    //}

    //private void UpdateJump()
    //{
    //    // Set falling flag
    //    if (isJumping && playerRigidbody.velocity.y < 0)
    //        isFalling = true;

    //    // Jump
    //    if (jumpInput && blockType != eBlockType.NONE)
    //    {
    //        playerStateMachine.ChangeState(eState.PLAYER_JUMP);

    //    }
    //    else if (isJumping && isFalling)
    //    {
    //        ////착지
    //        //if (blockType != eBlockType.NONE)
    //        //{
    //        if (isGrounded)
    //        {
    //            // 땅과 충돌했을 때 리지드바디가 멈추기 때문에, 벨로시티를 재설정
    //            if (resetSpeedOnLand)
    //            {
    //                prevVelocity.y = playerRigidbody.velocity.y;
    //                playerRigidbody.velocity = prevVelocity;
    //            }

    //            //착지판정
    //            isJumping = false;
    //            isFalling = false;

    //            //}

    //        }

    //    }
    //}

    private void UpdateDirection()
    {
        //스케일 변경으로 flip
        if (InputManager.Instance.buttonMoveRight.isPressed && playerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = Vector3.one;
        }
        else if (InputManager.Instance.buttonMoveLeft.isPressed && playerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = flippedScale;
        }
    }

    //private void UpdateGravityScale()
    //{
    //    // 정해놓은 그라비티 스케일로 설정
    //    var gravityScale = groundedGravityScale;

    //    if (blockType == eBlockType.NONE)
    //    {
    //        //만약 땅에 닿아있는 상태가 아닐때 : 점프중이라면 점프 그라비티 스케일로, 아니라면 추락 그라비티 스케일로 변경
    //        gravityScale =
    //            playerRigidbody.velocity.y > 0.0f ?
    //            jumpGravityScale : fallGravityScale;
    //    }

    //    playerRigidbody.gravityScale = gravityScale;
    //}
}


