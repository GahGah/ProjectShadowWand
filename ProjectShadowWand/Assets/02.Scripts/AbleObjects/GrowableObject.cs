using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 물에 닿으면 자라는 오브젝트입니다.
/// </summary>
public class GrowableObject : MonoBehaviour, IWaterable
{
    [HideInInspector]
    [Tooltip("물에 한 번이라도 젖은 적이 있다면 true가 됩니다.")]
    public bool isWetted = false;
    public void OnWater()
    {
        if (isWetted == false)
        {
            isWetted = true;
        }
    }

}
