using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{
    public ButtonSelector buttonSelector;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        canvasObject.SetActive(false);
    }

    public override bool Open()
    {

        if (isFading)
        {
            return false;
        }
        else
        {
            canvasObject.SetActive(true);
            buttonSelector.ForceSelect();
            buttonSelector.StaySelect();
            StartCoroutine(ProcessFadeAlpha_Open());
            return true;
        }
    }

    public override bool Close()
    {
        if (isFading)
        {
            return false;
        }
        else
        {
            buttonSelector.StaySelect();
            StartCoroutine(ProcessFadeAlpha_Close());
            return true;
        }

    }


    public void ButtonOpen()
    {
        Open();
        buttonSelector.ForceSelect();
    }

    public void ButtonClose()
    {

        buttonSelector.ForceDeSelect();
        Close();
    }
}
