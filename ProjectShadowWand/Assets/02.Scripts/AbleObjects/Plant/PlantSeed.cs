using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : GrowableObject
{
    [Header("Ŀ�� ���� �Ǵ°Ŵ�?")]
    [Tooltip("�� �ڶ����� �����Ǵ� �Ĺ�������Ʈ�Դϴ�.")]
    public GameObject plantObject;

    [Header("�ִϸ�����")]
    public Animator animator;
    private void Start()
    {
        GrowCoroutine = ProcessGrow();
    }
    public void Init()
    {
        if (growTime <= 0f)
        {
            growTime = 5f;
        }

        if (currentGrowTime >= 0f)
        {
            currentGrowTime = 0f;
        }

        GrowCoroutine = ProcessGrow();
    }
    public override void EndGrow()
    {
        base.EndGrow();
    }

    public override void OnWater()
    {
        base.OnWater();
        StartGrow();
    }

    public override void StartGrow()
    {
        base.StartGrow();
    }

    private IEnumerator ProcessGrow()
    {
        currentGrowTime = 0f;

        while (currentGrowTime < 1f)
        {
            currentGrowTime += Time.fixedDeltaTime / growTime;

            currentPer = Mathf.Lerp(0f, 1f, currentGrowTime);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        EndGrow();
        yield break;
    }
}
