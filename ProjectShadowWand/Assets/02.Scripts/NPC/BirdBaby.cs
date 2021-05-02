using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBaby : MonoBehaviour
{

    TalkStarter talkStarter;

    public int[] talkCodes;

    public int currentStartTalkCode;


    [Tooltip("대화한 횟수")]
    public int currentTalkCount;

    [Tooltip("데려간 상태인가?")]
    public bool isTogether;

    public bool isFirstTalk;

    public bool isMeetMom;
    public Transform babyPos;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        talkStarter = GetComponent<TalkStarter>();
        talkStarter.isEnd = false;
        isTogether = false;
        isFirstTalk = true;
        currentStartTalkCode = talkCodes[0];
    }

    private void Update()
    {
        if (isFirstTalk == true) //처음 말 거는 것이라면...
        {
            currentStartTalkCode = talkCodes[0];
            talkStarter.talkCode = currentStartTalkCode;
        }

        if (talkStarter.isEnd == true)
        {
            isFirstTalk = false;
            if (isMeetMom == false)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                Debug.Log("미트 맘");
                gameObject.transform.position = babyPos.position;
            }
        }

        if (talkStarter.isEnd == true && isTogether == false)
        {
            isTogether = true;
        }

    }
}
