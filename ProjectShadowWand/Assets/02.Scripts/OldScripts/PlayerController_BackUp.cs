using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_BackUp : Character
{

    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    //[SerializeField] CharacterAudio audioPlayer = null;

    [Header("�� ��")]

    [Tooltip("GroundCheck�� �� ����Ʈ�����Դϴ�.")]
    public ContactFilter2D contactFilter_Ground;

    public Transform[] childPostion;


    [SerializeField] float jumpGravityScale = 1.0f;
    [SerializeField] float fallGravityScale = 1.0f;
    [SerializeField] float groundedGravityScale = 1.0f;

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

    [SerializeField]
    private bool isLadder = false;

    private bool onLadder = false;
    //    public float angle;
    //    private float limitAngle = 0.1f; //����ġ

    public GameObject lightExplosionObject;


    private bool isDie = false;
    static PlayerController_BackUp instance;
    public static PlayerController_BackUp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController_BackUp>();
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
    void Init()
    {

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        updatingVelocity = playerRigidbody.velocity;
        groundMask = LayerMask.GetMask("Ground");
        wallMask = LayerMask.GetMask("Wall");
        movingGroundMask = LayerMask.GetMask("MovingGround");
        noPlayerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorWalkingBool = Animator.StringToHash("Walking");
        animatorJumpTrigger = Animator.StringToHash("Jump");

        inputManager = InputManager.Instance;

        isLadder = false;
        onLadder = false;
        rainMask = LayerMask.GetMask(eLayer.WeatherFx_withOpaqueTex.ToString());
        //  EdgeColliderTest();


       // playerStateMachine = new PlayerStateMachine(this);

    }

    private void Start()
    {
        playerStateMachine.ChangeState(eState.PLAYER_DEFAULT);
        playerStateMachine.Start();
    }

    void Update()
    {
        if (!CanMove)
            return;

        //if (inputManager.keyboard.tKey.wasPressedThisFrame)
        //{
        //    lightExplosionObject.SetActive(true);
        //}

        if (isLadder) //��ٸ��� ���� ��
        {
            CheckInput_LadderMove();
        }
        else
        {
            CheckInput();
        }
        playerStateMachine.Update();
    }
    void FixedUpdate()
    {
        UpdateGroundCheck();
        playerStateMachine.FixedUpdate();
        UpdateVelocity();
        UpdateDirection();
        UpdateJump();
        UpdateGravityScale();

        prevVelocity = playerRigidbody.velocity;
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
    public void CheckInput_LadderMove()
    {
        float moveVertical = 0f;

        if (inputManager.buttonDown.isPressed)
        {
            onLadder = true;

            moveVertical = -1f;
            movementInput = new Vector2(0f, moveVertical);

            if (isGrounded)
            {
                playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
                CheckInput();
            }
        }
        else if (inputManager.buttonUp.isPressed)
        {
            onLadder = true;

            moveVertical = 1f;
            movementInput = new Vector2(0f, moveVertical);
            LadderHelper();
        }
        else
        {
            playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
            CheckInput();
        }




    }
    public void SetIsLadder(bool _isLadder)
    {
        isLadder = _isLadder;
        if (_isLadder == false && onLadder == true)
        {
            playerRigidbody.velocity = Vector2.zero;
        }
    }

    private void LadderHelper()
    {

        if (movementInput != Vector2.zero)
        {
            playerRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        // Jumping input
        if (!isJumping && inputManager.buttonMoveJump.wasPressedThisFrame)
        {
            jumpInput = true;
            CheckInput();
        }
    }
    /// <summary>
    /// �̵�Ű �Է� ���� üũ
    /// </summary>
    private void CheckInput()
    {
        onLadder = false;
        playerRigidbody.bodyType = RigidbodyType2D.Dynamic;
        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;

        if (inputManager.buttonMoveLeft.isPressed)
        {
            moveHorizontal = -1.0f;
        }
        else if (inputManager.buttonMoveRight.isPressed)
        {
            moveHorizontal = 1.0f;
        }
        //else if (inputManager.buttonDown.isPressed)
        //{
        //    moveVertical = -10.0f;
        //}
        else
        {
            moveHorizontal = 0.0f;
            moveVertical = 0.0f;
        }

        movementInput = new Vector2(moveHorizontal, moveVertical);

        // Jumping input
        if (!isJumping && inputManager.buttonMoveJump.wasPressedThisFrame)
            jumpInput = true;

    }
    private void UpdateGroundCheck()
    {
        #region TestGroundCheck
        //if (playerCollider.IsTouchingLayers(groundMask) && angle >= limitAngle)
        //{
        //    blockType = eBlockType.GROUND;
        //}
        //else if (playerCollider.IsTouchingLayers(wallMask))
        //{
        //    blockType = eBlockType.WALL;
        //}
        //else if (playerCollider.IsTouchingLayers())
        //{
        //    blockType = eBlockType.MOVING_GROUND;
        //}
        //else
        //{
        //    blockType = eBlockType.NONE;

        //}

        //if (blockType != eBlockType.NONE)
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
        #endregion




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

        ////Ladder
        //if (playerCollider.IsTouchingLayers(LayerMask.NameToLayer("Ladder")))
        //{
        //    isLadder = true;
        //    blockType = eBlockType.LADDER;
        //}
        //else
        //{

        //    isLadder = false;
        //}

        animator.SetBool(animatorGroundedBool, isGrounded);
    }

    private void UpdateVelocity()
    {
        updatingVelocity = playerRigidbody.velocity;

        updatingVelocity += movementInput * movementSpeed * Time.fixedDeltaTime;

        saveMoveInputX = movementInput.x;


        updatingVelocity.x = Mathf.Clamp(updatingVelocity.x, -maxMovementSpeed, maxMovementSpeed);

        movementInput = Vector2.zero;


        playerRigidbody.velocity = updatingVelocity;


        if (playerStateMachine.GetCurrentStateName() != "PlayerState_Default" && !isJumping && !isFalling)
        {
            playerStateMachine.ChangeState(eState.PLAYER_DEFAULT);
        }

        // Play audio
        //audioPlayer.PlaySteps(blockType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && playerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput && blockType != eBlockType.NONE)
        {
            playerStateMachine.ChangeState(eState.PLAYER_JUMP);

        }
        else if (isJumping && isFalling)
        {
            ////����
            //if (blockType != eBlockType.NONE)
            //{
            if (isGrounded)
            {
                // ���� �浹���� �� ������ٵ� ���߱� ������, ���ν�Ƽ�� �缳��
                if (resetSpeedOnLand)
                {
                    prevVelocity.y = playerRigidbody.velocity.y;
                    playerRigidbody.velocity = prevVelocity;
                }

                //��������
                isJumping = false;
                isFalling = false;

                //}

            }

        }
    }

    private void UpdateDirection()
    {
        //������ �������� flip
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

    private void UpdateGravityScale()
    {
        // ���س��� �׶��Ƽ �����Ϸ� ����
        var gravityScale = groundedGravityScale;

        if (blockType == eBlockType.NONE)
        {
            //���� ���� ����ִ� ���°� �ƴҶ� : �������̶�� ���� �׶��Ƽ �����Ϸ�, �ƴ϶�� �߶� �׶��Ƽ �����Ϸ� ����
            gravityScale =
                playerRigidbody.velocity.y > 0.0f ?
                jumpGravityScale : fallGravityScale;
        }

        playerRigidbody.gravityScale = gravityScale;
    }

    #region TestCollisionStay
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        //ContactPoint2D test = collision.GetContact(0);
    //        //TestObject.transform.position = test.point;
    //        //������ ���
    //        Vector2 cOffset = collision.gameObject.transform.position;
    //        //Vector2 topPos = new Vector2(collision.collider.bounds.center.x, collision.collider.bounds.max.y);
    //        //Vector2 topPosUp = topPos * Vector2.up;

    //        Vector2 topPos = collision.transform.position;
    //        Vector2 topPosUp = collision.transform.up;

    //        Vector2 pOffset = playerRigidbody.position;
    //        Vector2 playerBottomPos = new Vector2(pOffset.x, playerCollider.bounds.min.y);
    //        Vector2 finalPlyBtPos = playerBottomPos - topPos;

    //        Debug.DrawRay(topPos, Vector2.up, Color.red);
    //        angle = Vector2.Dot(finalPlyBtPos, topPosUp);


    //        if (angle >= limitAngle)
    //        {
    //            Debug.DrawRay(topPos, finalPlyBtPos, Color.blue);

    //        }
    //        //Vector2 playerBottomPos = new Vector2(playerRigidbody.position.x, playerCollider.bounds.min.y);
    //        //RaycastHit2D hit = Physics2D.Raycast(playerBottomPos, Vector2.down, 30f, noPlayerMask);

    //        //angle = Vector2.Angle(hit.normal, Vector2.up);
    //        ////Debug.DrawRay(playerBottomPos, Vector2.down * 30f, Color.magenta);

    //        //Debug.DrawRay(hit.point, hit.normal*30f, Color.blue);

    //    }


    //}

    #endregion



    #region TestTopCheck
    //private void TestTopCheck()
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, noPlayerMask);

    //    var currentPos = hit.collider.transform.position;
    //    var currentUp = hit.collider.transform.up;
    //    var playerPos = transform.position;
    //    if (playerCollider.IsTouching(hit.collider))
    //    {
    //        if (Vector3.Dot(hit.collider.transform.up, transform.position - currentPos) >= 0)
    //        {
    //            Debug.DrawLine(currentPos, currentPos + currentUp, Color.red);
    //            Debug.DrawLine(currentPos, currentPos + (playerPos - currentPos), Color.blue);


    //        }
    //    }
    //}
    #endregion

    #region TestCreateEdgeCollider
    //void EdgeColliderTest()
    //{
    //    var offset = playerCollider.offset;
    //    var addPaddingX = 0.02f;
    //    var addRadius = 0.02f;
    //    playerSideCollider = gameObject.AddComponent<EdgeCollider2D>();
    //    Vector2[] tempPoints =
    //    {
    //        new Vector2(playerCollider.bounds.extents.x+offset.x+addPaddingX-addRadius, playerCollider.bounds.extents.y+offset.y-addRadius),
    //        new Vector2(playerCollider.bounds.extents.x+offset.x+addPaddingX-addRadius, -playerCollider.bounds.extents.y+offset.y+addRadius),
    //        new Vector2(-playerCollider.bounds.extents.x-offset.x-addPaddingX+addRadius, playerCollider.bounds.extents.y+offset.y-addRadius),
    //        new Vector2(-playerCollider.bounds.extents.x-offset.x-addPaddingX+addRadius, -playerCollider.bounds.extents.y+offset.y+addRadius)
    //    };
    //    playerSideCollider.edgeRadius = 0.02f;
    //    playerSideCollider.points = tempPoints;
    //}
    #endregion
}
