using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    [Tooltip("해당 퀘스트의 코드.")]
    public eQuestCode questCode;

    [Tooltip("해당 퀘스트를 가지고 있는 NPC입니다.")]
    public NPC npc;

    /// <summary>
    /// 토크 매니저에서 대화를 시작할 때 호출됩니다.
    /// </summary>
    public virtual void StartTalk(NPC _npc)
    {
        //if (_npc != null)
        //{
        //    Debug.Log("Start Talk 퀘스트 npc 이름 : " + _npc.GetType());
        //}
    }
    /// <summary>
    /// 퀘스트가 추가될 때 자동으로 호출되는 함수입니다.
    /// </summary>
    public virtual void StartQuest() { }

    /// <summary>
    /// 퀘스트가 실행되고 있는 도중 호출되는 함수입니다만, 실제로 사용되어지지 않았습니다.
    /// </summary>
    public virtual void ExecuteQuest() { }

    /// <summary>
    /// 퀘스트가 끝날 때 자동으로 호출되는 함수입니다.
    /// </summary>
    public virtual void EndQuest()
    {
        Debug.Log("퀘스트 완료! : " + this.GetType());
    }

    /// <summary>
    /// 토크 매니저에서 대화를 시작할 때 호출됩니다.
    /// </summary>
    public virtual void EndTalk(NPC _npc)
    {
        //if (_npc != null)
        //{
        //    Debug.Log("End Talk npc 이름 : " + _npc.GetType());
        //}

    }
}

public class Quest_MomAndBaby_01 : Quest //나리를 들어올려라!
{
    BirdBaby baby;
    BirdMom mom;
    public Quest_MomAndBaby_01(BirdBaby _baby, BirdMom _mom)
    {
        mom = _mom;
        baby = _baby;
    }
    public override void StartTalk(NPC _npc)
    {
        base.StartTalk(_npc);
    }
    public override void StartQuest()
    {
    }

    public override void ExecuteQuest()
    {

    }

    public override void EndQuest()
    {
        base.EndQuest();
        //퀘스트가 끝나면 바로 아카에게 가야하는 퀘스트 추가
        QuestManager.Instance.QuestSystem_AddQuest(baby.quest_02);
    }

    public override void EndTalk(NPC _npc)
    {
        base.EndTalk(_npc);

        //?? : BirdBaby에서 리무브퀘스트를 호출하니 걱정 마세용.
        if (_npc == baby) //아이와 말을 했다면
        { //잡기 체크 시작
            baby.StartCatchBabyQuest();
        }


    }
}

public class Quest_MomAndBaby_02 : Quest
{
    BirdBaby baby;
    BirdMom mom;
    public Quest_MomAndBaby_02(BirdBaby _baby, BirdMom _mom)
    {
        mom = _mom;
        baby = _baby;
    }
    public override void StartTalk(NPC _npc)
    {
        base.StartTalk(_npc);
        if (_npc == mom) //맘...이라면
        {
            baby.catchableObject.GoPutThis();
            PlayerController.Instance.SetCatchedObject(null);
            baby.catchableObject.enabled = false;
            baby.gameObject.SetActive(true);
            baby.gameObject.transform.position = baby.momTogetherPos.position;
        }
    }
    public override void StartQuest()
    {

    }

    public override void ExecuteQuest()
    {

    }

    public override void EndQuest()
    {
        base.EndQuest();
        StageManager.Instance.SetLastQuestClear(true);
    }
    public override void EndTalk(NPC _npc)
    {
        base.EndTalk(_npc);
        if (_npc == mom)
        {
            QuestManager.Instance.QuestSystem_RemoveQuest(this, true);
            mom.currentTalkCode = 12;
            baby.currentTalkCode = -1;
            baby.birdCollider.enabled = false;
        }
    }
}

public class Quest_CheckTheSoulMemory : Quest
{

    SquirrelDoritos doritos;
    SoulMemory soulMemory;
    public Quest_CheckTheSoulMemory(SquirrelDoritos _doritos, SoulMemory _soulMemory)
    {
        doritos = _doritos;
        soulMemory = _soulMemory;
    }
    public override void StartTalk(NPC _npc)
    {
        base.StartTalk(_npc);
        if (_npc == doritos)
        {
            //딱히 없는듯

        }
    }
    public override void StartQuest()
    {

    }

    public override void ExecuteQuest()
    {

    }

    public override void EndQuest()
    {
        base.EndQuest();
        StageManager.Instance.SetLastQuestClear(true);
    }
    public override void EndTalk(NPC _npc)
    {
        base.EndTalk(_npc);
        if (soulMemory.isEnd == true)
        {
            if (_npc == doritos)
            {
                QuestManager.Instance.QuestSystem_RemoveQuest(this, true);
                doritos.currentTalkCode = 13;
                //mom.currentTalkCode = 12;
                //baby.currentTalkCode = -1;
                //baby.birdCollider.enabled = false;
            }
        }

    }
}