using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMom : NPC
{
    Quest_MomAndBaby_01 quest_01;
    Quest_MomAndBaby_02 quest_02;

    [Header("����")]
    public BirdBaby baby;

    [Header("�ִϸ�����")]
    public Animator animator;

    [Tooltip("�ִϸ����Ϳ� direction �Ķ����(���⿡ ���� �ִϸ��̼��� �ٸ��� ������ ����մϴ�).")]
    private int animatorDirectionBlend;
    private int animatorMeetBlend;

    [Tooltip("������ ������ ����˴ϴ�.")]
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

        if (direction == eDirection.RIGHT) //�������̸� 1
        {
            animator.SetFloat(animatorDirectionBlend, 1f);
        }
        else
        {
            animator.SetFloat(animatorDirectionBlend, -1f);
        }
        float timer = 0f;
        while (timer < 3f) //�ʵ��� ���
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

        if (direction == eDirection.RIGHT) //�������̸� 1
        {
            animator.SetFloat(animatorDirectionBlend, 1f);
        }
        else
        {
            animator.SetFloat(animatorDirectionBlend, -1f);
        }

    }
}
