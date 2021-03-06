using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMainMenu : UIBase
{

    [Header("게임 시작 텍스트")]
    public TMP_Text gameStartText;
    public UIPopup uiPopup_NewGame;
    public UISettings uiSetting;
    public UIPopup uiPopup_ExitGame;

    public UICutScene cutscene;

    private bool isExistSaveData;

    public override void Init()
    {
        base.Init();
    }
    private void Awake()
    {
        uiSetting.onMainMenu = true;
        uiSetting.uiMainMenu = this;
        uiType = eUItype.MAIN;
    }
    private IEnumerator Start()

    {
        canvasGroup.interactable = false;
        UIManager.Instance.AddToDictionary(this);
        yield return StartCoroutine(SaveLoadManager.Instance.LoadData_Stage());

        if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00")
        {
            gameStartText.text = "시작하기";
        }
        else
        {
            gameStartText.text = "새로하기";
        }

        canvasGroup.interactable = true;
    }
    public void Button_GoNewGame(UIBase _ui)
    {
        if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00") //00스테이지라면
        {
            NotButton_StartNewGame();
        }
        else
        {
            UIManager.Instance.OpenThis(_ui);
        }

    }

    public void NotButton_StartNewGame()
    {
        canvasGroup.interactable = false;
        StartCoroutine(NewGameIntro());
    }

    private IEnumerator NewGameIntro()
    {

        cutscene.StartPlayCutScene();

        while (cutscene.isEnd == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        while (cutscene.isNext == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        SceneChanger.Instance.LoadThisSceneName("Stage_00", true);
       // AudioManager.Instance.Stop_Bgm();
    }


    //게임을 이어서 플레이합니다.
    public void Button_GoContinue(UIBase _ui)
    {

        UIManager.Instance.OpenThis(_ui);

        //if (SaveLoadManager.Instance.currentData_Stage.stageName == "Stage_00")
        //{
        //    canvasGroup.interactable = false;

        //    StartCoroutine(NewGameIntro());
        //    // SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
        //    // AudioManager.Instance.Stop_Bgm();
        //}
        //else
        //{
        //    canvasGroup.interactable = false;
        //    UIManager.Instance.OpenThis(_ui);
        //    SceneChanger.Instance.LoadThisSceneName(SaveLoadManager.Instance.currentData_Stage.stageName, false);
        //}

    }

    //public void Button_OpenSetting(UIBase _ui)
    //{
    //    UIManager.Instance.OpenThis(_ui);
    //    base.Close();
    //}

    //public void Button_CloseSetting(UIBase _ui)
    //{
    //    UIManager.Instance.OpenThis(_ui);
    //}
    //public void Button_OpenExit(UIBase _ui)
    //{
    //    UIManager.Instance.OpenThis(_ui);
    //}

    public void Button_Quit()
    {
        Application.Quit();
    }

    public override bool Open()
    {
        canvasObject.SetActive(true);
        return base.Open();
    }
}
