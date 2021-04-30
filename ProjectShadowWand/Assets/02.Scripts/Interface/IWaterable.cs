using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 물에 닿으면 변화가 생기는 오브젝트에 쓰입니다.
/// </summary>
public interface IWaterable
{

    /// <summary>
    /// 물에 맞았을 때
    /// </summary>
    public void OnWater();
}
