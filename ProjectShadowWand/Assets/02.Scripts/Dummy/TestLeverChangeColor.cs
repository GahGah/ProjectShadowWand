using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLeverChangeColor : MonoBehaviour
{

    private SpriteRenderer sr;
    private Lever lever;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        lever = GetComponent<Lever>();
    }

    private void Update()
    {

        if (lever.isOn)
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = Color.red;

        }

    }


}
