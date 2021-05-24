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
        canvasObject.SetActive(false);
    }

    public override bool Open()
    {

        canvasObject.SetActive(true);

        buttonSelector.StaySelect();
        buttonSelector.ForceSelect();
        return true;
    }

    public override bool Close()
    {

        buttonSelector.StaySelect();
        canvasObject.SetActive(false);

        return true;
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
