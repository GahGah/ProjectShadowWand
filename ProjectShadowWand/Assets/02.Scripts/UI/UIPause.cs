using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임의 일시정지 UI
/// </summary>
public class UIPause : UIBase
{
    public GameObject pauseGroup;
    private ButtonSelector buttonSelector;
    private void Awake()
    {
        uiType = eUItype.PAUSE;
        buttonSelector = GetComponent<ButtonSelector>();
    }
    private void Start()
    {
        UIManager.Instance.AddToDictionary(this);
        Init();
    }

    public override void Init()
    {

        canvasObject.SetActive(false);
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public override bool Open()
    {
        canvasObject.SetActive(true);
        buttonSelector.ForceSelect();
        Time.timeScale = 0f;
        return true;
    }
    public override bool Close()
    {
        Time.timeScale = 1f;
        canvasObject.SetActive(false);
        buttonSelector.ForceSelect();
        return true;
    }

    public void ButtonSetting(UIBase _ui)
    {
        UIManager.Instance.OpenThis(_ui);
    }
    public void ButtonContinue()
    {
        UIManager.Instance.CloseThis(this);
    }
    public void ButtonRestart()
    {
        SceneChanger.Instance.LoadThisSceneName(StageManager.Instance.nowStageName, false);
        UIManager.Instance.CloseThis(this);
        // StageManager.Instance.UpdateStageName();
    }
    public void ButtonReturnMain()
    {
        SceneChanger.Instance.LoadThisSceneName("Stage_Main", false);
        UIManager.Instance.CloseThis(this);
    }



}
