using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ������ ������ ��� �Ŵ��� Ŭ����.
/// </summary>
public class StageManager : MonoBehaviour
{
    public List<SoulMemory> soulMemoryList;

    [Tooltip("")]
    public bool isClear_Quest;
    public bool isClear_SoulMemory;

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
    }

    public void InitSoulMemoryList()
    {
        if (soulMemoryList.Count == 0)
        {
            soulMemoryList = new List<SoulMemory>();
        }
    }

    /// <summary>
    /// �ҿ�޸� ����Ʈ�� �߰��մϴ�.
    /// </summary>
    public void AddSoulMemory(SoulMemory _soulMemory)
    {
        soulMemoryList.Add(_soulMemory);
        CheckSoulMemoryCondition();

    }

    /// <summary>
    /// �ҿ� �޸𸮵��� ���¸� ���� Ŭ���� ������ �����ߴ����� üũ�մϴ�.
    /// </summary>
    private void CheckSoulMemoryCondition()
    {

    }

    //public void SetStageClear_SoulMemory()
    //{

    //}

    //public void SetStageClear_LastQuest()
    //{

    //}


}
