using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricableWorkObject : MonoBehaviour, IElectricable
{
    [Tooltip("���� ���߾���?")]
    public bool isShocked;

    public void OnLightining()
    {
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Thunder"))
    //    {
    //        isShocked = true;
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Thunder"))
    //    {
    //        isShocked = true;
    //    }

    //}
}
