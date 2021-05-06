using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ������ ������ ��� �Ŵ��� Ŭ����.
/// </summary>
public class StageManager : MonoBehaviour
{
    public List<SoulMemory> soulMemoryList;

    [Header("Ư�� ����Ʈ�� Ŭ�����ؾ��� ��� üũ")]
    [Tooltip("���� ���������� �Ѿ�� ���ؼ� �����ϴ� ����Ʈ ������ �ִ°�?")]
    public bool isQuestExist;
    [Header("�� ��")]
    [Tooltip("����Ʈ�� Ŭ������ �����ΰ�?")]
    public bool isClear_Quest;

    [Tooltip("����� ���� ���� �����ΰ�?")]
    public bool isClear_SoulMemory;

    [Tooltip("[�ǵ��� IsStageClear() �Լ��� ����ϴ� ���� ����] ����Ʈ�� ��� ������ ���� �Ϸ��� �����ΰ�? ")]
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
        CheckClearCondition_SoulMemory();

    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdateQuestExist()
    {

    }
    /// <summary>
    /// �ҿ� �޸𸮵��� ���¸� ���� Ŭ���� ������ �����ߴ����� üũ�մϴ�.
    /// </summary>
    public void CheckClearCondition_SoulMemory()
    {
        bool isEnd_All = true;

        for (int i = 0; i < soulMemoryList.Count; i++)
        {
            if (soulMemoryList[i].isEnd == false) //�ϳ��� isEnd�� �ƴ϶��
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
    }

    //public void CheckClearCondition_Quest()
    //{

    //}

    /// <summary>
    /// ����Ʈ�� ��� ���¿� ���� isStageClear�� ������Ʈ�մϴ�.
    /// </summary>
    /// <returns> Ŭ���� ������ �����ߴٸ� true, �������� �ʾҴٸ� false�� ��ȯ�մϴ�.</returns>
    public bool IsStageClear()
    {
        if (isQuestExist == true)//Ŭ���� ���ǿ� ����Ʈ�� �ִٸ�
        {
            if (isClear_Quest && isClear_SoulMemory)
            {
                isStageClear = true;
            }
            else
            {
                isStageClear = false;
            }
        }
        else
        {
            if (isClear_SoulMemory)
            {
                isStageClear = true;
            }
            else
            {
                isStageClear = false;
            }
        }
        return isStageClear;

    }

    //public void SetStageClear_SoulMemory()
    //{

    //}

    //public void SetStageClear_LastQuest()
    //{

    //}


}
