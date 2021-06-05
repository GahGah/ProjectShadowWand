using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : UIBase
{
    public ButtonSelector buttonSelector;


    public bool onMainMenu;
    public bool onPause;

    [HideInInspector]
    public UIMainMenu uiMainMenu = null;
    [HideInInspector]
    public UIPause uiPause = null;

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

            if (onMainMenu)
            {
                if (uiMainMenu == null)
                {
                    UIBase tempUI;
                    UIManager.Instance.uiDicitonary.TryGetValue(eUItype.MAIN, out tempUI);
                    uiMainMenu = tempUI as UIMainMenu;
                }
                uiMainMenu.Close();
            }
            else if (onPause)
            {
                if (uiPause == null)
                {
                    UIBase tempUI;
                    UIManager.Instance.uiDicitonary.TryGetValue(eUItype.PAUSE, out tempUI);
                    uiPause = tempUI as UIPause;
                }
                uiPause.CloseMenu();
            }


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

            if (onMainMenu)
            {
                if (uiMainMenu == null)
                {
                    UIBase tempUI;
                    UIManager.Instance.uiDicitonary.TryGetValue(eUItype.MAIN, out tempUI);
                    uiMainMenu = tempUI as UIMainMenu;
                }
                uiMainMenu.Open();
            }
            else if (onPause)
            {
                if (uiPause == null)
                {
                    UIBase tempUI;
                    UIManager.Instance.uiDicitonary.TryGetValue(eUItype.PAUSE, out tempUI);
                    uiPause = tempUI as UIPause;
                }
                uiPause.OpenMenu();
            }


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
