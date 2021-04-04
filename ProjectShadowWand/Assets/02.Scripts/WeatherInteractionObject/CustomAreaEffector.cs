using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaEffector를 흉내낸 것 뿐입니다. 일단 대충 만들어서 기능이 부족함
/// </summary>
public class CustomAreaEffector : MonoBehaviour
{

    [Header("각도")]
    public float angle;

    [Header("힘")]
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
