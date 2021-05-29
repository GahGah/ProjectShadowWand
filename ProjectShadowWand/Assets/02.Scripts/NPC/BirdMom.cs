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

    [Tooltip("애니메이터에 direction 파라미터(방향에 따라 애니메이션이 다르기 때문에 사용합니다).")]
    private int animatorDirectionBlend;
    private int animatorMeetBlend;

    [Tooltip("나리와 만나면 변경됩니다.")]
    private int animatorMeetingBool;

    private int animatorTalkingBool;

    private int animatorIdleBool;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        animatorDirectionBlend = Animator.StringToHash("DirectionBlend");
        animatorMeetingBool = Animator.StringToHash("Meeting");
        animatorTalkingBool = Animator.StringToHash("Talking");
        animatorIdleBool = Animator.StringToHash("Idle");

        animatorMeetBlend = Animator.StringToHash("MeetBlend");
        currentTalkCode = 0;
        canInteract = true;
    }

    public override void StartTalk()
    {
        if (baby.catchableObject.isCatched == true)
        {
            currentTalkCode = 8;
            StartCoroutine(ProcessPlayHugAnimation());
            return;
        }

        UpdateDirection();
        UpdateAnimation();

        switch (currentTalkCode)
        {
            case 0:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                currentTalkCode = 4;
                break;

            case 8:
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

    private IEnumerator ProcessPlayHugAnimation()
    {
        PlayerController.Instance.canMove = false;

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
        while (timer < 3f) //초동안 대기
        {
            timer += Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.canMove = true;
        UpdateDirection();

        TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
    }

    public override void UpdateAnimation()
    {
        if (isTalking)
        {
            animator.SetBool(animatorTalkingBool, true);
        }
        else
        {
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
