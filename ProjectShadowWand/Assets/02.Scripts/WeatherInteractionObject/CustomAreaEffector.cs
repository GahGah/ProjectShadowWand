using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaEffector를 흉내낸 것 뿐입니다. 일단 대충 만들어서 기능이 부족함
/// </summary>
public class CustomAreaEffector : MonoBehaviour
{
    public Vector2 dir;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            collision.attachedRigidbody.AddForce(dir, ForceMode2D.Force);
        }

    }
}
