using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMom : NPC
{
    TalkStarter talkStarter;

    public BirdBaby birdBaby;

    public int[] talkCodes;

    public int currentStartTalkCode;

    [Tooltip("대화한 횟수")]
    public int currentTalkCount;


    [Tooltip("첫번째 대화를 완수헀는지를 뜻합니다.")]
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

        if (isFirstTalk == true) //처음 말 거는 것이라면...
        {
            currentStartTalkCode = talkCodes[0];
            talkStarter.talkCode = currentStartTalkCode;
        }
        else
        {
            if (birdBaby.isTogether == false) //같이 있는 상태가 아니라면
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
