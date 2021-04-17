using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class PlayerController : Character
{

    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    //[SerializeField] CharacterAudio audioPlayer = null;

    [Header("그 외")]

    [Tooltip("GroundCheck에 쓸 컨택트필터입니다.")]
    public ContactFilter2D contactFilter_Ground;

    //[SerializeField, Tooltip("점프키를 꾹 눌렀을 때, 해당하는 중력값으로 변합니다.")]
    //private float jumpGravityScale = 1.0f;

    //[SerializeField, Tooltip("떨어지고 있는 상태라면 해당하는 중력값으로 변합니다.")]
    //float fallGravityScale = 1.0f;

    [SerializeField, Tooltip("기본적인 중력값입니다. ")]
    private float groundedGravityScale = 1.0f;

    public bool resetSpeedOnLand = false;

    [HideInInspector] public Rigidbody2D playerRigidbody;

    private Collider2D playerCollider;
    private EdgeCollider2D playerSideCollider;

    public eBlockType blockType;
    private int noPlayerMask;
    private LayerMask groundMask;
    private LayerMask wallMask;
    private LayerMask movingGroundMask;
    private LayerMask playerMask;
    private LayerMask rainMask;

    [HideInInspector] public Vector2 prevVelocity;
    private Vector2 updatingVelocity;

    [HideInInspector] public bool jumpInput;
    public bool isJumping;
    public bool isGrounded;
    public bool isFalling;

    [HideInInspector] public int animatorGroundedBool;
    [HideInInspector] public int animatorWalkingBool;
    [HideInInspector] public int animatorJumpTrigger;
    [HideInInspector] public int animatorClimbBool;
    //public int animatorFallingBool;

    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [HideInInspector] public InputManager inputManager;

    [HideInInspector]
    public float saveMoveInputX;


    [HideInInspector]
    public bool canMove = true;



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

    [Tooltip("사다리에 닿은 상태로 상하키 입력을 했는가를 뜻합니다.")]
    public bool inLadder = false;

    [Tooltip("사다리에 닿은 !!! 상태인가를 뜻합니다.")]
    public bool onLadder = false; //퍼블릭으로 변경함

    [Tooltip("사다리에 올라탄 상태에서 특정 방향으로 점프했는가를 뜻합니다.")]
    [HideInInspector] public bool onLadderJump = false;

    [Tooltip("사다리를 오르고 있는 상태인가를 뜻합니다.")]
    public bool isClimbLadder = false;

    [Tooltip("현재 타고있는 사다리의 위치.")]
    [HideInInspector] public Vector2 ladderPosition = Vector2.zero;


    public TextMesh stateTextMesh;

    public Vector2 prevPosition;
    #endregion


    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
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
        playerStateMachine = new PlayerStateMachine(this);
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        puppet = gameObject.transform;

        playerMask = LayerMask.NameToLayer("Player");
        groundMask = LayerMask.NameToLayer("Ground");
        rainMask = LayerMask.NameToLayer("WeatherFx_withOpaqueTex");

        animatorWalkingBool = Animator.StringToHash("Walking");
        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorClimbBool = Animator.StringToHash("Climb");
        Init_ContactFilter();
    }

    private void Init_ContactFilter()
    {
        contactFilter_Ground.useLayerMask = true;
        contactFilter_Ground.useNormalAngle = true;
    }

    private void Start()
    {
        ChangeState(eState.PLAYER_DEFAULT);
        playerStateMachine.Start();
    }

    void Update()
    {
        CheckMoveInput();
        CheckLadderInput();
        CheckJumpInput();

        UpdateChangeState();
        playerStateMachine.Update();
        if (stateTextMesh != null)
        {
            UpdateStateTextMesh();
        }

    }
    void FixedUpdate()
    {
        GroundCheck();
        UpdateMoveVelocity();
        UpdateJumpVelocity();
        UpdateGravityScale();
        UpdateDirection();

        playerStateMachine.FixedUpdate();

    }

    //TEST
    private void UpdateStateTextMesh()
    {
        stateTextMesh.gameObject.transform.position = playerRigidbody.position;
        var _text = playerStateMachine.GetCurrentStateName();
        stateTextMesh.text = _text;
    }
    #region 이동, 점프, 사다리
    private void CheckMoveInput()
    {
        //무브먼트 인풋을 0으로 초기화
        movementInput = Vector2.zero;

        if (true)
        {
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
        }

    }

    private void CheckLadderInput()
    {
        //사다리에 닿은!!! 상태일때
        if (onLadder)
        {
            if (InputManager.Instance.buttonUp.wasPressedThisFrame
                || InputManager.Instance.buttonDown.wasPressedThisFrame)
            {
                inLadder = true;
                isClimbLadder = true;
                isJumping = false;

            }
        }

        if (inLadder)
        {
            if (InputManager.Instance.buttonUp.isPressed)//위쪽 이동
            {
                movementInput.y = 1f;
                isClimbLadder = true;
                //사다리의 가운데 부분으로 이동시키기 위해서...

                playerRigidbody.position = (new Vector2(ladderPosition.x, playerRigidbody.position.y));
                //if (prevPosition != playerRigidbody.position)
                //{

                //    ChangeState(eState.PLAYER_CLIMB_LADDER);
                //}

            }
            else if (InputManager.Instance.buttonDown.isPressed) //아래쪽 이동
            {
                movementInput.y = -1f;
                isClimbLadder = true;
                //사다리의 가운데 부분으로 이동시키기 위해서...
                playerRigidbody.position = (new Vector2(ladderPosition.x, playerRigidbody.position.y));
                //if (prevPosition != playerRigidbody.position)
                //{

                //    ChangeState(eState.PLAYER_CLIMB_LADDER);
                //}
            }
        }

    }

    /// <summary>
    /// 점프 입력을 감지
    /// </summary>
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
        //if (InputManager.Instance.buttonMoveJump.isPressed)
        //{
        //    playerRigidbody.gravityScale = jumpGravityScale;
        //}

        //if (InputManager.Instance.buttonMoveJump.wasReleasedThisFrame)
        //{
        //    playerRigidbody.gravityScale = groundedGravityScale;
        //}
        #endregion
    }

    #endregion

    /// <summary>
    /// 플레이어가 땅에 닿았는지 체크합니다.
    /// </summary>
    private void GroundCheck()
    {
        if (playerCollider.IsTouching(contactFilter_Ground))
        {
            if (playerRigidbody.velocity.y > 0f)
            {
                //isJumping = false;
            }
            else
            {
                isGrounded = true;
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //ChangeState(eState.PLAYER_DEFAULT);
            }
        }
        else
        {
            isGrounded = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }
    }

    private void UpdateChangeState()
    {
        if (inLadder && prevPosition.y != playerRigidbody.position.y)
        {
            ChangeState(eState.PLAYER_CLIMB_LADDER);
        }
        else if (isGrounded && !isJumping)
        {
            ChangeState(eState.PLAYER_DEFAULT);
        }
        else if (!isGrounded && isJumping)
        {
            ChangeState(eState.PLAYER_JUMP);
        }
        else if (!inLadder && !isGrounded && !onLadder)
        {
            ChangeState(eState.PLAYER_DEFAULT);
        }

    }
    private void ChangeState(eState _state)
    {
        playerStateMachine.ChangeState(_state);
    }

    /// <summary>
    /// 이동 관련 벨로시티 업데이트.
    /// </summary>
    private void UpdateMoveVelocity()
    {
        prevPosition = new Vector2(playerRigidbody.position.x, playerRigidbody.position.y);
        if (isClimbLadder == false)
        {
            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, playerRigidbody.velocity.y + extraForce.y);
        }
        else
        {
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

                isClimbLadder = false;
                onLadderJump = false;
                inLadder = false;

                // UpdateGravityScale();
                //playerRigidbody.gravityScale = groundedGravityScale;


                //오른쪽을 보고있으면 1f, 아니면 -1f
                float jumpX = isRight ? 1f : -1f;
                //필요하다면 쓰기.


                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);
                shouldJump = false;

                ChangeState(eState.PLAYER_JUMP);

                isJumping = true;
                isGrounded = false;
            }
            else
            {
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);

                shouldJump = false;

                ChangeState(eState.PLAYER_JUMP);

                isJumping = true;
                isGrounded = false;
            }

        }
    }


    private void UpdateGravityScale()
    {
        if (isClimbLadder)
        {
            playerRigidbody.gravityScale = 0f;
            Physics2D.IgnoreLayerCollision(groundMask, playerMask, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(groundMask, playerMask, false);
            if (isGrounded)
            {
                playerRigidbody.gravityScale = groundedGravityScale;
            }
            else if (!isClimbLadder)
            {
                playerRigidbody.gravityScale = groundedGravityScale;
            }
            //if (isGrounded && !isJumping)
            //{
            //    playerRigidbody.gravityScale = groundedGravityScale;
            //}
            //else if( isJumping)
            //{
            //    playerRigidbody.gravityScale = jumpGravityScale;
            //}
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex && canMove == true)
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
            canMove = false;
            SiyeonManager.Instance.SetActiveTrueRestartUI();
            Time.timeScale = 0f;
        }

    }
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

    public void ProcessRaise()
    {
        playerRigidbody.rotation = 0f;
        canMove = true;
    }
    public void SetIsLadder(bool _isLadder)
    {
        if (_isLadder == true)
        {
            //Physics2D.IgnoreLayerCollision(groundMask, playerMask, true);
        }
        else
        {
            isClimbLadder = false;
            inLadder = false;
            //Physics2D.IgnoreLayerCollision(groundMask, playerMask, false);
            UpdateGravityScale();
        }
        onLadder = _isLadder;
    }

    public void SetIsLadder(bool _isLadder, Vector2 _pos)
    {
        if (_isLadder == true)
        {
            ladderPosition = _pos;
            Physics2D.IgnoreLayerCollision(groundMask, playerMask, true);
        }
        else
        {
            isClimbLadder = false;
            inLadder = false;
            ladderPosition = Vector2.zero;
            Physics2D.IgnoreLayerCollision(groundMask, playerMask, false);
            UpdateGravityScale();

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

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(new Vector2(transform.position.x, transform.position.y), new Vector2(5, 5)),"Hi");
    //}


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


