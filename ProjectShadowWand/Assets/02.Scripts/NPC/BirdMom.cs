using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMom : NPC
{
    Quest_MomAndBaby_01 quest_01;
    Quest_MomAndBaby_02 quest_02;

    [Header("나리")]
    public BirdBaby baby;

    [Header("애니메이터")]
    public Animator animator;

    [Header("상호작용할 레버")]
    public Lever lever;

    [Tooltip("애니메이터에 direction 파라미터(방향에 따라 애니메이션이 다르기 때문에 사용합니다).")]
    private int animatorDirectionBlend;
    private int animatorMeetBlend;



    [Tooltip("나리와 만나면 변경됩니다.")]
    private int animatorMeetingBool;

    private int animatorTalkingBool;

    private int animatorIdleBool;

    [HideInInspector]
    public bool isMeetNari = false;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        lever.canInteract = false;
        animatorDirectionBlend = Animator.StringToHash("DirectionBlend");
        animatorMeetingBool = Animator.StringToHash("Meeting");
        animatorTalkingBool = Animator.StringToHash("Talking");
        animatorIdleBool = Animator.StringToHash("Idle");

        animatorMeetBlend = Animator.StringToHash("MeetBlend");
        currentTalkCode = 4; // 퀘스트 미수락 대화
        canInteract = true;
    }


    private bool nowTalking;
    public override void StartTalk()
    {
        if (baby.catchableObject.isCatched == true)
        {
            currentTalkCode = 5; // 퀘스트 완료 대화
            isMeetNari = true;
            StartCoroutine(ProcessPlayHugAnimation());
            return;
        }

        UpdateDirection();
        UpdateAnimation();

        switch (currentTalkCode)
        {
            //case 0:
            //    TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
            //    currentTalkCode = 4;
            //    break;

            case 4:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;

            case 5:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;

            default:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;
                //case 4:
                //    TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                //    break;
                //case 12:
                //    Talk
        }
    }

    private IEnumerator UpdateTalkAnimation()
    {
        while (true)
        {

            if (isTalking)
            {
                animator.SetBool(animatorTalkingBool, true);
            }
            else
            {
                animator.SetBool(animatorTalkingBool, false);
            }
            yield return null;
        }
    }
    private IEnumerator ProcessPlayHugAnimation()
    {
        PlayerController.Instance.canMove = false;

        baby.catchableObject.GoPutThis();
        PlayerController.Instance.SetCatchedObject(null); //나리를 내려놓고


        baby.catchableObject.enabled = false; //잡지 못하게 한다.
        baby.gameObject.SetActive(false);


        animator.SetBool(animatorMeetingBool, true);
        animator.SetFloat(animatorMeetBlend, 1f);


        //UpdateAnimation();

        if (direction == eDirection.RIGHT) //오른쪽이면 1
        {
            animator.SetFloat(animatorDirectionBlend, 1f);
        }
        else
        {
            animator.SetFloat(animatorDirectionBlend, -1f);
        }
        float timer = 0f;
        while (timer < 0.5f) //초동안 대기
        {
            timer += Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.canMove = true;

        UpdateDirection();

        isTalking = true;

        UpdateAnimation();


        TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);

        animator.SetBool(animatorMeetingBool, false);

        yield return new WaitWhile(() => isTalking);//isTalking이 false가 될 때 까지 기다리기
        yield return new WaitUntil(() => TalkSystemManager.Instance.GetCurrentTalkCode() == 7); //7번 될때까지 기다리기
        StartCoroutine(UpdateTalkAnimation());
    }

    public override void UpdateAnimation()
    {
        if (isTalking)
        {
            Debug.Log("Talking");
            animator.SetBool(animatorTalkingBool, true);
        }
        else
        {
            Debug.Log("Not Talking");
            animator.SetBool(animatorTalkingBool, false);
        }

        if (direction == eDirection.RIGHT) //오른쪽이면 1
        {
            animator.SetFloat(animatorDirectionBlend, 1f);
        }
        else
        {
            animator.SetFloat(animatorDirectionBlend, -1f);
        }

    }
}
