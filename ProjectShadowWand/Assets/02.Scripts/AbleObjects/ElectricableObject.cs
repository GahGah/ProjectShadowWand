using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 번개의 영향을 받는 오브젝트입니다.
/// </summary>
public class ElectricableObject : MonoBehaviour, IElectricable
{
    [Header("타입 : 부서지냐/작동하냐"), Tooltip("WORK면 작동, DESTROY면 부서집니다.")]
    public eElectricableType electricableType;

    [HideInInspector]
    [Header("번개에 맞은 적이 있는가!"), Tooltip("번개에 한 번이라도 맞았을 경우 true가 됩니다.")]
    public bool isShocked = false;

    /// <summary>
    /// 번개에 맞았을 때 해당 함수를 호출합시다.
    /// </summary>
    public void OnLightining()
    {
 
        if (isShocked == false)
        {
            isShocked = true;
            Debug.Log("찌리릿!!! : " + name);
        }
    }
}
