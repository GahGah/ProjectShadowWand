using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMom : NPC
{
    TalkStarter talkStarter;

    public BirdBaby birdBaby;

    public int[] talkCodes;

    public int currentStartTalkCode;

    [Tooltip("��ȭ�� Ƚ��")]
    public int currentTalkCount;


    [Tooltip("ù��° ��ȭ�� �ϼ��������� ���մϴ�.")]
    public bool isFirstTalk;

    public Transform babyPos;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        talkStarter = GetComponent<TalkStarter>();

        isFirstTalk = true;

        talkStarter.isEnd = false;
        currentStartTalkCode = talkCodes[0];
    }

    private void Update()
    {
        if (talkStarter.isEnd == true)
        {
            isFirstTalk = false;
        }

        if (isFirstTalk == true) //ó�� �� �Ŵ� ���̶��...
        {
            currentStartTalkCode = talkCodes[0];
            talkStarter.talkCode = currentStartTalkCode;
        }
        else
        {
            if (birdBaby.isTogether == false) //���� �ִ� ���°� �ƴ϶��
            {
                currentStartTalkCode = talkCodes[1];
                talkStarter.talkCode = currentStartTalkCode;
            }
            else
            {
                currentStartTalkCode = talkCodes[2];
                talkStarter.talkCode = currentStartTalkCode;
                if (birdBaby.isMeetMom == false && talkStarter.isStart == true)
                {
                    birdBaby.isMeetMom = true;
                }

            }
        }
    }
}
