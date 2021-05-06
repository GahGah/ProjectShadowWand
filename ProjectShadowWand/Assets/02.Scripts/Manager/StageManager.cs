using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ������ ������ ��� �Ŵ��� Ŭ����.
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
}
