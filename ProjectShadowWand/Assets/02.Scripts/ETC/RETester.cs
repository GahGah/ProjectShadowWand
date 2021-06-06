using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RETester : MonoBehaviour
{

    public TMP_Text test;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    Resolution testR;
    // Update is called once per frame
    void Update()
    {
        testR = Screen.currentResolution;
        test.text = testR.width.ToString();
    }
}
