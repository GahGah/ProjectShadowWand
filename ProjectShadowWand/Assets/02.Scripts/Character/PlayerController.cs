using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerController : Character
{

    #region 변수 짱 많아

    [Header("사다리 이동 속도"), Tooltip("사다리 등을 올라가는 속도입니다.")]
    public float climbSpeed;

    [Tooltip("디버그모드입니다.")]
    public bool isDebug;
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    public GameObject landedFX;
    public GameObject flipFX;
    public IEnumerator GlideCoroutine;
    public IEnumerator WaterCoroutine;
    public IEnumerator LightningCoroutine;

    [Header("활강 관련")]

    [Tooltip("활강하는 각도입니다.")]
    public float glideAngle;
    private float currentGlideAngle;
    [Tooltip("활강하는 속도입니다.")]
    public float glideSpeed;

    [Tooltip("활강할 수 있는 시간입니다.")]
    public float glideTime;
    private float currentGlideTIme;


    [Header("물 관련")]

    [Tooltip("물 한 칸(?)의 크기입니다.")]
    public Vector2 waterSize;

    [Tooltip("물의 판정 길이입니다.")]
    public float waterDistance;

    [Tooltip("물의 지속 시간입니다.")]
    public float waterActiveTime;

    [Tooltip("물의 방향입니다.")]
    public Vector2 waterDirection = Vector2.right;


    [Header("번개 관련")]

    [Tooltip("번개의 중심점입니다.")]
    public Transform lightningPosition;


    [HideInInspector]
    public Transform footPosition;
    [Tooltip("번개의 원 크기입니다.")]
    public float lightningRadius;

    [Tooltip("번개의 지속 시간입니다.")]
    public float lightningActiveTime;

    ////[SerializeField] CharacterAudio audioPlayer = null;

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

    [Tooltip("물체와 상호작용하고 있는가?")]
    public bool isInteracting;
    public bool isSkillUse_Water;
    public bool isSkillUse_Lightning;

    public BoxCollider2D playerCollider;
    private CapsuleCollider2D playerCapsuleCollider;


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

    #region 애니메이터 파라미터

    [HideInInspector] public int animatorDieBool;
    [HideInInspector] public int animatorGroundedBool;
    [HideInInspector] public int animatorWalkingBool;

    [HideInInspector] public int animatorJumpingBool;
    [HideInInspector] public int animatorJumpTrigger;

    [HideInInspector] public int animatorClimbingBool;
    [HideInInspector] public int animatorCatchingBool;

    [HideInInspector] public int animatorIdleBlend;
    [HideInInspector] public int animatorCatchBlend;

    [HideInInspector] public int animatorFallingBool;
    [HideInInspector] public int animatorGlidingBool;

    [HideInInspector] public int animatorLightningBool;
    [HideInInspector] public int animatorWaterBool;


    [HideInInspector] public int animatorRestoreBool;

    [HideInInspector] public int animatorInteractingBool;
    [HideInInspector] public int animatorInteractingBlend;
    #endregion
    // [HideInInspector] public int animatorPushingBool;
    //public int animatorFallingBool;

    [HideInInspector] public PlayerStateMachine playerStateMachine;
    [HideInInspector] public InputManager inputManager;

    [HideInInspector] public float saveMoveInputX;

    [Tooltip("UpdateVelocity에서 들어갈 추가적인 힘입니다.")]
    private Vector2 extraForce;


    //상태적인 bool 값들
    [HideInInspector] public bool canMove = true;

    [HideInInspector] public bool isJumping;

    [Tooltip("경사면에 있냐 없냐를 뜻합니다.")]
    public bool isSlope;

    [Tooltip("잡기 오브젝트와 NPC가 동시에 존재할 때 잡기를 수행하기 위해 만듬")]
    private bool isTalkReady;

    [HideInInspector]
    public bool isGrounded;

    [HideInInspector]
    public bool isFalling;

    [HideInInspector]
    public bool isInteractingSoulMemory;

    [HideInInspector]
    public bool isDie = false;

    [Tooltip("활강상태로 진입할 수 있는지 여부입니다.")]
    public bool canGliding = false;

    [Tooltip("캐릭터가 물과 닿았는가")]
    private bool isWater = false;

    [Tooltip("캐릭터가 오른쪽을 바라보고 있는가"), SerializeField]
    public bool isRight = true;

    [Tooltip("점프를 해야하는가")]
    private bool shouldJump = false;

    [Tooltip("사다리에 닿은 상태로 상하키 입력을 했는가를 뜻합니다.")]
    [HideInInspector]
    public bool inLadder = false;

    [Tooltip("사다리에 닿은 !!! 상태인가를 뜻합니다.")]
    [HideInInspector]
    public bool onLadder = false; //퍼블릭으로 변경함

    [Tooltip("사다리에 올라탄 상태에서 특정 방향으로 점프했는가를 뜻합니다.")]
    [HideInInspector]
    public bool onLadderJump = false;

    [Tooltip("사다리를 오르고 있는 상태인가를 뜻합니다.")]
    [HideInInspector]
    public bool isClimbLadder = false;

    //[Tooltip("물건에 닿았나? - 물건을 잡으면 false가 되어야한다.")]
    //public bool isTouching = false;

    [Tooltip("물건을 잡았나?")]
    public bool isCatching = false;

    [Tooltip("활강 중인가?")]
    public bool isGliding;

    public bool existInteractObject;



    [HideInInspector] public bool isTalking = false;


    [Tooltip("현재 타고있는 사다리의 위치.")]
    [HideInInspector] public Vector2 ladderPosition = Vector2.zero;

    [HideInInspector] public Vector2 prevPosition;
    [HideInInspector] public float originalMoveSpeed;

    [Tooltip("테스트용 idle 모션입니다.")]
    public Motion[] testIdleMotions;


    [Tooltip("잡기 키를 눌렀는가?")]
    [HideInInspector] public bool isInputCatchKey = false;

    // [SerializeField]
    [HideInInspector]
    private CatchableObject catchedObject = null;

    //[SerializeField]
    [HideInInspector]
    public CatchableObject touchedObject = null;

    //[HideInInspector]
    //public InteractableObject interactedObject = null;
    [HideInInspector]
    public InteractableObject touchedInteractObject = null;

    [HideInInspector] public Rigidbody2D catchBody;


    public SceneChanger sceneChanger;
    [Header("잡기 손 위치")]
    public Vector2 handPosition_idle;
    public Vector2 handPosition_walk;
    public Vector2[] handPosition_jump;
    public Vector2[] handPosition_landed;

    public Vector2 currentHandPosition;

    [HideInInspector]
    [Tooltip("땅 체크에 쓰일 hit")]
    public RaycastHit2D groundHit;

    [HideInInspector]
    [Tooltip("일단 밟을 수 있으면 다 해볼 생각")]
    public int groundCheckMask;

    [Header("Ray길이")]
    public float groundCheckDistance;

    [HideInInspector]
    [Tooltip("현재 대화중인/대화를 시작할 수 있는NPC")]
    public NPC currentNPC;

    [HideInInspector]
    [Tooltip("현재 상호작용 할 수 있는/ 하는 중인 사념")]
    public SoulMemory currentSoulMemory;

    [HideInInspector]
    public PlayerSkillManager playerSkillManager;

    #endregion

    //[Tooltip("대화할 캐릭터의 토크 스타터")]
    //public TalkStarter talkStater;


    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            //if (instance == null)
            //{
            //    instance = FindObjectOfType<PlayerController>();
            //}
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
        ChangeState(eState.PLAYER_FALL);

        playerSkillManager.Init();
        playerStateMachine.Start();

        hitSize = new Vector2(playerCollider.size.x, 0.1f);
        sceneChanger = SceneChanger.Instance;
    }
    public void Init()
    {

        playerStateMachine = new PlayerStateMachine(this);
        if (playerSkillManager == null)
        {
            playerSkillManager = GetComponent<PlayerSkillManager>();

        }
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        puppet = gameObject.transform;


        Init_LayerMask();

        Init_AnimatorParameter();

        isRight = true;
        isWater = false;
        isInputCatchKey = false;
        isTalkReady = false;
        isGliding = false;
        existInteractObject = false;

        isInteractingSoulMemory = false;

        currentNPC = null;
        catchedObject = null;
        //pushedObject = null;
        touchedObject = null;
        currentSoulMemory = null;

        originalMoveSpeed = movementSpeed;
        //animator.SetFloat("WindBlend", 0);
        currentGlideAngle = glideAngle + 90f;

        footPosition = lightningPosition;
        myRigidbody = playerRigidbody;

        Init_ContactFilter();
    }
    private void Init_AnimatorParameter()
    {

        animatorWalkingBool = Animator.StringToHash("Walking");
        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorClimbingBool = Animator.StringToHash("Climbing");
        animatorCatchingBool = Animator.StringToHash("Catching");

        animatorFallingBool = Animator.StringToHash("Falling");

        animatorGlidingBool = Animator.StringToHash("Gliding");
        animatorWaterBool = Animator.StringToHash("Water");
        animatorLightningBool = Animator.StringToHash("Lightning");

        animatorCatchBlend = Animator.StringToHash("CatchBlend");
        animatorIdleBlend = Animator.StringToHash("IdleBlend");
        //animatorWindBlend = Animator.StringToHash("WindBlend");
        //animatorPushingBool = Animator.StringToHash("Pushing");
        animatorDieBool = Animator.StringToHash("Die");
        animatorJumpingBool = Animator.StringToHash("Jumping");
        animatorInteractingBool = Animator.StringToHash("Interacting");

        //animatorRestoreBool = Animator.StringToHash("Restore");
    }

    public void Init_LayerMask()
    {
        groundCheckMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Ground_Soft")) | (1 << LayerMask.NameToLayer("Plant")) | (1 << LayerMask.NameToLayer("Ground_Hard"));
        //| (1 << LayerMask.NameToLayer("Default"));
        //  noPlayerMask = ~noPlayerMask;

        playerMask = LayerMask.NameToLayer("Player");
        groundMask = LayerMask.NameToLayer("Ground");// LayerMask.NameToLayer("Ground_Soft") LayerMask.NameToLayer("Ground_Hard");

        groundSoftMask = LayerMask.NameToLayer("Ground_Soft");
        groundHardMask = LayerMask.NameToLayer("Ground_Hard");
        rainMask = LayerMask.NameToLayer("WeatherFx_withOpaqueTex");
    }


    private void Init_ContactFilter()
    {
        contactFilter_Ground.useLayerMask = true;
        contactFilter_Ground.useNormalAngle = true;
    }


    void Update()
    {
        CheckInteractInput();
        CheckCatch();
        //CheckTalkInput();
        CheckMoveInput();
        CheckLadderInput();
        CheckJumpInput();

        playerSkillManager.Execute();
        //CheckGlideInput();

        UpdateChangeState();
        CheckFalling();

        playerStateMachine.Update();

    }
    void FixedUpdate()
    {
       // UpdateParentsFollowMovement();
        UpdateGroundCheck_Fixed();
        // GroundCheck();
        //UpdateGroundCheck_Cast();

        UpdateSlopeStop();
        // UpdateGroundCheck_Touch_Cast();
        UpdateMoveVelocity();
        UpdateJumpVelocity();

        playerSkillManager.PhysicsExcute();
        //UpdateCanGlide();

        UpdateGravityScale();
        UpdateDirection();

        playerStateMachine.FixedUpdate();

    }

    //private void CheckCatch()
    //{
    //    if (ReferenceEquals(touchedObject, null))
    //    {

    //    }
    //}


    /// <summary>
    /// 경사면에서 플레이어가 밀리지 않게 합니다.
    /// </summary>
    private void UpdateSlopeStop()
    {

        if (isSlope == true) //경사면에 있을 때
        {
            if (isFalling == false && isJumping == false && isGliding == false && movementInput == Vector2.zero && playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT)
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

        }
        else
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            return;
        }
    }

    /// <summary>
    /// CheckInteractInput에 들어갑니다.
    /// </summary>
    private bool CheckCanTalk()
    {
        if (!ReferenceEquals(currentNPC, null) && isTalking == false && currentNPC.canInteract && playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    eState testState;
    private void CheckInteractInput()
    {
        if (InputManager.Instance.buttonInteract.wasPressedThisFrame)// 인터렉트 키 입력
        {
            if (!CanMove())
            {
                return;
            }

            #region SoulMemory
            //if (!ReferenceEquals(currentSoulMemory, null))
            //{
            //    if (currentSoulMemory.isTake == false) //상호작용한 적 없는 소울 메모리라면
            //    {
            //        currentSoulMemory.isTake = true;
            //        currentSoulMemory.TakeSoulMemory();
            //    }
            //}
            //else if (ReferenceEquals(catchedObject, null) && !ReferenceEquals(touchedObject, null))
            #endregion 

            #region Catch

            if (ReferenceEquals(catchedObject, null) && !ReferenceEquals(touchedObject, null))
            {
                if (touchedObject.canCatched == true)
                {
                    SetCatchedObject(touchedObject);

                    if (!ReferenceEquals(catchedObject, null))
                    {
                        catchedObject.GoCatchThis();
                    }
                }
                else
                {
                    if (CheckCanTalk()) //대화를 할 수 있다면
                    {

                        TalkSystemManager.Instance.currentTalkNPC = currentNPC;
                        currentNPC.StartTalk();

                        // TalkSystemManager.Instance.StartGoTalk(currentNPC.currentTalkCode, currentNPC);
                    }
                }

                return;
            }
            #endregion //잡기

            #region Talk
            if (CheckCanTalk())
            {
                TalkSystemManager.Instance.currentTalkNPC = currentNPC;
                currentNPC.StartTalk();
                //if () //대화를 해야한다면
                //{


                //    // TalkSystemManager.Instance.StartGoTalk(currentNPC.currentTalkCode, currentNPC);
                //}
                //else //아니라면
                //{

                //    PutCatchedObject();

                //}
                return;
            }
            #endregion //대화

            #region Put
            if (ReferenceEquals(catchedObject, null) == false) // 잡고 있는 게 이미 있다면
            {
                catchedObject.GoPutThis();
                SetCatchedObject(null);
                return;
            }
            #endregion //놓기

            #region Interact

            testState = playerStateMachine.GetCurrentStateE();
            if (existInteractObject && touchedInteractObject.canInteract == true && testState == eState.PLAYER_DEFAULT)
            {
                touchedInteractObject.DoInteract();
            }

            #endregion //상호작용
        }
    }
    #region 이동, 점프, 사다리, 활강
    private void CheckMoveInput()
    {
        //무브먼트 인풋을 0으로 초기화
        movementInput = Vector2.zero;


        if (InputManager.Instance.buttonMoveRight.isPressed) //오른쪽 이동
        {

            if (CanMove())
            {
                movementInput = Vector2.right;
                //if (pushedObject == null)
                //{
                isRight = true;
                //}

                if (ReferenceEquals(catchedObject, null) == false)
                {
                    catchedObject.isRight = true;
                }

            }

            return;
        }

        if (InputManager.Instance.buttonMoveLeft.isPressed) //왼쪽 이동
        {
            if (CanMove())
            {
                movementInput = Vector2.left;
                //if (pushedObject == null)
                //{
                isRight = false;
                //}
                if (ReferenceEquals(catchedObject, null) == false)
                {
                    catchedObject.isRight = false;
                }

            }
            return;
        }

    }
    private bool CanLadderInput()
    {
        //if (isCatching)
        //{
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}
        return true;
    }

    private void CheckLadderInput()
    {
        if (CanLadderInput() == false)
        {
            return;
        }

        //사다리에 닿은!!! 상태일때
        if (onLadder)
        {
            if (InputManager.Instance.buttonUp.wasPressedThisFrame
                || InputManager.Instance.buttonDown.wasPressedThisFrame)
            {
                if (CanMove())
                {
                    inLadder = true;
                    isClimbLadder = true;
                    isJumping = false;
                    if (canGliding == false)
                    {
                        GroundGlide();
                    }
                }
            }
        }

        if (inLadder)
        {
            if (InputManager.Instance.buttonUp.isPressed)//위쪽 이동
            {
                if (CanMove())
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

            }
            else if (InputManager.Instance.buttonDown.isPressed) //아래쪽 이동
            {
                if (CanMove())
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

        if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame)
        {
            if (CanMove() == false)
            {
                return;
            }

            if (isGrounded) //땅에 닿아있을 때와 사다리를 타는 상태일 때만 점프를 할 수 있습니다.
            {
                shouldJump = true;

            }
            else
            {
                if (playerStateMachine.GetCurrentStateE() == eState.PLAYER_DEFAULT)
                {
                    shouldJump = true;
                }
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

    private void LateUpdate()
    {
        if (!ReferenceEquals(catchedObject, null)) //널이 아닐 경우


        //if (catchedObject != null)
        {
            catchedObject.SetPosition(GetHandPosition());
        }

    }

    #region 밀기/잡기
    private void CheckCatch() //최적화가 필요함
    {
        if (!ReferenceEquals(catchedObject, null)) //널이 아닐 경우
                                                   //if (catchedObject != null)
        {
            isCatching = true;
        }
        else
        {
            isCatching = false;
        }
    }

    public void SetCatchedObject(CatchableObject _co)
    {
        catchedObject = _co;
    }

    public CatchableObject GetCatchedObject()
    {
        return catchedObject;
    }

    public void SetTouchedObject(CatchableObject _go)
    {
        touchedObject = _go;
    }
    public CatchableObject GetTouchedObject()
    {
        return touchedObject;
    }

    #endregion

    private int walkLayer;
    /// <summary>
    /// 걷는 효과음을 출력합니다.
    /// </summary>
    public void PlayWalkAudio()
    {
        //walkLayer = groundHit.collider.gameObject.layer;

        //if (walkLayer == groundSoftMask)
        //{
        //    Debug.Log("Soft");
        //    AudioManager.Instance.Play_WalkSoft();
        //}
        //else if (walkLayer == groundHardMask)
        //{
        //    Debug.Log("Hard");
        //    AudioManager.Instance.Play_WalkHard();

        //}
        //else if(walkLayer==LayerMask.NameToLayer("Plant"))
        //{
        //    Debug.Log(LayerMask.LayerToName(walkLayer));
        //    AudioManager.Instance.Play_WalkHard();

        //}

        AudioManager.Instance.Play_WalkHard();
    }


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
                GroundGlide();
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //ChangeState(eState.PLAYER_DEFAULT);
            }
        }
        else if (isWater == true) // 아니면 물 속?
        {
            isGrounded = true;

            GroundGlide();
            isJumping = false;

            animator.SetBool(animatorGroundedBool, isGrounded);
        }
        else
        {
            isGrounded = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }
    }

    public float groundAngle;
    public Vector2 groundPerp;

    Vector2 hitSize;
    public Vector2 castStartPos;


    /// <summary>
    /// Touching을 쓰고, 경사면에서 고정됩니다.
    /// </summary>
    private void UpdateGroundCheck_Fixed()
    {
        //박스캐스트 버전
        hitSize = new Vector2(playerCollider.size.x, 0.1f);
        castStartPos = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
        groundHit = Physics2D.BoxCast(castStartPos, hitSize, 0f, Vector2.down, groundCheckDistance, groundCheckMask);


        ////써클캐스트 버전
        //hitSize = new Vector2(playerCapsuleCollider.size.x, 0.1f);
        //castStartPos = new Vector2(playerCapsuleCollider.bounds.center.x, playerCapsuleCollider.bounds.min.y);
        //groundHit = Physics2D.CircleCast(castStartPos, playerCapsuleCollider.size.x, Vector2.down, groundCheckDistance, groundCheckMask);



        //groundHit = Physics2D.Raycast(castStartPos, Vector2.down, groundCheckDistance, groundCheckMask);

        // groundPerp = Vector2.Perpendicular(groundHit.normal).normalized; //실제 사용할 때에는-1 곱해줘야함

        groundAngle = Vector2.Angle(groundHit.normal, Vector2.up);

        if (playerCollider.IsTouching(contactFilter_Ground))
        {
            if (playerRigidbody.velocity.y <= 2f)
            {
                isGrounded = true;
                GroundGlide();
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //
            }
        }
        else
        {
            isGrounded = false;
            isSlope = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }


        if (groundHit == true)
        {
            //Debug.DrawLine(groundHit.point, groundHit.point + groundHit.normal, Color.blue);


            if (groundAngle != 0f)
            {
                isSlope = true;
            }
            else
            {
                isSlope = false;
            }
        }

        UpdateSlopeStop();


    }
    /// <summary>
    /// 박스캐스트로 땅 체크를 합니다.
    /// </summary>
    private void UpdateGroundCheck_Cast()
    {
        castStartPos = new Vector2(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
        groundHit = Physics2D.BoxCast(castStartPos, hitSize, 0f, Vector2.down, groundCheckDistance, groundCheckMask);
        //groundHit = Physics2D.Raycast(castStartPos, Vector2.down, groundCheckDistance, groundCheckMask);
        groundPerp = Vector2.Perpendicular(groundHit.normal).normalized; //-1 곱해줘야함
        groundAngle = Vector2.Angle(groundHit.normal, Vector2.up);

        if (groundHit == true)
        {
            Debug.DrawLine(groundHit.point, groundHit.point + groundHit.normal, Color.blue);


            if (groundAngle != 0f) //수직이 아니라면 
            {
                isSlope = true; //경사면임
            }
            else
            {
                isSlope = false; //아님
            }
            //a만약 
            if (Mathf.Abs(groundHit.point.y - castStartPos.y) <= 0.1f && groundAngle != 90f && playerRigidbody.velocity.y <= 2f)
            {
                isGrounded = true;
                GroundGlide();
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //ChangeState(eState.PLAYER_DEFAULT);
            }
            else
            {

                isGrounded = false;
                isSlope = false;
                animator.SetBool(animatorGroundedBool, isGrounded);
            }
            return;
        }
        else
        {
            isGrounded = false;
            isSlope = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }

        ////if (groundHit.normal)
        ////{

        ////}
        //isGrounded = groundHit;
    }

    /// <summary>
    /// 터칭레이어가 false면, 박스캐스트로 땅 체크를 합니다.
    /// </summary>
    private void UpdateGroundCheck_Touch_Cast()
    {

        groundHit = Physics2D.BoxCast(playerRigidbody.position, playerCollider.size, 0f, Vector2.down, groundCheckDistance, groundCheckMask);

        if (playerCollider.IsTouching(contactFilter_Ground))
        {
            if (playerRigidbody.velocity.y > 0f)
            {
                //isJumping = false;
            }
            else
            {
                isGrounded = true;
                GroundGlide();
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //ChangeState(eState.PLAYER_DEFAULT);
            }

        }
        else if (groundHit == true) //TODO : 노말값 추가해서 원웨이 콜라이더를 거르기.
        {
            if (playerRigidbody.velocity.y > 0f)
            {
                //isJumping = false;
            }
            else
            {
                isGrounded = true;
                GroundGlide();
                isJumping = false;

                animator.SetBool(animatorGroundedBool, isGrounded);

                //ChangeState(eState.PLAYER_DEFAULT);
            }

        }
        //else if (isWater == true) // 아니면 물 속?
        //{
        //    isGrounded = true;

        //    GroundGlide();
        //    isJumping = false;

        //    animator.SetBool(animatorGroundedBool, isGrounded);
        //}
        else
        {
            isGrounded = false;
            animator.SetBool(animatorGroundedBool, isGrounded);
        }



    }
    private void GroundGlide()
    {

        //if (glideGauge != null)
        //{
        //    glideGauge.fillAmount = 0f;
        //}
        isGliding = false;
        canGliding = true;
        if (GlideCoroutine != null)
        {
            StopCoroutine(GlideCoroutine);
            AudioManager.Instance.Stop_Skill_Wind();
            GlideCoroutine = null;
        }
    }
    private void CheckFalling()
    {
        if (playerRigidbody.velocity.y < -3f && isGrounded == false)
        {
            if (isClimbLadder == false && inLadder == false)
            {
                isFalling = true;

            }

        }
        else
        {
            isFalling = false;
        }
    }
    private void UpdateChangeState()
    {
        if (isDie)
        {
            ChangeState(eState.PLAYER_DIE);
            return;
        }

        if (isCatching)
        {
            animator.SetFloat(animatorIdleBlend, 7f);
            animator.SetFloat(animatorCatchBlend, 7f);
        }
        else
        {
            animator.SetFloat(animatorIdleBlend, 0f);
            animator.SetFloat(animatorCatchBlend, 0f);

        }


        if (inLadder && prevPosition.y != playerRigidbody.position.y) //사다리 안쪽에 있고, 위로 올라갔다면
        {//사다리

            PutCatchedObject(); //물체 들고있다면 내리고

            ChangeState(eState.PLAYER_CLIMB_LADDER); // 사다리상태로 변경
        }
        else if (isInteracting) //상호작용 하고 있다면
        {
            ChangeState(eState.PLAYER_INTERACT);
        }
        //else if (isSkillUse_Water)
        //{//물능력
        //    PutCatchedObject(); //물체 들고있다면 내리고
        //    ChangeState(eState.PLAYER_SKILL_WATER);
        //}
        //else if (isSkillUse_Lightning)
        //{//번개능력

        //    PutCatchedObject();
        //    ChangeState(eState.PLAYER_SKILL_LIGHTNING);
        //}
        else if (isGrounded && !isJumping && !isGliding)
        {
            ChangeState(eState.PLAYER_DEFAULT);
        }
        else if (isFalling && !isGliding && inLadder == false)
        {
            ChangeState(eState.PLAYER_FALL);


        }
        else if (!isGrounded && isJumping && !isGliding)
        {
            if (playerStateMachine.GetCurrentStateE() != eState.PLAYER_GLIDE)
            {
                if (playerStateMachine.GetCurrentStateE() != eState.PLAYER_FALL)
                {

                    ChangeState(eState.PLAYER_JUMP);
                }
            }

        }
        else if (isGliding && !inLadder)
        {

            if (playerStateMachine.GetCurrentStateE() != eState.PLAYER_GLIDE)
            {
                PutCatchedObject();
                ChangeState(eState.PLAYER_GLIDE);
                GlideCoroutine = playerSkillManager.skillWindGilde.ProcessGlideTimer();
                StartCoroutine(GlideCoroutine);
            }

        }
        else if (!inLadder && !isGrounded && !onLadder)
        {
            ChangeState(eState.PLAYER_DEFAULT);
        }
    }

    //TEST
    private void ChangeState(eState _state)
    {
        if (playerStateMachine.GetCurrentStateE() != _state)
        {
            playerStateMachine.ChangeState(_state);
        }
    }

    /// <summary>
    /// 이동 관련 벨로시티 업데이트.
    /// </summary>
    private void UpdateMoveVelocity()
    {
        prevPosition = new Vector2(playerRigidbody.position.x, playerRigidbody.position.y);


        if (isClimbLadder == false && isGliding == false)//사다리도 안타고, 글라이딩상태도 아닐때 ( 기본 상태)
        {

            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, playerRigidbody.velocity.y);//+ extraForce.y);
        }
        else if (isClimbLadder)
        {
            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, (movementInput.y * climbSpeed));// + extraForce.y);
        }
        //else if (isGliding) //글라이딩 상태일때
        //{
        //    Vector2 dir = new Vector2(Mathf.Sin(Mathf.Deg2Rad * currentGlideAngle * movementInput.x), Mathf.Cos(Mathf.Deg2Rad * currentGlideAngle * movementInput.x));

        //    if (movementInput != Vector2.zero)
        //    {
        //        playerRigidbody.velocity = dir * glideSpeed;
        //    }
        //    else
        //    {
        //        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -glideSpeed * 0.3f);
        //    }


        //    //playerRigidbody.velocity =
        //    //    new Vector2()
        //}

        SetMoveVector(playerRigidbody.velocity);
        UpdateParentsFollowMovement();

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
                Debug.Log("Jump!");
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
        else
        {

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
    private void UpdateDirection()
    {
        //스케일 변경으로 flip


        if (InputManager.Instance.buttonMoveRight.isPressed && playerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = Vector3.one;


            //if (flipFX.activeSelf == false)
            //{
            //    flipFX.SetActive(true);
            //}
        }
        else if (InputManager.Instance.buttonMoveLeft.isPressed && playerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = flippedScale;
            //if (flipFX.activeSelf == false)
            //{
            //    flipFX.SetActive(true);

            //}
        }

    }


    /// <summary>
    /// 죽습니다.
    /// </summary>
    public void GoDie()
    {
        isDie = true;
        ChangeState(eState.PLAYER_DIE);
    }
    public IEnumerator ProcessDie()
    {
        if (isDie)
        {

        }

        yield return YieldInstructionCache.WaitForFixedUpdate;
    }


    /// <summary>
    /// 아무 스킬이 사용중이라면, true를 리턴합니다.
    /// </summary>
    /// <returns></returns>
    public bool isOtherSkillUse()
    {
        if (isSkillUse_Lightning)
        {
            return true;
        }
        else if (isSkillUse_Water)
        {
            return true;
        }
        else if (isGliding)
        {
            return true;
        }

        return false;
    }

    public bool CanMove()
    {

        if (canMove == false)
        {
            return false;
        }
        if (isDie)
        {
            return false;
        }
        else if (isInteracting)
        {
            return false;
        }
        else if (isSkillUse_Water)
        {
            return false;
        }
        else if (isSkillUse_Lightning)
        {
            return false;
        }
        else if (isTalking)
        {
            return false;
        }
        else if (isInteractingSoulMemory)
        {
            return false;
        }
        return true;
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



    public Vector2 GetHandPosition()
    {
        return currentHandPosition;
    }
    public void SetHandPosition_Idle()
    {
        currentHandPosition = handPosition_idle;
    }

    public void SetHandPosition_Walk()
    {
        currentHandPosition = handPosition_walk;
    }
    public void SetHandPosition_Jump(int _index)
    {
        currentHandPosition = handPosition_jump[_index];
    }

    public void SetHandPosition_Landed(int _index)
    {
        currentHandPosition = handPosition_landed[_index];
    }

    /// <summary>
    /// 바람이펙트를 킵니다.
    /// </summary>
    public void ResetWindStart()
    {
        playerSkillManager.skillWindGilde.windEffect.SetActive(true);
        playerSkillManager.skillWindGilde.windAnimator.SetFloat(playerSkillManager.skillWindGilde.windAnimatorTornadoBlend, 1.5f);
    }
    /// <summary>
    /// 바람 이펙트를 끕니다.
    /// </summary>
    public void ResetWindEnd()
    {
        //if (glideGauge != null)
        //{
        //    glideGauge.fillAmount = 0f;
        //}
        playerSkillManager.skillWindGilde.windAnimator.SetFloat(playerSkillManager.skillWindGilde.windAnimatorTornadoBlend, 3f);
        isGliding = false;
    }
    /// <summary>
    /// 물건을 놓게할 때 쓰이는 함수입니다. 무려 널체크까지 해준다구~
    /// </summary>
    public void PutCatchedObject()
    {
        if (!ReferenceEquals(catchedObject, null))
        {
            catchedObject.GoPutThis();
            catchedObject = null;
        }
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    //if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex && canMove == true)
    //    //{
    //    //    ProcessDie();
    //    //}
    //    //else
    //    //{

    //    //}
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //WeatherInteractionObject tempObject = collision.gameObject.GetComponent<WeatherInteractionObject>();
    //    //if (collision is ICatchable)
    //    //{

    //    //}
    //}
    private Plant currentPlant;

    public Plant GetCurrentPlant()
    {
        return currentPlant;
    }

    public void SetCurrentPlant(Plant _plant)
    {
        currentPlant = _plant;
    }


    #region 구버전


    //public void CheckCatchInput(CatchableObject _obj)
    //{
    //    if (CanMove())
    //    {
    //        var tempObj = _obj;

    //        if (InputManager.Instance.buttonCatch.wasPressedThisFrame)// 키 누르기
    //        {
    //            if (ReferenceEquals(catchedObject, null))
    //            //   if (catchedObject == null) //잡아야 할 경우
    //            {
    //                if (!ReferenceEquals(touchedObject, null))
    //                //   if (touchedObject != null)
    //                {
    //                    SetCatchedObject(tempObj);
    //                    if (!ReferenceEquals(catchedObject, null))
    //                    {
    //                        catchedObject.GoCatchThis();
    //                    }
    //                }

    //            }
    //            else //놓아야 함
    //            {
    //                catchedObject.GoPutThis();

    //                catchedObject = null;
    //            }
    //        }

    //    }

    //}



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
    //private void UpdateGroundCheck()
    //{
    //    if (playerCollider.IsTouching(contactFilter_Ground))
    //    {
    //        isGrounded = true;
    //        blockType = eBlockType.GROUND;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //        blockType = eBlockType.NONE;
    //    }

    //    animator.SetBool(animatorGroundedBool, isGrounded);
    //}


    ///// <summary>
    ///// 잡을 수 있는 오브젝트의 FixedJoint를 설정합니다.
    ///// </summary>
    ///// <param name="_fixedJoint">이걸로 설정합니다.</param>
    //public void SetCurrentJointThis(Joint2D _fixedJoint)
    //{
    //    currentCatchJoint = _fixedJoint;
    //}


    //public Joint2D GetCurrentCatchJoint()
    //{
    //    return currentCatchJoint;
    //}



    //private void CheckTalkInput()
    //{
    //    if (InputManager.Instance.buttonInteract.wasPressedThisFrame)
    //    {
    //        if (!ReferenceEquals(currentNPC, null) && isTalking == false)
    //        {

    //            if (!ReferenceEquals(touchedObject, null)) //닿고 있는 게 있을 경우...즉, 잡아야 할 경우
    //            {
    //                if (isCatching == false) //잡고있지 않을 경우 
    //                {
    //                    CheckCatchInput(touchedObject);
    //                }
    //                else
    //                {
    //                    TalkSystemManager.Instance.currentTalkNPC = currentNPC;
    //                    currentNPC.StartTalk();
    //                }

    //            }

    //            // TalkSystemManager.Instance.StartGoTalk(currentNPC.currentTalkCode, currentNPC);
    //        }
    //    }
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

    #endregion


    private void OnDestroy()
    {
        instance = null;
    }
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;

        //Gizmos.DrawWireCube(castStartPos, hitSize);
        //Gizmos.DrawWireCube((Vector3)castStartPos + (Vector3)Vector2.down * groundCheckDistance, hitSize);

        //if (groundHit == true)
        //{
        //    Gizmos.DrawSphere(groundHit.point, 0.1f);

        //}

        //Gizmos.color = Color.cyan;

        //Gizmos.DrawWireCube(transform.position, waterSize);
        ////Draw a Ray forward from GameObject toward the maximum distance
        //Gizmos.DrawRay(transform.position, waterDirection * waterDistance);
        ////Draw a cube at the maximum distance
        //Gizmos.DrawWireCube(transform.position + (Vector3)waterDirection * waterDistance, waterSize);


        //번개쪽
        //Gizmos.color = Color.yellow;

        //Gizmos.DrawWireSphere(lightningPosition.position, lightningRadius);


        ////Draw a Ray forward from GameObject toward the maximum distance
        //Gizmos.DrawRay(transform.position, waterDirection * waterDistance);
        ////Draw a cube at the maximum distance
        //Gizmos.DrawWireCube(transform.position + (Vector3)waterDirection * waterDistance, waterSize);

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;

    //    var startPos = gameObject.transform.position;
    //    var finalPos = new Vector2 ()
    //    Debug.Log(right);

    //    //Vector2 finalPos =
    //    //    new Vector2(
    //    //        Mathf.Cos(Mathf.PI / 180 * Quaternion.AngleAxis(glideAngle * right, startPos).eulerAngles.z),
    //    //        Mathf.Sin(Quaternion.AngleAxis(glideAngle * right, startPos).eulerAngles.z * Mathf.Deg2Rad)).normalized;

    //    Gizmos.DrawLine(startPos, _rightV);
    //}

    //}
}

