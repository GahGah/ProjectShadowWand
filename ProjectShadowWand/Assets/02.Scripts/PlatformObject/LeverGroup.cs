using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverGroup : MonoBehaviour
{

    //[Header("���� �׷쿡 ���Ե� ����")]
    //[Tooltip("���� �׷쿡 ���Ե� ���� ����Ʈ. ���� �־���� ������ �ڵ����� �߰��Ǳ� �մϴ�. ")]
    //public List<Lever> levers;

    //[HideInInspector]
    //[Tooltip("���� On ������ ����.")]
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
    //        Debug.Log(gameObject.name + ": ���� �ڵ� �߰� �Ϸ�!");
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
    ///// ���� �����ִ� ������ ����, �����ִ� ������ levers[_index]�� �����մϴ�.
    ///// </summary>
    ///// <param name="_index">������ �ε���~</param>
    //public void UpdateLeverToggle(int _index)
    //{
    //    if (ReferenceEquals(currentActiveLever, null) == false) // null�� �ƴ� ������
    //    {
    //        currentActiveLever.SetIsOn(false);
    //    }
    //    currentActiveLever = levers[_index];
    //}


}
