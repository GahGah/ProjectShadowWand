using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterObject : MonoBehaviour//, IPushable
{



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
    }
}

