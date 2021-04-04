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


    public GameObject tornadoObject_Collider;
    public GameObject tornadoObject_Trigger;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        //�÷��̾�� ����̵� �ݶ��̴����� �浹�� ����.
        LayerMask layer1 = LayerMask.NameToLayer("Tornado_Collider");
        LayerMask layer2 = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(layer1, layer2, true);

    }
}
