using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{

    private void Start()
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
        return gameObject.activeSelf;
    }

    public override bool Close()
    {
        SetActive(false);
        return !gameObject.activeSelf;
    }
}
