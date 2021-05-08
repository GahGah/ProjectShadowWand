using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.Instance.GoDie();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
