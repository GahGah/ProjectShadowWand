using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� �� �ִ� ������Ʈ �������̽�
/// </summary>
public interface ICatchable
{
    //public void SetConnectedBody(Rigidbody2D _rb);
    //public void SetConnectedAnchor(Vector2 _vec);
    //public void SetAutoAnchor(bool _b);

    /// <summary>
    /// �� ������Ʈ�� ����ϴ�.
    /// </summary>
    public void GoCatchThis();

    /// <summary>
    /// �� ������Ʈ�� ���������ϴ�.
    /// </summary>
    public void GoPutThis();
}
