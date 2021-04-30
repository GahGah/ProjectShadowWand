using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �ڶ�� ������Ʈ�Դϴ�.
/// </summary>
public class GrowableObject : MonoBehaviour, IWaterable
{
    [HideInInspector]
    [Tooltip("���� �� ���̶� ���� ���� �ִٸ� true�� �˴ϴ�.")]
    public bool isWetted = false;
    public void OnWater()
    {
        if (isWetted == false)
        {
            isWetted = true;
        }
    }

}
