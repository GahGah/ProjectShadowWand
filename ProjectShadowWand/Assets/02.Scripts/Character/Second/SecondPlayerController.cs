using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SecondPlayerController : Character
{

    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    //[SerializeField] CharacterAudio audioPlayer = null;

    [Header("�� ��")]

    [Tooltip("GroundCheck�� �� ����Ʈ�����Դϴ�.")]
    public ContactFilter2D contactFilter_Ground;
    //[SerializeField, Tooltip("����Ű�� �� ������ ��, �ش��ϴ� �߷°����� ���մϴ�.")]
    //private float jumpGravityScale = 1.0f;

    //[SerializeField, Tooltip("�������� �ִ� ���¶�� �ش��ϴ� �߷°����� ���մϴ�.")]
    //float fallGravityScale = 1.0f;

    [SerializeField, Tooltip("�⺻���� �߷°��Դϴ�. ")]
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
    //    private float limitAngle = 0.1f; //����ġ


    private bool isDie = false;

    #region �߰��� ��

    [Tooltip("ĳ���Ͱ� �������� �ٶ󺸰� �ִ°�")]
    private bool isRight = false;

    [Tooltip("������ �ؾ��ϴ°�")]
    private bool shouldJump = false;

    [Tooltip("UpdateVelocity���� �� �߰����� ���Դϴ�.")]
    private Vector2 extraForce;

    [Tooltip("��ٸ� ���� �ö󰡴� �ӵ��Դϴ�.")]
    public float climbSpeed;

    [Tooltip("��ٸ��� ���� ���·� ����Ű �Է��� �ߴ°��� ���մϴ�.")]
    public bool inLadder = false;

    [Tooltip("��ٸ��� ���� !!! �����ΰ��� ���մϴ�.")]
    public bool onLadder = false; //�ۺ����� ������

    [Tooltip("��ٸ��� �ö�ź ���¿��� Ư�� �������� �����ߴ°��� ���մϴ�.")]
    public bool onLadderJump = false;

    [Tooltip("��ٸ��� ������ �ִ� �����ΰ��� ���մϴ�.")]
    public bool isClimbLadder = false;

    [Tooltip("���� Ÿ���ִ� ��ٸ��� ��ġ.")]
    public Vector2 ladderPosition = Vector2.zero;

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
        CheckLadderInput();
        CheckJumpInput();

    }
    void FixedUpdate()
    {
        GroundCheck();
        UpdateMoveVelocity();
        UpdateJumpVelocity();
        UpdateGravityScale();
    }

    private void JudgementMoveOrLadder()
    {
        if (onLadder)
        {
            if (inLadder)
            {
                CheckLadderInput();
            }
            else
            {
                if (InputManager.Instance.buttonUp.isPressed || InputManager.Instance.buttonDown.isPressed)
                {

                    //if (InputManager.Instance.buttonUp.wasPressedThisFrame
                    //    || InputManager.Instance.buttonDown.wasPressedThisFrame)
                    //{
                    inLadder = true;
                    isClimbLadder = true;
                    CheckLadderInput();
                }
                else
                {
                    CheckMoveInput();
                }
            }

        }
        else
        {
            CheckMoveInput();
        }

    }
    private void CheckMoveInput()
    {
        //�����Ʈ ��ǲ�� 0���� �ʱ�ȭ
        movementInput = Vector2.zero;

        if (inLadder == false)
        {
            if (InputManager.Instance.buttonMoveRight.isPressed) //������ �̵�
            {
                movementInput = Vector2.right;
                isRight = true;

            }
            else if (InputManager.Instance.buttonMoveLeft.isPressed) //���� �̵�
            {
                movementInput = Vector2.left;
                isRight = false;
            }
        }

    }

    private void CheckLadderInput()
    {
        //��ٸ��� ����!!! �����϶�
        if (onLadder)
        {
            if (InputManager.Instance.buttonUp.wasPressedThisFrame
                || InputManager.Instance.buttonDown.wasPressedThisFrame)
            {
                inLadder = true;
                isClimbLadder = true;
                isJumping = false;
                //��ٸ��� ��� �κ����� �̵���Ű�� ���ؼ�...
                playerRigidbody.position = (new Vector2(ladderPosition.x, playerRigidbody.position.y));
            }
        }


        if (inLadder)
        {
            movementInput = Vector2.zero;
            if (InputManager.Instance.buttonUp.isPressed)//���� �̵�
            {
                movementInput.y = 1f;
                isClimbLadder = true;

            }
            else if (InputManager.Instance.buttonDown.isPressed) //�Ʒ��� �̵�
            {
                movementInput.y = -1f;
                isClimbLadder = true;
            }


        }

    }

    /// <summary>
    /// ���� �Է��� ����
    /// </summary>
    private void CheckJumpInput()
    {
        if (InputManager.Instance.buttonMoveJump.wasPressedThisFrame)
        {
            if (isGrounded) //���� ������� ���� ��ٸ��� Ÿ�� ������ ���� ������ �� �� �ֽ��ϴ�.
            {
                shouldJump = true;

            }
            if (isClimbLadder)
            {
                onLadderJump = true;
                shouldJump = true;
            }

        }
        #region �߷�����
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
    /// <summary>
    /// �̵� ���� ���ν�Ƽ ������Ʈ.
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
            Debug.Log("��ٸ� �̵�");
            playerRigidbody.velocity =
                new Vector2((movementInput.x * movementSpeed) + extraForce.x, (movementInput.y * climbSpeed) + extraForce.y);
        }

    }

    /// <summary>
    /// ���� ���� ���ν�Ƽ ������Ʈ.
    /// </summary>
    private void UpdateJumpVelocity()
    {
        if (shouldJump)
        {
            if (onLadderJump)
            {
                Debug.Log("���� ����");

                shouldJump = false;
                isClimbLadder = false;
                onLadderJump = false;
                inLadder = false;
                // UpdateGravityScale();

                playerRigidbody.isKinematic = false;
                //playerRigidbody.gravityScale = jumpGravityScale;
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);
                isJumping = true;
                isGrounded = false;
            }
            else
            {
                shouldJump = false;
                playerRigidbody.velocity =
    new Vector2(playerRigidbody.velocity.x, jumpForce);
                isJumping = true;
                isGrounded = false;
            }

        }
    }

    /// <summary>
    /// �÷��̾ ���� ��Ҵ��� üũ�մϴ�.
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
            }
        }
        else
        {
            isGrounded = false;
        }
    }


    /// <summary>
    /// ��ٸ��� Ÿ���ִ� ������ ��, ���� �� üũ�� �մϴ�.
    /// </summary>
    private void CheckLadderGround()
    {

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
            inLadder = false;
            UpdateGravityScale();
        }
        onLadder = _isLadder;
    }

    public void SetIsLadder(bool _isLadder, Vector2 _pos)
    {
        if (_isLadder == true)
        {
            ladderPosition = _pos;
        }
        else
        {
            isClimbLadder = false;
            inLadder = false;
            ladderPosition = Vector2.zero;
            UpdateGravityScale();

        }
        onLadder = _isLadder;
    }

    /// <summary>
    /// �̵�Ű �Է� ���� üũ
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
    //        ////����
    //        //if (blockType != eBlockType.NONE)
    //        //{
    //        if (isGrounded)
    //        {
    //            // ���� �浹���� �� ������ٵ� ���߱� ������, ���ν�Ƽ�� �缳��
    //            if (resetSpeedOnLand)
    //            {
    //                prevVelocity.y = playerRigidbody.velocity.y;
    //                playerRigidbody.velocity = prevVelocity;
    //            }

    //            //��������
    //            isJumping = false;
    //            isFalling = false;

    //            //}

    //        }

    //    }
    //}

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

    //private void UpdateGravityScale()
    //{
    //    // ���س��� �׶��Ƽ �����Ϸ� ����
    //    var gravityScale = groundedGravityScale;

    //    if (blockType == eBlockType.NONE)
    //    {
    //        //���� ���� ����ִ� ���°� �ƴҶ� : �������̶�� ���� �׶��Ƽ �����Ϸ�, �ƴ϶�� �߶� �׶��Ƽ �����Ϸ� ����
    //        gravityScale =
    //            playerRigidbody.velocity.y > 0.0f ?
    //            jumpGravityScale : fallGravityScale;
    //    }

    //    playerRigidbody.gravityScale = gravityScale;
    //}
}


