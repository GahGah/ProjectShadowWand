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

    [Header("바람 관련 속도")]
    [Tooltip("느려지는 속도입니다. 기본 속도에 -계산을 합니다.")]
    public float windDecreaseSpeed;
    [Tooltip("빨라지는 속도입니다. 기본 속도에 +계산을 합니다.")]
    public float windIncreaseSpeed;
    [Tooltip("기본적으로 밀리는 속도입니다. 기본 속도에 +계산을 합니다.")]
    public float windSlideSpeed;

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


    [HideInInspector] public Collider2D playerCollider;
    private EdgeCollider2D playerSideCollider;


    [HideInInspector] public eBlockType blockType;
    private int noPlayerMask;

    private LayerMask wallMask;
    private LayerMask movingGroundMask;
    private LayerMask playerMask;
    private LayerMask rainMask;

    private LayerMask groundMask;
    private LayerMask groundSoftMask;
    private LayerMask groundHardMask;


    [HideInInspector] public Vector2 prevVelocity;
    private Vector2 updatingVelocity;

    [HideInInspector] public bool jumpInput;

    [HideInInspector] public bool isJumping;

    [HideInInspector] public bool isGrounded;

    [HideInInspector] public bool isFalling;

    [HideInInspector] public int animatorGroundedBool;
    [HideInInspector] public int animatorWalkingBool;
    [HideInInspector] public int animatorJumpTrigger;
    [HideInInspector] public int animatorClimbBool;
    [HideInInspector] public int animatorPushingBool;
    [HideInInspector] public int animatorLiftingBool;
    //public int animatorFallingBool;

    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [HideInInspector] public InputManager inputManager;

    [HideInInspector] public float saveMoveInputX;


    [HideInInspector] public bool canMove = true;


    public bool isDie = false;

    #region 추가된 것
    [Tooltip("캐릭터가 물과 닿았는가")]
    private bool isWater = false;

    [HideInInspector] public float animatorWindBlend;

    [Tooltip("캐릭터가 오른쪽을 바라보고 있는가"), SerializeField]
    private bool isRight = false;

    [Tooltip("점프를 해야하는가")]
    private bool shouldJump = false;

    [Tooltip("UpdateVelocity에서 들어갈 추가적인 힘입니다.")]
    private Vector2 extraForce;

    [Header("사다리 이동 속도"), Tooltip("사다리 등을 올라가는 속도입니다.")]
    public float climbSpeed;

    [Tooltip("사다리에 닿은 상태로 상하키 입력을 했는가를 뜻합니다.")]
    [HideInInspector] public bool inLadder = false;

    [Tooltip("사다리에 닿은 !!! 상태인가를 뜻합니다.")]
    [HideInInspector] public bool onLadder = false; //퍼블릭으로 변경함

    [Tooltip("사다리에 올라탄 상태에서 특정 방향으로 점프했는가를 뜻합니다.")]
    [HideInInspector] public bool onLadderJump = false;

    [Tooltip("사다리를 오르고 있는 상태인가를 뜻합니다.")]
    [HideInInspector] public bool isClimbLadder = false;

    [Tooltip("현재 타고있는 사다리의 위치.")]
    [HideInInspector] public Vector2 ladderPosition = Vector2.zero;

    [HideInInspector] public Vector2 prevPosition;
    [HideInInspector] public float originalMoveSpeed;

    [Tooltip("테스트용 idle 모션입니다.")]
    public Motion[] testIdleMotions;

    private Joint2D currentCatchJoint = null;


    [Header("밀기 시 이동속도")]
    public float pushMoveSpeed;

    [Tooltip("잡기 키를 눌렀는가?")]
    [HideInInspector] public bool isInputCatchKey = false;

    [Tooltip("밀기 키를 눌렀는가?")]
    [HideInInspector] public bool isInputPushKey = false;


    [Tooltip("미는 중인가?")]
    public bool isPushing = false;

    [Tooltip("물건을 잡았나?")]
    public bool isCatching = false;

    [Header("밀기/잡기 시 손의 위치"), Space(10)]
    public GameObject handPosition_Push;
    public GameObject handPosition_Catch;

    [SerializeField]
    [HideInInspector] private CatchableObject catchedObject = null;
    [SerializeField]
    [HideInInspector] private PushableObject pushedObject = null;
    [SerializeField]
    [HideInInspector] public GameObject touchedObject = null;

    [HideInInspector] public Rigidbody2D catchBody;

    public bool isWinding;

    public eWindDirection windDirection;


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
    private void Start()
    {

        ChangeState(eState.PLAYER_DEFAULT);
        playerStateMachine.Start();

    }
    private void Init()
    {
        playerStateMachine = new PlayerStateMachine(this);
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        puppet = gameObject.transform;

        playerMask = LayerMask.NameToLayer("Player");
        groundMask = LayerMask.NameToLayer("Ground");// LayerMask.NameToLayer("Ground_Soft") LayerMask.NameToLayer("Ground_Hard");

        groundSoftMask = LayerMask.NameToLayer("Ground_Soft");
        groundHardMask = LayerMask.NameToLayer("Ground_Hard");
        rainMask = LayerMask.NameToLayer("WeatherFx_withOpaqueTex");

        animatorWalkingBool = Animator.StringToHash("Walking");
        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorClimbBool = Animator.StringToHash("Climb");
        animatorWindBlend = Animator.StringToHash("WindBlend");
        animatorLiftingBool = Animator.StringToHash("Lifting");
        animatorPushingBool = Animator.StringToHash("Pushing");

        isWater = false;
        currentCatchJoint = null;
        isInputCatchKey = false;

        catchedObject = null;
        pushedObject = null;
        touchedObject = null;

        originalMoveSpeed = movementSpeed;
        animator.SetFloat("WindBlend", 0);
        if (stateTextMesh == null)
        {
            CreateStateTextMesh();
        }
        Init_ContactFilter();
    }

    private void Init_ContactFilter()
    {
        contactFilter_Ground.useLayerMask = true;
        contactFilter_Ground.useNormalAngle = true;
    }


    void Update()
    {
        CheckCatchAndPush();
        CheckMoveInput();
        CheckLadderInput();
        CheckJumpInput();

        UpdateChangeState();
        playerStateMachine.Update();

        UpdateStateTextMesh();

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
    #region 이동, 점프, 사다리
    private void CheckMoveInput()
    {
        //무브먼트 인풋을 0으로 초기화
        movementInput = Vector2.zero;

        if (CanMove())
        {
            if (InputManager.Instance.buttonMoveRight.isPressed) //오른쪽 이동
            {
                movementInput = Vector2.right;
                if (pushedObject == null)
                {
                    isRight = true;
                }


            }
            else if (InputManager.Instance.buttonMoveLeft.isPressed) //왼쪽 이동
            {
                movementInput = Vector2.left;
                if (pushedObject == null)
                {
                    isRight = false;
                }
            }
        }

    }

    private void CheckLadderInput()
    {
        if (CanMove())
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
      

    }

    /// <summary>
    /// 점프 입력을 감지
    /// </summary>
    private void CheckJumpInput()
    {
        if (CanMove())
        {
            if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame)
            {
                if (pushedObject == null)
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


    #region 밀기/잡기
    private void CheckCatchAndPush() //최적화가 필요함
    {
        if (CanMove())
        {
            if (pushedObject != null)//뭔가 밀고 있는 오브젝트가 있을 때
            {

                isPushing = true;
                var tempVelo = pushedObject.rigidBody.velocity;
                var testSpeed = movementSpeed;
                if (isRight)
                {
                    if (movementInput == Vector2.left)
                    {
                        pushedObject.rigidBody.velocity = Vector2.zero;
                        tempVelo += Vector2.left;

                        var vec = new Vector2(Mathf.Clamp(tempVelo.x, -testSpeed, testSpeed), Mathf.Clamp(tempVelo.y, -testSpeed, testSpeed));
                        pushedObject.rigidBody.velocity += vec;
                        //pushedObject.rigidBody.velocity = playerRigidbody.velocity;
                    }
                }
                else
                {
                    if (movementInput == Vector2.right)
                    {
                        //  pushedObject.rigidBody.velocity += (Vector2.right);;
                        pushedObject.rigidBody.velocity = Vector2.zero;
                        tempVelo += Vector2.right;

                        var vec = new Vector2(Mathf.Clamp(tempVelo.x, -testSpeed, testSpeed), Mathf.Clamp(tempVelo.y, -testSpeed, testSpeed));
                        pushedObject.rigidBody.velocity += vec;
                        //pushedObject.rigidBody.velocity = playerRigidbody.velocity;
                    }
                }

            }
            else
            {
                isPushing = false;
            }

            if (catchedObject != null)
            {
                isCatching = true;
            }
            else
            {
                isCatching = false;
            }

        }


        //}

    }
    public void CheckCatchInput(CatchableObject _obj)
    {
        if (CanMove())
        {
            var tempObj = _obj;

            if (InputManager.Instance.buttonCatch.wasPressedThisFrame)// 키 누르기
            {
                if (catchedObject == null) //잡아야 할 경우
                {
                    if (touchedObject != null)
                    {
                        SetCatchedObject(tempObj);
                        if (catchedObject != null)
                        {
                            catchedObject.GoCatchThis();
                        }
                    }

                }
                else //놓아야 함
                {
                    catchedObject.GoPutThis();

                    catchedObject = null;
                }
            }
            else
            {

                if (InputManager.Instance.buttonCatch.isPressed) //키 계속 누르기 
                {
                    //딱히...?
                }

                if (InputManager.Instance.buttonCatch.wasReleasedThisFrame) // 키 떼기
                {
                    //얘도 딱히...
                }
            }
        }

    }

    public void CheckPushInput(PushableObject _obj)
    {
        if (CanMove())
        {
            var tempObj = _obj;

            if (InputManager.Instance.buttonPush.wasPressedThisFrame)// 키 누르기
            {
                if (pushedObject == null) //밀어야 할 경우
                {
                    if (touchedObject != null)
                    {
                        SetPushedObject(tempObj);

                        if (pushedObject != null)
                        {
                            pushedObject.GoPushReady();
                        }
                    }
                }
            }

            if (InputManager.Instance.buttonPush.wasReleasedThisFrame) // 키 떼기
            {
                if (pushedObject != null)
                {
                    pushedObject.GoPutThis();
                    pushedObject = null;
                }
            }
            if (InputManager.Instance.buttonPush.isPressed) //키 계속 누르기 
            {
                if (pushedObject == null) //밀어야 할 경우
                {
                    if (touchedObject != null)
                    {
                        SetPushedObject(tempObj);

                        if (pushedObject != null)
                        {
                            pushedObject.GoPushReady();

                        }
                    }
                }
                else
                {
                    pushedObject.GoPushThis();
                }
            }

        }


    }



    public void SetPushedObject(PushableObject _po)
    {
        pushedObject = _po;
    }

    public void SetCatchedObject(CatchableObject _co)
    {
        catchedObject = _co;
    }

    public PushableObject GetPushedObject()
    {
        return pushedObject;
    }

    public CatchableObject GetCatchedObject()
    {
        return catchedObject;
    }

    public void SetTouchedObject(GameObject _go)
    {
        touchedObject = _go;
    }
    public GameObject GetTouchedObject()
    {
        return touchedObject;
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
        else if (isWater == true) // 아니면 물 속?
        {
            isGrounded = true;
            isJumping = false;

            animator.SetBool(animatorGroundedBool, isGrounded);
        }
        else
        {
            isGrounded = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }
    }

    private void UpdateChangeState()
    {
        if (isDie)
        {
            ChangeState(eState.PLAYER_DIE);
        }
        else if (inLadder && prevPosition.y != playerRigidbody.position.y) //사다리 안쪽에 있고, 위로 올라갔다면
        {
            if (catchedObject != null)//그런데 물체를 들고있다면
            {
                catchedObject.GoPutThis();
                catchedObject = null;
                //물체를 일단 내려놓음
            }

            ChangeState(eState.PLAYER_CLIMB_LADDER); // 사다리상태로 변경
        
        }
        else if (isPushing)
        {
            ChangeState(eState.PLAYER_PUSH);
        }
        else if (isCatching)
        {
            ChangeState(eState.PLAYER_LIFT);
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

        if (isWinding)
        {
            animator.SetFloat("WindBlend", 1);

            if (windDirection == eWindDirection.RIGHT) //오른쪽으로 -> 바람이 불면
            {
                if (movementInput == Vector2.left) //왼쪽으로 이동하고 싶을 때
                {
                    extraForce.x = windDecreaseSpeed; //저항이 생김
                }
                else if (movementInput == Vector2.right) //오른쪽으로 이동하고 싶을 때
                {
                    extraForce.x = windIncreaseSpeed;//속도가 오름
                }
                else if (movementInput == Vector2.zero)
                {
                    extraForce.x = windSlideSpeed;
                }

            }
            else if (windDirection == eWindDirection.LEFT) //왼쪽으로 <- 바람이 불면
            {
                if (movementInput == Vector2.left) //왼쪽으로 이동하고 싶을 때
                {
                    extraForce.x = windIncreaseSpeed; //속도가 오름
                }
                else if (movementInput == Vector2.right) //오른쪽으로 이동하고 싶을 때
                {
                    extraForce.x = windDecreaseSpeed;//저항이 생김
                }
                else if (movementInput == Vector2.zero)
                {
                    extraForce.x = -windSlideSpeed;
                }
            }
            else
            {
                extraForce = Vector2.zero;
            }
        }
        else
        {
            animator.SetFloat("WindBlend", 0);
            extraForce = Vector2.zero;
        }


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
    new Vector2(playerRigidbody.velocity.x + extraForce.x, jumpForce + extraForce.y);
                shouldJump = false;

                ChangeState(eState.PLAYER_JUMP);

                isJumping = true;
                isGrounded = false;
            }
            else
            {
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x + extraForce.x, jumpForce + extraForce.y);

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
            SetIgnoreGroundCollision(true);
        }
        else
        {
            SetIgnoreGroundCollision(false);
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
        //if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex && canMove == true)
        //{
        //    ProcessDie();
        //}
        //else
        //{

        //}

    }

    public void ProcessDie()
    {
        if (isDie == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            canMove = false;
            //SiyeonManager.Instance.SetActiveTrueRestartUI();
            Time.timeScale = 0f;
        }

    }
    private void UpdateDirection()
    {
        //스케일 변경으로 flip

        if (isPushing == false)
        {
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
        else //isPushing이 트루일때
        {

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
            SetIgnoreGroundCollision(true);
        }
        else
        {
            isClimbLadder = false;
            inLadder = false;
            ladderPosition = Vector2.zero;
            SetIgnoreGroundCollision(false);
            UpdateGravityScale();

        }
        onLadder = _isLadder;
    }
    public void SetIgnoreGroundCollision(bool _b)
    {
        Physics2D.IgnoreLayerCollision(groundMask, playerMask, _b);
        Physics2D.IgnoreLayerCollision(groundHardMask, playerMask, _b);
        Physics2D.IgnoreLayerCollision(groundSoftMask, playerMask, _b);

    }
    public void SetExtraForce(Vector2 _force)
    {
        extraForce = _force;
    }
    public void SetIsWater(bool _isWater)
    {
        isWater = _isWater;
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

    /// <summary>
    /// 스테이트 제대로 돌아가는지 테스트용의 텍스트 메쉬
    /// </summary>
    private TextMesh stateTextMesh = null;
    private void CreateStateTextMesh()
    {
        GameObject tempObject = new GameObject("stateTextMesh(Created)");
        tempObject.transform.localScale = new Vector3(0.3f, 0.3f);
        tempObject.transform.position = new Vector3(0f, 0f, -9f);

        stateTextMesh = tempObject.AddComponent<TextMesh>();

        stateTextMesh.fontSize = 20;
        stateTextMesh.characterSize = 0.5f;

        stateTextMesh.anchor = TextAnchor.MiddleCenter;
        stateTextMesh.alignment = TextAlignment.Center;
        stateTextMesh.color = Color.white;

    }
    private void UpdateStateTextMesh()
    {
        stateTextMesh.gameObject.transform.position = new Vector3(playerRigidbody.position.x, playerRigidbody.position.y + 0.8f, -9f);
        var _text = playerStateMachine.GetCurrentStateName();
        stateTextMesh.text = _text;
    }

    /// <summary>
    /// 잡을 수 있는 오브젝트의 FixedJoint를 설정합니다.
    /// </summary>
    /// <param name="_fixedJoint">이걸로 설정합니다.</param>
    public void SetCurrentJointThis(Joint2D _fixedJoint)
    {
        currentCatchJoint = _fixedJoint;
    }


    public Joint2D GetCurrentCatchJoint()
    {
        return currentCatchJoint;
    }


    public bool CanMove()
    {
        if (isDie)
        {
            return false;
        }
        return true;
    }
    //public GameObject GetCatchingObject()
    //{
    //    return catchingObject;
    //}

    //public void SetCatchingObject(GameObject _gameObject)
    //{
    //    catchingObject = _gameObject;
    //}
    ///// <summary>
    ///// 커런트조인트에 있는 것을 잡습니다.
    ///// </summary>
    //public void CatchCurrentJoint()
    //{
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    WeatherInteractionObject tempObject = collision.gameObject.GetComponent<WeatherInteractionObject>();
    //    if (collision is ICatchable)
    //    {

    //    }
    //}
    #region 구버전
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

    #endregion
}


