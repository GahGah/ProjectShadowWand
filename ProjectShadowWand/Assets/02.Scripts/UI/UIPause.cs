using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ΰ����� �Ͻ����� UI
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

        if (isFading)
        {
            return false;
        }
        else
        {
            Time.timeScale = 0f;
            canvasObject.SetActive(true);
            buttonSelector.ForceSelect();
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
            Time.timeScale = 1f;
            buttonSelector.ForceDeSelect(); //���� ���� ����Ʈ���¤��� �ϴ� �ٲ����...�� ����Ʈ��������?
            StartCoroutine(ProcessFadeAlpha_Close());
            return true;
        }
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
