using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public UIPopup newGamePopup;

    private void Start()
    {
        canvasGroup.interactable = true;
    }
    public void Button_GoNewGame(UIBase _ui)
    {
        _ui.Open();
    }

    public void Button_StartNewGame()
    {
        canvasGroup.interactable = false;
        SceneChanger.Instance.LoadThisSceneName("Stage_00", false);
    }
    public void Button_GoContinue(UIBase _ui)
    {
        //_ui.Open();
        if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00")
        {
            canvasGroup.interactable = false;
            SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
        }
        else
        {
            canvasGroup.interactable = false;
            SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
        }
    }

    public void Button_OpenSetting(UIBase _ui)
    {
        _ui.Open();
    }
    public void Button_OpenExit(UIBase _ui)
    {
        _ui.Open();
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

}
