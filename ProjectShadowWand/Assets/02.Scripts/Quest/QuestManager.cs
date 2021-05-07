using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 일단은 대화로 퀘스트를 완료/ 끝내는 그런 시스템.
/// </summary>
public class QuestManager : MonoBehaviour
{

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

    /// <summary>
    /// 퀘스트 목록에 있는 모든 Quest의 StartTalk(전달할 NPC)함수를 호출합니다.
    /// </summary>
    /// <param name="_npc">전달할 NPC</param>
    public void QuestSystem_TalkStart(NPC _npc)
    {
        if (questList.Count != 0)
        {
            var index = questList.Count;
            for (int i = 0; i < index; i++)
            {
                //Debug.Log(questList[i].GetType() + "에게 " + _npc.GetType() + "전달합니다. ");
                questList[i].StartTalk(_npc);
            }
        }
    }

    /// <summary>
    /// 퀘스트 목록에 있는 모든 Quest의 EndTalk(전달할 NPC)함수를 호출합니다.
    /// </summary>
    /// <param name="_npc"></param>
    public void QuestSystem_TalkEnd(NPC _npc)
    {
        Quest prevQuest = new Quest();
        if (questList.Count != 0)
        {
            var index = questList.Count;
            for (int i = 0; i < index; i++)
            {
                if (prevQuest == questList[i])
                {
                    Debug.LogWarning("어째서 이전 퀘스트와 현재 퀘스트가 같은 것인가?");
                }
                prevQuest = questList[i];
                questList[i].EndTalk(_npc);
            }
        }
    }

    /// <summary>
    /// 해당 퀘스트의 StartQuest()를 호출하고, 퀘스트 목록에서 해당 퀘스트를 삭제합니다.
    /// </summary>
    /// <param name="_quest"></param>
    public void QuestSystem_AddQuest(Quest _quest)
    {
        _quest.StartQuest();
        if (questList.Contains(_quest) == false)//없을 경우에만
        {
            Debug.Log("퀘스트 등록 : " + _quest.GetType());
            questList.Add(_quest);
        }
        else
        {
            Debug.Log("퀘스트 등록에 실패했습니다. 리스트에 이미 같은 퀘스트가 존재합니다  : " + _quest.GetType());
        }
    }

    /// <summary>
    /// 해당 퀘스트의 EndQuest()를 호출하고, 퀘스트 목록에서 해당 퀘스트를 삭제합니다.
    /// </summary>
    /// <param name="_quest"> 삭제할 퀘스트입니다.</param>
    /// <param name="_callEnd">true일 때만 EndQuest() 함수를 호출합니다.</param>
    public void QuestSystem_RemoveQuest(Quest _quest, bool _callEnd)
    {
        if (_callEnd)
        {
            _quest.EndQuest();
        }
        if (questList.Contains(_quest) == true) //있을 경우에만
        {
            Debug.Log("퀘스트 삭제 : " + _quest.GetType());
            questList.Remove(_quest);
            //if (_quest != null)
            //{

            //}
        }
        else
        {
            Debug.Log("퀘스트 삭제에 실패했습니다. 리스트에 존재하지 않는 퀘스트입니다. : " + _quest.GetType());
        }
    }
}
