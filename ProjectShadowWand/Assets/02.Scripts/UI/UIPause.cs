using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임의 일시정지 UI
/// </summary>
public class UIPause : UIBase
{
    public CanvasGroup menuGroup;
    private ButtonSelector buttonSelector;

    [Header("세팅 UI")]
    public UISettings uiSetting;
    private void Awake()
    {
        uiType = eUItype.PAUSE;
        UIManager.Instance.AddToDictionary(this);
        buttonSelector = GetComponent<ButtonSelector>();

        uiSetting.onPause = true;
        uiSetting.uiPause = this;
    }
    private void Start()
    {
        UIManager.Instance.AddToDictionary(this);
        Init();
    }

    public override void Init()
    {
        base.Init();
        canvasObject.SetActive(false);
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    private float audioTime = 0f;
    public override bool Open()
    {

        if (isFading)
        {
            return false;
        }
        else
        {
            menuGroup.alpha = 1f;
            Time.timeScale = 0f;

            canvasObject.SetActive(true);
            menuGroup.interactable = true;
            audioTime = AudioManager.Instance.audioSource_bgm.time;
            AudioManager.Instance.audioSource_bgm.Stop();

            AudioManager.Instance.Play_UI_Pause_On();
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
            menuGroup.alpha = 1f;
            buttonSelector.ForceDeSelect(); //원래 포스 셀렉트였는ㄴ데 일단 바꿔봤음...왜 셀렉트였던거지?

            AudioManager.Instance.audioSource_bgm.volume = 0f;
            AudioManager.Instance.audioSource_bgm.Play();
            AudioManager.Instance.audioSource_bgm.time = audioTime;
            AudioManager.Instance.audioSource_bgm.volume = 1f;
            StartCoroutine(ProcessFadeAlpha_Close());
            return true;
        }
    }


    public void OpenMenu()
    {
        menuGroup.gameObject.SetActive(true);
        canvasGroup.interactable = true;
        StartCoroutine(FadeAlphaMenuGroup(0f, 1f, alphaFadeTime));
    }

    public void CloseMenu()
    {
        //  StartCoroutine(FadeAlphaMenuGroup(1f, 0f, fadeTime));
        menuGroup.interactable = true;
        menuGroup.gameObject.SetActive(false);

    }

    private IEnumerator FadeAlphaMenuGroup(float _start, float _end, float _time)
    {
        isFading = true;

        float timer = 0f;

        float progress = 0f;

        menuGroup.alpha = _start;

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;

            progress = timer / _time;

            menuGroup.alpha = Mathf.Lerp(_start, _end, progress);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        menuGroup.alpha = _end;
        isFading = false;
    }




    public void ButtonSetting()
    {
        menuGroup.interactable = false;
        UIManager.Instance.OpenThis(uiSetting);
    }
    public void ButtonContinue()
    {
        menuGroup.interactable = false;
        UIManager.Instance.CloseThis(this);
    }
    public void ButtonRestart(UIBase _ui)
    {
        menuGroup.interactable = false;
        UIManager.Instance.CloseThis(_ui);
        UIManager.Instance.CloseThis(this);
        SceneChanger.Instance.LoadThisSceneName(StageManager.Instance.nowStageName, false);

        // StageManager.Instance.UpdateStageName();
    }
    public void ButtonReturnMain(UIBase _ui)
    {
        menuGroup.interactable = false;
        UIManager.Instance.CloseThis(_ui);
        UIManager.Instance.CloseThis(this);
        SceneChanger.Instance.LoadThisSceneName("Stage_Main", false);

    }

    public void ButtonClose()
    {
        UIManager.Instance.CloseTop();
    }


}
