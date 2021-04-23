using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    public void SetConnectedBody(Rigidbody2D _rb);
    public void SetConnectedAnchor(Vector2 _vec);
    public void SetAutoAnchor(bool _b);

    /// <summary>
    /// �� ������Ʈ�� �Ӵϴ�.
    /// </summary>
    public void GoPushThis();

    /// <summary>
    /// �� ������Ʈ�� ���������ϴ�.
    /// </summary>
    public void GoPutThis();
}
