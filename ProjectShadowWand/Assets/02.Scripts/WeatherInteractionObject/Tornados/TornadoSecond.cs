using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : MonoBehaviour
{
    /// <summary>
    /// �÷��̾�� ��Ÿ ������Ʈ�� �浹���� �ʴ� ������ �ٵ�. (�߿�)
    /// </summary>
    public Rigidbody2D tornadoRigidBody; 
    public Collider2D tornadoTrigger;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {

    }
}
