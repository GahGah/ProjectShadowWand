using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    [Tooltip("�ش� ����Ʈ�� �ڵ�.")]
    public eQuestCode questCode;

    [Tooltip("�ش� ����Ʈ�� ������ �ִ� NPC�Դϴ�.")]
    public NPC npc;

    public virtual void StartTalk(NPC _npc)
    {
        if (_npc != null)
        {
            Debug.Log("Start Talk ����Ʈ npc �̸� : " + _npc.GetType());
        }
    }
    public virtual void StartQuest() { }
    public virtual void ExecuteQuest() { }
    public virtual void EndQuest() { }
    public virtual void EndTalk(NPC _npc)
    {
        if (_npc != null)
        {
            Debug.Log("End Talk npc �̸� : " + _npc.GetType());
        }

    }
}

public class Quest_MomAndBaby_01 : Quest
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
        baby.gameObject.SetActive(true);
    }

    public override void ExecuteQuest()
    {

    }

    public override void EndQuest()
    {
        baby.gameObject.SetActive(false);
    }

    public override void EndTalk(NPC _npc)
    {
        base.EndTalk(_npc);

        if (_npc == baby) //���̿� ���� �ߴٸ�
        {
            QuestManager.Instance.QuestSystem_RemoveQuest(this);
            QuestManager.Instance.QuestSystem_AddQuest(new Quest_MomAndBaby_02(baby, mom));
            mom.currentTalkCode = 8;
            PlayerController.Instance.playerSkillManager.UnlockWind();
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
        if (_npc == mom) //��...�̶��
        {
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
    }
    public override void EndTalk(NPC _npc)
    {
        base.EndTalk(_npc);
        if (_npc == mom)
        {
            QuestManager.Instance.QuestSystem_RemoveQuest(this);
            mom.currentTalkCode = 12;
            baby.currentTalkCode = -1;
            baby.birdCollider.enabled = false;
        }
    }
}
