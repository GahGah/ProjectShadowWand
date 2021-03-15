using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMonsterType
{
    A, B, C
}
public class Monster : Character
{
    [HideInInspector] public MonsterStateMachine monsterStateMachine;

    #region 애니메이터 관련
    [HideInInspector] public int animatorIdleBool;
    [HideInInspector] public int animatorWalkingBool;
    [HideInInspector] public int animatorOutShadowBool;
    [HideInInspector] public int animatorDieBool;
    #endregion

    [HideInInspector] public Vector2 updatingVelocity;
    [HideInInspector] public Rigidbody2D monsterRigidbody;

    [Tooltip("몬스터의 눈 위치입니다.\n눈이라고는 하지만 그냥 이 위치에서 레이캐스트를 할 뿐입니다.")]
    public Transform eyePosition;
    public BoxCollider2D monsterCollider;

    [Header("몬스터 설정 관련")]
    public eMonsterType monsterType;

    [Tooltip("몬스터가 추격할 대상입니다.")]
    public GameObject targetObject;

    [Tooltip("플레이어를 감지하는 범위입니당.")]
    public float detectDistance;
    public bool isDetected;

    [SerializeField] private eSTATE currentMonsterState;
    [HideInInspector] public float saveMoveInputX;

    #region 기존의 Monster 요소


    [HideInInspector] public int layerMask; //레이어 마스크
    [HideInInspector] public LayerMask playerMask;

    [HideInInspector] public Bounds bounds; // 몬스터의 사각형 영역

    [HideInInspector] public RaycastHit2D[] hits; //레이캐스트 히트. LightObject에서 조종한다.
    [HideInInspector] public RaycastHit2D hit; //레이캐스트 히트 하나. LightObject에서 DistanceMode일때 사용한다.
    [HideInInspector] public RaycastHit2D targetHit; //레이캐스트 히트 하나. detect에 쓰인다.
    [HideInInspector] public bool[] hitsLog; //사각형 영역의 레이캐스트가 hit했는지 인스펙터에 알려줌

    [HideInInspector] public Color[] colors;

    [HideInInspector] public Vector2 relOffset;

    [HideInInspector] public Vector2[] directions; // 원래는 빛을 향하는 방향이었음. 하지만 LightObject의 경우에는 반대가 되야겠지?
    [HideInInspector] public Vector3[] path; // 바운드의 꼭짓점 4개 위치.


    [HideInInspector] public Vector2 offset;

    [Tooltip("몬스터가 그림자에 들어가있는 상태면 true, 아니면 false를 갖습니다.")]
    public bool inShadow;

    public bool isDie;

    [Tooltip("타겟과의 거리입니다.")]
    [HideInInspector] public float targetDistance;

    [Tooltip("타겟의 방향입니다.")]
    [HideInInspector] public float targetDir;
    #endregion

    protected void StartSetting()
    {
        if (targetObject == null)
        {
            Debug.LogError("타겟이 없습니다.");
        }
        if (monsterRigidbody == null)
        {
            monsterRigidbody = GetComponent<Rigidbody2D>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        monsterCollider = GetComponent<BoxCollider2D>();

        puppet = gameObject.transform;
        isDie = false;
        hits = new RaycastHit2D[4];

        hitsLog = new bool[4];

        playerMask = LayerMask.NameToLayer("Player");
        layerMask = (1 << LayerMask.NameToLayer("Monster")); //hit가 자기 자신에게는 부딪히지 않기 위해 
        layerMask = ~layerMask;

        if (monsterCollider != null)
        {
            bounds = monsterCollider.bounds;

            Debug.Log("콜라이더 기준으로 바운딩");
        }
        else
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                bounds = renderer.bounds;

                Debug.Log("렌더러 기준으로 바운딩");

            }

        }

        ColorSetting();

        relOffset = monsterRigidbody.position;

        offset = monsterCollider.offset;

        //path = new Vector3[] //꼭짓점 4개의 위치를 구함.
        //{
        //        relOffset + offset + new Vector2(-bounds.extents.x, -bounds.extents.y),
        //        relOffset + offset + new Vector2(bounds.extents.x, -bounds.extents.y),
        //        relOffset + offset + new Vector2(bounds.extents.x, bounds.extents.y),
        //        relOffset + offset + new Vector2(-bounds.extents.x, bounds.extents.y)
        //};

        path = new Vector3[]
        {
            transform.TransformPoint(offset + new Vector2(-monsterCollider.size.x, -monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(monsterCollider.size.x, -monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(monsterCollider.size.x, monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(-monsterCollider.size.x, monsterCollider.size.y) * 0.5f)
        };

        if (MonsterManager.Instance.monsterList.Contains(this) == false) //자기 자신이 안들어가있다면
        {
            MonsterManager.Instance.AddMonsterToList(this); //넣는다.
        }

        #region 애니메이터 관련

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        animatorIdleBool = Animator.StringToHash("Idle");
        animatorWalkingBool = Animator.StringToHash("Walk");
        animatorOutShadowBool = Animator.StringToHash("OutShadow");
        animatorDieBool = Animator.StringToHash("Die");

        #endregion

        monsterStateMachine = new MonsterStateMachine(this);
        monsterStateMachine.ChangeState(eSTATE.MONSTER_DEFAULT);
        monsterStateMachine.Start();
    }

    void Start()
    {
        StartSetting();
    }

    void Update()
    {
        if (isDie == false)
        {
            UpdatePath();

            UpdateDetectTarget();

            UpdateState();
            monsterStateMachine.Update();
        }


        currentMonsterState = monsterStateMachine.GetCurrentStateE();
    }

    private void FixedUpdate()
    {
        monsterStateMachine.FixedUpdate();
        UpdateVelocity();
        UpdateDirection();
    }

    public void UpdateState()
    {
        if (inShadow == false && monsterStateMachine.GetCurrentStateE() != eSTATE.MONSTER_OUTSHADOW)
        {
            monsterStateMachine.ChangeState(eSTATE.MONSTER_OUTSHADOW);
        }
        else if (isDetected == false && inShadow && monsterStateMachine.GetCurrentStateE() != eSTATE.MONSTER_DEFAULT) // 당연히 inShadow겠지만 일단 보험으로
        {
            monsterStateMachine.ChangeState(eSTATE.MONSTER_DEFAULT);
        }
        else if (isDetected == true && inShadow && monsterStateMachine.GetCurrentStateE() != eSTATE.MONSTER_CHASE)
        {
            monsterStateMachine.ChangeState(eSTATE.MONSTER_CHASE);
        }
        else
        {

        }

    }
    public void UpdateDetectTarget()
    {
        if (inShadow == true)
        {
            Vector3 monsterPosition = eyePosition.position;
            Vector2 RayPosition = targetObject.transform.position - monsterPosition;

            //Debug.DrawRay(monsterPosition, targetPosition*detectDistance,Color.cyan);
            targetDistance = Vector2.Distance(monsterPosition, targetObject.transform.position);
            if (targetDistance < detectDistance) // 만약 거리 안에 타겟이 들어왔다면
            {
                targetHit = Physics2D.Raycast(monsterPosition, RayPosition, detectDistance, layerMask); // 몬스터 레이어 제외 레이캐스트
                Debug.DrawRay(monsterPosition, RayPosition * (detectDistance), Color.green);

                if (targetHit == true && targetHit.collider.CompareTag("Player")) // 만약 플레이어에게 닿았다면
                {
                    isDetected = true;

                    float dir = eyePosition.position.x - targetObject.transform.position.x;
                    if (dir < 0f) //타겟은 오른쪽에 있다
                    {
                        targetDir = 1f;
                    }
                    else if (dir > 0f)
                    {
                        targetDir = -1f;
                    }
                    else //0
                    {
                        targetDir = 0f;
                    }
                }
                else
                {

                    isDetected = false;
                    if (targetHit != false)
                    {
                        Debug.Log(targetHit.collider.gameObject.name);
                    }

                }
            }
            else
            {
                isDetected = false;
            }

        }

    }
    public void UpdateVelocity()
    {
        updatingVelocity = monsterRigidbody.velocity;

        updatingVelocity += movementInput * movementSpeed * Time.fixedDeltaTime;

        saveMoveInputX = movementInput.x;

        movementInput = Vector2.zero;

        updatingVelocity.x = Mathf.Clamp(updatingVelocity.x, -maxMovementSpeed, maxMovementSpeed);

        monsterRigidbody.velocity = updatingVelocity;

    }
    private void UpdatePath()
    {
        relOffset = monsterRigidbody.position;
        offset = monsterCollider.offset;

        path = new Vector3[]
        {
            transform.TransformPoint(offset + new Vector2(-monsterCollider.size.x, -monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(monsterCollider.size.x, -monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(monsterCollider.size.x, monsterCollider.size.y) * 0.5f),
            transform.TransformPoint(offset + new Vector2(-monsterCollider.size.x, monsterCollider.size.y) * 0.5f)
        };

    }

    private void UpdateDirection()
    {
        //스케일 변경으로 flip
        if (monsterRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = Vector3.one;
        }
        else if (monsterRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = flippedScale;
        }
    }

    protected void ColorSetting()
    {
        colors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.blue
        };
    }


    /// <summary>
    ///hitsLog를 업데이트합니다.
    /// </summary>
    public void UpdateHitsLog() //hitsLog를 hits랑 동일하게 합니다.
    {
        hitsLog[0] = hits[0];
        hitsLog[1] = hits[1];
        hitsLog[2] = hits[2];
        hitsLog[3] = hits[3];
    }
    /// <summary>
    /// 모든 hits가 트루일때 트루를 반환함~
    /// </summary>
    public bool isAllHitsTrue()
    {
        if (hits[0] && hits[1] && hits[2] && hits[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 몬스터를 아예 말려 죽입니다. 꾸에엑!!
    /// </summary>
    public void KillMonster()
    {
        MonsterManager.Instance.RemoveMonsterToList(this);
        isDie = true;
        monsterStateMachine.ChangeState(eSTATE.MONSTER_DIE);
    }
    public void DestroyMonster()
    {
        Destroy(gameObject);
    }
    public void StartDie()
    {
        StartCoroutine(GoDieAnimation());
    }
    private IEnumerator GoDieAnimation()
    {
        animator.SetBool(animatorDieBool, true);

        while (spriteRenderer.color.a > 0f)
        {
            yield return null;
        }

        DestroyMonster();
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyePosition.position, detectDistance);
    }
}
