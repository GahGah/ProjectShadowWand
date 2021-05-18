using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMom : NPC
{
    Quest_MomAndBaby_01 quest_01;
    Quest_MomAndBaby_02 quest_02;

    public BirdBaby birdBaby;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        currentTalkCode = 0;
        canInteract = true;
    }

    public override void StartTalk()
    {
        switch (currentTalkCode)
        {
            case 0:
                QuestManager.Instance.QuestSystem_AddQuest(new Quest_MomAndBaby_01(birdBaby, this));
                TalkSystemManager.Instance.StartGoTalk(currentTalkCode, this);
                currentTalkCode = 4;
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
