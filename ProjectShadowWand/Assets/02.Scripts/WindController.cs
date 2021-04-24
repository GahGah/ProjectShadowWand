using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 바람을 담당하는 컨트롤러
/// </summary>
public class WindController : MonoBehaviour
{
    [Header("바람 영역")]
    public Collider2D windArea;

    [Header("바람의 방향")]
    public eWindDirection windDirection;

    [Header("바람 세기")]
    public float windPower;

    [Header("오브젝트들의 최대 이동속도")]
    public float maxWindSpeed;

    [Header("바람의 영향을 받지 않는 레이어")]
    public LayerMask notAffectedLayer;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log("Test");
    }
    public void CreateEdgeCollider()
    {
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != notAffectedLayer.value)
        {
            collision.attachedRigidbody.AddForce(GetDirection(windDirection) * windPower, ForceMode2D.Force);
            collision.attachedRigidbody.velocity =
                new Vector2(Mathf.Clamp(collision.attachedRigidbody.velocity.x, -maxWindSpeed, maxWindSpeed),
                            Mathf.Clamp(collision.attachedRigidbody.velocity.y, -maxWindSpeed, maxWindSpeed));
        }
    }

    private Vector2 GetDirection(eWindDirection _dir)
    {
        if (_dir == eWindDirection.LEFT)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }
}
