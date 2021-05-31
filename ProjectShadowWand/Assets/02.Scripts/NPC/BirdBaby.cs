using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBaby : NPC
{

    #region Old
    //TalkStarter talkStarter;

    //public int[] talkCodes;

    //public int currentStartTalkCode;


    //[Tooltip("대화한 횟수")]
    //public int currentTalkCount;

    //[Tooltip("데려간 상태인가?")]
    //public bool isTogether;

    //public bool isFirstTalk;

    //public bool isMeetMom;
    //public Transform babyPos;
    //private void Start()
    //{
    //    Init();
    //}

    //public void Init()
    //{
    //    talkStarter = GetComponent<TalkStarter>();
    //    talkStarter.isEnd = false;
    //    isTogether = false;
    //    isFirstTalk = true;
    //    currentStartTalkCode = talkCodes[0];
    //}

    //private void Update()
    //{
    //    if (isFirstTalk == true) //처음 말 거는 것이라면...
    //    {
    //        currentStartTalkCode = talkCodes[0];
    //        talkStarter.talkCode = currentStartTalkCode;
    //    }

    //    if (talkStarter.isEnd == true)
    //    {
    //        isFirstTalk = false;
    //        if (isMeetMom == false)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            gameObject.SetActive(true);
    //            Debug.Log("미트 맘");
    //            gameObject.transform.position = babyPos.position;
    //        }
    //    }

    //    if (talkStarter.isEnd == true && isTogether == false)
    //    {
    //        isTogether = true;
    //    }

    //}
    #endregion

    [HideInInspector]
    public Quest_MomAndBaby_01 quest_01;

    [HideInInspector]
    public Quest_MomAndBaby_02 quest_02;

    public Transform momTogetherPos;

    public Collider2D birdCollider;

    public BirdMom birdMom;

    public CatchableObject catchableObject;

    private Rigidbody2D rb;


    private Animator animator;
    private bool animtorOn;

    private int animatorIdleBool;
    private int animatorFallingBool;
    private int animatorCatchingBool;
    private void Awake()
    {
        Init();

    }
    private void Start()
    {
        Debug.Log("버드 베이비 스타트");
    }

    public override void Init()
    {
        base.Init();
        currentTalkCode = 0;
        catchableObject.canCatched = false;
        canInteract = true;
        if (birdCollider == null)
        {
            birdCollider = GetComponent<Collider2D>();

        }
        animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animtorOn = true;
        }
        rb = GetComponent<Rigidbody2D>();
        myTransform = gameObject.transform;

        quest_01 = new Quest_MomAndBaby_01(this, birdMom);
        quest_02 = new Quest_MomAndBaby_02(this, birdMom);
        Init_AnimatorParameter();
    }

    private void Update()
    {
        UpdateAnimation();
    }
    private void Init_AnimatorParameter()
    {
        animatorCatchingBool = Animator.StringToHash("Catching");
        animatorFallingBool = Animator.StringToHash("Falling");
        animatorIdleBool = Animator.StringToHash("Idle");
    }


    private void SetAnimatorBool(int _bool , bool _b)
    {
        animator.SetBool(_bool, _b);
    }
    private float fallingVelocity = -1f;
    public override void UpdateAnimation()
    {
        if (catchableObject.isCatched)
        {
            SetAnimatorBool(animatorIdleBool,false);
            SetAnimatorBool(animatorCatchingBool, true);
            SetAnimatorBool(animatorFallingBool, false);

            return;
        }
        else
        {
            SetAnimatorBool(animatorCatchingBool, false);
            SetAnimatorBool(animatorIdleBool, true);

        }

        if (rb.velocity.y >= 0f)
        {
            SetAnimatorBool(animatorFallingBool, false);
            SetAnimatorBool(animatorIdleBool, true);
            return;
        }
        else if (rb.velocity.y < fallingVelocity)
        {

            SetAnimatorBool(animatorFallingBool, true);
            SetAnimatorBool(animatorIdleBool, false);
            return;
        }

    }
    private IEnumerator ProcessCatchBirdBabyQuest()
    {

        while (catchableObject.isCatched == true)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        PlayerController.Instance.currentNPC = null;
        TalkSystemManager.Instance.currentTalkNPC = null;
        QuestManager.Instance.QuestSystem_RemoveQuest(quest_01, true);

    }

    public void StartCatchBabyQuest()
    {
        StartCoroutine(ProcessCatchBirdBabyQuest());
    }
    public override void StartTalk()
    {
        switch (currentTalkCode)
        {
            case 0:
                QuestManager.Instance.QuestSystem_AddQuest(quest_01);
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                canInteract = false;
                //if (PlayerController.Instance.currentNPC == this)//토크 스타터가 본인일때만
                //{
                //    PlayerController.Instance.currentNPC = null;
                //}
                break;
            default:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;
        }

    }
}
