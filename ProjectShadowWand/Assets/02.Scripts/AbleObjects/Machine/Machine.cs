using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Tooltip("기계의 작동에 사용되는 코루틴입니다.")]
    public IEnumerator WorkCoroutine;

    [Tooltip("기계가 작동중인 상태인가?")]
    public bool isWorking;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        WorkCoroutine = ProcessWork();
    }


    public void UpdateWorkCoroutine()
    {
        if (isWorking == true)
        {
            isWorking = false;
            StopCoroutine(WorkCoroutine);
        }
        else
        {
            isWorking = true;
            StartCoroutine(WorkCoroutine);
        }

    }


    protected virtual IEnumerator ProcessWork()
    {
        Debug.Log("Start Work");
        while (isWorking)
        {
            gameObject.transform.Rotate(0f, 0f, 5f);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        Debug.LogWarning("End Work");
        
    }
}
