using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlackMask : MonoBehaviour
{

    public GameObject blackImage;


    private void Start()
    {
        blackImage.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blackImage.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            blackImage.SetActive(false);
        }

    }
}
