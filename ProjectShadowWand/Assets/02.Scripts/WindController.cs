using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ٶ��� ����ϴ� ��Ʈ�ѷ�
/// </summary>
public class WindController : MonoBehaviour
{
    [Header("�ٶ� ����")]
    public Collider2D windArea;

    [Header("�ٶ��� ����")]
    public eWindDirection windDirection;

    [Header("�ٶ� ����")]
    public float windPower;

    [Header("������Ʈ���� �ִ� �̵��ӵ�")]
    public float maxWindSpeed;

    [Header("�ٶ��� ������ ���� �ʴ� ���̾�")]
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
