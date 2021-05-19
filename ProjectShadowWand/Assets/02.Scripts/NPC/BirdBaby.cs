using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBaby : NPC
{

    #region Old
    //TalkStarter talkStarter;

    //public int[] talkCodes;

    //public int currentStartTalkCode;


    //[Tooltip("��ȭ�� Ƚ��")]
    //public int currentTalkCount;

    //[Tooltip("������ �����ΰ�?")]
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
    //    if (isFirstTalk == true) //ó�� �� �Ŵ� ���̶��...
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
    //            Debug.Log("��Ʈ ��");
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

    [HideInInspector]
    public Collider2D birdCollider;

    public BirdMom birdMom;

    public CatchableObject catchableObject;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        catchableObject.enabled = false;
        currentTalkCode = 5;
        canInteract = true;
        birdCollider = GetComponent<Collider2D>();
        quest_01 = new Quest_MomAndBaby_01(this, birdMom);
        quest_02 = new Quest_MomAndBaby_02(this, birdMom);
    }

    private IEnumerator ProcessCatchBirdBabyQuest()
    {
        catchableObject.enabled = true;

        while (catchableObject.isCatched == true)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

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
            case 5:
                QuestManager.Instance.QuestSystem_AddQuest(quest_01);
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                canInteract = false;
                //if (PlayerController.Instance.currentNPC == this)//��ũ ��Ÿ�Ͱ� �����϶���
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
