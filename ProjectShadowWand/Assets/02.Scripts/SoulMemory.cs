using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;
    private void Start()
    {
        stageManager = StageManager.Instance;

        if (stageManager.soulMemoryList.Contains(this)) // ����Ʈ �ȿ� �ڱⰡ �ȵ��ִٸ�
        {
            Debug.LogError("�������� �Ŵ��� ��� ����Ʈ�� " + gameObject.name + "�� �־��ּ���.");
        }
    }
}
