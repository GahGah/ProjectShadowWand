using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : MonoBehaviour
{
    /// <summary>
    /// 플레이어와 기타 오브젝트와 충돌하지 않는 리지드 바디. (중요)
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
