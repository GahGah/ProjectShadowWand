using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public void SetConnectedBody(Rigidbody2D _rb);
    public void SetConnectedAnchor(Vector2 _vec);
    public void SetAutoAnchor(bool _b);

    /// <summary>
    /// 이 오브젝트를 밉니다.
    /// </summary>
    public void GoPushThis();

    /// <summary>
    /// 이 오브젝트를 내려놓습니다.
    /// </summary>
    public void GoPutThis();
}
