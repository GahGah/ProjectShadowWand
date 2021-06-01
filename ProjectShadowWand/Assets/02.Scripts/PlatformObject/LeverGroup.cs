using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGroup : MonoBehaviour
{

    //[Header("레버 그룹에 포함된 레버")]
    //[Tooltip("레버 그룹에 포함된 레버 리스트. 직접 넣어놓지 않으면 자동으로 추가되긴 합니다. ")]
    //public List<Lever> levers;

    //[HideInInspector]
    //[Tooltip("현재 On 상태인 레버.")]
    //public Lever currentActiveLever;

    //private int count;
    //private void Init()
    //{
    //    if (levers.Count == 0)
    //    {
    //        var tempLevers = GetComponentsInChildren<Lever>();
    //        for (int i = 0; i < tempLevers.Length; i++)
    //        {
    //            levers.Add(tempLevers[i]);
    //            tempLevers[i].SetLeverGroupAndIndex(this, i);
    //        }
    //        Debug.Log(gameObject.name + ": 레버 자동 추가 완료!");
    //    }
    //    else
    //    {
    //        for (int i = 0; i < levers.Count; i++)
    //        {
    //            levers[i].SetLeverGroupAndIndex(this, i);
    //        }


    //    }

    //    count = levers.Count;

    //    currentActiveLever = null;
    //}
    //private void Awake()
    //{
    //    Init();
    //}


    ///// <summary>
    ///// 현재 켜져있는 레버를 끄고, 켜져있는 레버를 levers[_index]로 설정합니다.
    ///// </summary>
    ///// <param name="_index">레버의 인덱스~</param>
    //public void UpdateLeverToggle(int _index)
    //{
    //    if (ReferenceEquals(currentActiveLever, null) == false) // null이 아닐 떄에만
    //    {
    //        currentActiveLever.SetIsOn(false);
    //    }
    //    currentActiveLever = levers[_index];
    //}


}
