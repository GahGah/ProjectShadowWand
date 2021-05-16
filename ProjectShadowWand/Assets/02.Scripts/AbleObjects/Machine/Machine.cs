using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [Tooltip("����� �۵��� ���Ǵ� �ڷ�ƾ�Դϴ�.")]
    public IEnumerator WorkCoroutine;

    [Tooltip("��谡 �۵����� �����ΰ�?")]
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
