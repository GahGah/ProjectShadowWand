using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 번개에 맞으면 설정한 기계들을 작동시킵니다.
/// </summary>
public class MachineFuseBox : ElectricableObject
{
    public Machine[] machines;

    private void OnEnable()
    {
        electricableType = eElectricableType.WORK;
    }

    public override void OnLightining()
    {
        base.OnLightining();

        for (int i = 0; i < machines.Length; i++)
        {
            //작동 코루틴을 실행
            machines[i].UpdateWorkCoroutine();
        }
    }
}
