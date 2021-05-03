using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ϴ��� ��ȭ�� ����Ʈ�� �Ϸ�/ ������ �׷� �ý���.
/// </summary>
public class QuestManager : MonoBehaviour
{

    /// <summary>
    /// �ϴ��� ��ȭ�� ����Ʈ�� �Ϸ�/ ������ �׷� �ý���.
    /// </summary>
    //Dictionary<eQuestCode, Quest> questDict;


    List<Quest> questList;

    public NPC currentNPC;
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {

        if (Instance == null)
        {
            instance = this;
        }
        Init();
    }

    public void Init()
    {
        //questDict = new Dictionary<eQuestCode, Quest>();
        
        questList = new List<Quest>();

    }



    public void QuestSystem_TalkStart(NPC _npc)
    {
        
        if (questList.Count != 0)
        {
            var index = questList.Count;
            for (int i = 0; i < index; i++)
            {
                Debug.Log(questList[i].GetType() + "���� " + _npc.GetType() + "�����մϴ�. ");
                questList[i].StartTalk(_npc);
            }
        }
    }

    public void QuestSystem_TalkEnd(NPC _npc)
    {
        if (questList.Count != 0)
        {
            var index = questList.Count;
            for (int i = 0; i < index; i++)
            {
                questList[i].EndTalk(_npc);
            }
        }
    }

    public void QuestSystem_AddQuest(Quest _quest)
    {
        _quest.StartQuest();
        if (questList.Contains(_quest) == false)//���� ��쿡��
        {
            Debug.Log("����Ʈ ��� : " + _quest.GetType());
            questList.Add(_quest);
        }
        else
        {
            Debug.Log("����Ʈ ��Ͽ� �����߽��ϴ�. ����Ʈ�� �̹� ���� ����Ʈ�� �����մϴ�  : " + _quest.GetType());
        }
    }

    public void QuestSystem_RemoveQuest(Quest _quest)
    {
        _quest.EndQuest();
        if (questList.Contains(_quest) == true) //���� ��쿡��
        {
            Debug.Log("����Ʈ ���� : " + _quest.GetType());
            questList.Remove(_quest);
        }
        else
        {
            Debug.Log("����Ʈ ������ �����߽��ϴ�. ����Ʈ�� �������� �ʴ� ����Ʈ�Դϴ�. : " + _quest.GetType());
        }
    }
}
