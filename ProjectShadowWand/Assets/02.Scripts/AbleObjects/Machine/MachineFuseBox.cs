using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ������ ������ ������ �۵���ŵ�ϴ�.
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
            //�۵� �ڷ�ƾ�� ����
            machines[i].UpdateWorkCoroutine();
        }
    }
}
