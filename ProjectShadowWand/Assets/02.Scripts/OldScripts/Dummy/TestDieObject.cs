using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDieObject : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.isDie = true;
        }
    }
}