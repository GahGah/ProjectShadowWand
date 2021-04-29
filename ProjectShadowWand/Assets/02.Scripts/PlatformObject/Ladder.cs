using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : WeatherInteractionObject
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.SetIsLadder(true, gameObject.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.SetIsLadder(false, gameObject.transform.position);
        }
    }
}
