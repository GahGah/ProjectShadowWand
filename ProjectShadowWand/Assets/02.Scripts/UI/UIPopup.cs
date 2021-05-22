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
        return canvasObject.activeSelf;
    }

    public override bool Close()
    {
        canvasObject.SetActive(false);
        return !canvasObject.activeSelf;
    }


    public void ButtonOpen()
    {
        Open();
        buttonSelector.ForceSelect();
    }

    public void ButtonClose()
    {
        if (buttonSelector.eventSystem.currentSelectedGameObject == buttonSelector.activeButton.gameObject)
        {
            buttonSelector.eventSystem.SetSelectedGameObject(null);
        }
        Close();
    }
}
