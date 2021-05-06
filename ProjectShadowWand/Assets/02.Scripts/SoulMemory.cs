using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;
    private void Start()
    {
        stageManager = StageManager.Instance;

        if (stageManager.soulMemoryList.Contains(this)) // 리스트 안에 자기가 안들어가있다면
        {
            Debug.LogError("스테이지 매니저 사념 리스트에 " + gameObject.name + "을 넣어주세요.");
        }
    }
}
