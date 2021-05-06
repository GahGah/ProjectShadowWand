using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;

    [Tooltip("UI�� �����°�?")]
    public bool isEnd;
    private void Start()
    {
        stageManager = StageManager.Instance;

        if (stageManager.soulMemoryList.Contains(this)) // ����Ʈ �ȿ� �ڱⰡ �ȵ��ִٸ�
        {
            Debug.LogError("�������� �Ŵ��� ��� ����Ʈ�� " + gameObject.name + "�� �־��ּ���.");
        }
    }
}
