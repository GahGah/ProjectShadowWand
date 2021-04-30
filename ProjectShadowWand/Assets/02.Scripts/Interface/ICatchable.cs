using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 잡을 수 있는 오브젝트 인터페이스
/// </summary>
public interface ICatchable
{
    //public void SetConnectedBody(Rigidbody2D _rb);
    //public void SetConnectedAnchor(Vector2 _vec);
    //public void SetAutoAnchor(bool _b);

    /// <summary>
    /// 이 오브젝트를 잡습니다.
    /// </summary>
    public void GoCatchThis();

    /// <summary>
    /// 이 오브젝트를 내려놓습니다.
    /// </summary>
    public void GoPutThis();
}
