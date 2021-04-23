using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 번개로 인해 전기가 통할 수 있는 오브젝트입니다.
/// </summary>
public interface IElectricable
{
    //번개에 맞았을 떄
    public void OnThunder();
}
