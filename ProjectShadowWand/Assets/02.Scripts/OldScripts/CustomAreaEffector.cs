using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AreaEffector를 흉내낸 것 뿐입니다. 일단 대충 만들어서 기능이 부족함
/// </summary>
public class CustomAreaEffector : MonoBehaviour
{

    [Header("각도"), Range(-180, 180)]
    public float angle;

    [Header("힘")]
    public float power;

    int layerMask;

    private void Awake()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            var tempVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
            collision.attachedRigidbody.AddForce(tempVector * power, ForceMode2D.Force);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var tempVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        Gizmos.DrawLine(tempVector, tempVector * power);
        Gizmos.DrawLine(tempVector * power, Vector2.left * 0.3f);
        Gizmos.DrawLine(tempVector * power, Vector2.right * 0.3f);
    }
}
