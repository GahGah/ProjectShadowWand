using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : GrowableObject
{
    [Tooltip("�� �ڶ����� �����Ǵ� �Ĺ�������Ʈ�Դϴ�.")]
    GameObject plantObject;

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
    }

    public override void StartGrow()
    {
        base.StartGrow();
    }

    private IEnumerator ProcessGrow()
    {
        yield break;
    }
}
