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

    [Header("��ȣ�ۿ��� ����")]
    public Lever lever;

    [Tooltip("�ִϸ����Ϳ� direction �Ķ����(���⿡ ���� �ִϸ��̼��� �ٸ��� ������ ����մϴ�).")]
    private int animatorDirectionBlend;
    private int animatorMeetBlend;



    [Tooltip("������ ������ ����˴ϴ�.")]
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
        currentTalkCode = 4; // ����Ʈ �̼��� ��ȭ
        canInteract = true;
    }


    private bool nowTalking;
    public override void StartTalk()
    {
        if (baby.catchableObject.isCatched == true)
        {
            currentTalkCode = 5; // ����Ʈ �Ϸ� ��ȭ
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
        PlayerController.Instance.SetCatchedObject(null); //������ ��������


        baby.catchableObject.enabled = false; //���� ���ϰ� �Ѵ�.
        baby.gameObject.SetActive(false);


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
        while (timer < 0.5f) //�ʵ��� ���
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

        yield return new WaitWhile(() => isTalking);//isTalking�� false�� �� �� ���� ��ٸ���
        yield return new WaitUntil(() => TalkSystemManager.Instance.GetCurrentTalkCode() == 7); //7�� �ɶ����� ��ٸ���
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
