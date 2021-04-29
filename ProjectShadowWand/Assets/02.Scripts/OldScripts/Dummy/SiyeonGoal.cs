using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiyeonGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SiyeonManager.Instance.SetActiveTrueGoalUI();
        }
    }
}
