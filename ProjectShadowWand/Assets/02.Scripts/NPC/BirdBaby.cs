using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBaby : MonoBehaviour
{

    TalkStarter talkStarter;

    public int[] talkCodes;

    public int currentStartTalkCode;


    [Tooltip("��ȭ�� Ƚ��")]
    public int currentTalkCount;

    [Tooltip("������ �����ΰ�?")]
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
        if (isFirstTalk == true) //ó�� �� �Ŵ� ���̶��...
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
                Debug.Log("��Ʈ ��");
                gameObject.transform.position = babyPos.position;
            }
        }

        if (talkStarter.isEnd == true && isTogether == false)
        {
            isTogether = true;
        }

    }
}
