using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 스테이지 정보를 가지고 노는 매니저 클래스.
/// </summary>
public class StageManager : MonoBehaviour
{
    private List<SoulMemory> soulMemoryList;

    private void Awake()
    {
        InitSoulMemoryList();
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
        CheckSoulMemoryCondition();

    }

    /// <summary>
    /// 소울 메모리들의 상태를 보고 클리어 조건을 만족했는지를 체크합니다.
    /// </summary>
    private void CheckSoulMemoryCondition()
    {

    }
}
