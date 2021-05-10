using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelDoritos : NPC
{

    Quest_CheckTheSoulMemory quest_01;
    SoulMemory soulMemory;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void StartTalk()
    {

        if (currentTalkCode == 0)
        {

            if (soulMemory.isEnd) //이미 해당 사념을 읽었다면
            {
                currentTalkCode = 8;
            }

        }
        else if (currentTalkCode == 4)
        {
            if (soulMemory.isEnd == true)
            {
                currentTalkCode = 5;
            }

        }



        switch (currentTalkCode)
        {
            case 0:
                QuestManager.Instance.QuestSystem_AddQuest(new Quest_CheckTheSoulMemory(this, soulMemory));
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                currentTalkCode = 4;
                break;

            case 4:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;

            case 8:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                currentTalkCode = 13;
                break;
            default:
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                break;
                //case 4:
                //    TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                //    break;
                //case 8:
                //    TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                //    break;
                //case 12:
                //    Talk
        }
    }
}
