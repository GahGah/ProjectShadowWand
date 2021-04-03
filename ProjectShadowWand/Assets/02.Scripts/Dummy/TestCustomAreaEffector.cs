using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCustomAreaEffector : MonoBehaviour
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
