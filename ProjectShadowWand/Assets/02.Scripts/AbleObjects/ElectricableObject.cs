using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricableObject : MonoBehaviour,IElectricable
{
    [Tooltip("감전 당했었나?")]
    public bool isShocked; 
    public void OnThunder()
    {
        isShocked = true;   
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
