using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 스테이지 정보를 가지고 노는 매니저 클래스.
/// </summary>
public class StageManager : MonoBehaviour
{
    public List<SoulMemory> soulMemoryList;

    [Header("특정 퀘스트를 클리어해야할 경우 체크")]
    [Tooltip("다음 스테이지로 넘어가기 위해서 존재하는 퀘스트 조건이 있는가?")]
    public bool isQuestExist;

    [Header("사념을 획득해야할 경우 체크")]
    [Tooltip("당므스테이지로 넘어가기 위해서 존재하는 사념이 있는가?")]
    public bool isSoulMemoryExist;
    [Header("그 외")]
    [Tooltip("퀘스트를 클리어한 상태인가?")]
    public bool isClear_Quest;

    [Tooltip("사념을 전부 모은 상태인가?")]
    public bool isClear_SoulMemory;

    [Tooltip("[되도록 IsStageClear() 함수를 사용하는 것이 좋음] 퀘스트와 사념 조건을 전부 완료한 상태인가? ")]
    public bool isStageClear;


    private static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
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
        InitSoulMemoryList();

        if (IsStageClear()) //시작부터 클리어일 경우
        {
            Debug.Log("이 스테이지는 퀘스트와 사념이 존재하지 않습니다. 그냥 클리어 처리 합니다.");
        }
        else
        {
            StartCoroutine(ProcessStageClear());
        }

    }
    private void Start()
    {

    }

    public void InitSoulMemoryList()
    {
        if (soulMemoryList.Count == 0)
        {
            soulMemoryList = new List<SoulMemory>();
        }
    }

    /// <summary>
    /// 소울메모리 리스트에 추가합니다.
    /// </summary>
    public void AddSoulMemory(SoulMemory _soulMemory)
    {
        soulMemoryList.Add(_soulMemory);
        CheckClearCondition_SoulMemory();

    }


    /// <summary>
    /// 스테이지 클리어까지 기다리다가 함수를 호출합니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ProcessStageClear()
    {
        yield return new WaitUntil(() => isStageClear);
        //스테이지 클리어가 true일 때 까지 대기
        Debug.Log("클리어 조건 만족!");


    }

    /// <summary>
    /// 소울 메모리들의 상태를 보고 클리어 조건을 만족했는지를 체크합니다.
    /// </summary>
    public bool CheckClearCondition_SoulMemory()
    {
        bool isEnd_All = true;

        for (int i = 0; i < soulMemoryList.Count; i++)
        {
            if (soulMemoryList[i].isEnd == false) //하나라도 isEnd가 아니라면
            {
                isEnd_All = false;
                break;
            }

        }

        if (isEnd_All)
        {
            isClear_SoulMemory = true;
        }
        else
        {
            isClear_SoulMemory = false;
        }

        return isClear_SoulMemory;
    }

    /// <summary>
    /// 마지막 퀘스트의 클리어 여부를 설정합니다. 그리고, IsStageClear()함수를 호출합니다.
    /// </summary>
    /// <param name="_b">true시 클리어, false시 아직 클리어 안함.</param>
    public void SetLastQuestClear(bool _b)
    {
        isClear_Quest = _b;
        IsStageClear();
    }
    //public void CheckClearCondition_Quest()
    //{

    //}

    /// <summary>
    /// 퀘스트와 사념 상태에 따라 isStageClear를 업데이트합니다.
    /// </summary>
    /// <returns> 클리어 조건을 만족했다면 true, 만족하지 않았다면 false를 반환합니다.</returns>
    public bool IsStageClear()
    {
        var clearQ = false;
        var clearS = false;
        if (isQuestExist == true)//클리어 조건에 퀘스트가 있다면
        {

            clearQ = isClear_Quest;
        }
        else //퀘스트가 없다면
        { //무조건 완료로
            clearQ = true;
        }

        if (isSoulMemoryExist == true) //클리어 조건에 사념이 있다면
        {
            clearS = isClear_SoulMemory;
        }
        else
        {
            clearS = true;
        }


        isStageClear = clearQ && clearS;

        return isStageClear;

    }

    //public void SetStageClear_SoulMemory()
    //{

    //}

    //public void SetStageClear_LastQuest()
    //{

    //}


}
