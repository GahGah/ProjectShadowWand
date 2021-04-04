using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaEffector�� �䳻�� �� ���Դϴ�. �ϴ� ���� ���� ����� ������
/// </summary>
public class CustomAreaEffector : MonoBehaviour
{

    [Header("����")]
    public float angle;

    [Header("��")]
    public float power;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            var tempVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            collision.attachedRigidbody.AddForce(tempVector * power, ForceMode2D.Force);
        }

    }
}
