using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestWindow : UIBase
{
    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        SetActive(false);
    }

    public override bool Open()
    {
        SetActive(true);
        return true;
    }


    public override bool Close()
    {
        SetActive(false);
        return true;
    }

}