﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
public class UISettings : UIBase
{
    [Header("오디오 믹서")]
    public AudioMixer audioMixer;


    [Header("소리 슬라이더")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;


    [Header("음소거 버튼")]
    public Button masterMute;
    public Button bgmMute;
    public Button sfxMute;

    [Header("소리 퍼센트 텍스트")]
    public TMP_Text masterPer;
    public TMP_Text bgmPer;
    public TMP_Text sfxPer;

    [Header("화면모드 토글")]
    public Toggle fullScreenToggle;
    public Toggle windowScreenToggle;



    [Header("음소거 버튼 이미지")]
    public Sprite soundMuteImage;
    public Sprite soundOnImage;

    public Slider brightnessSlider;




    public CanvasGroup canvasGroup;
    //public UISelecter languageSelector;

    //  public ToggleGroup screenModeGroup;


    private Data_Settings currentSettingsData;
    // private Data_Settings settingsData;
    private Data_Settings originalSettingsData;


    private ButtonSelector buttonSelector;
    private IEnumerator Start()
    {
        yield return StartCoroutine(SaveLoadManager.Instance.LoadData_Settings());

        SetData(SaveLoadManager.Instance.currentData_Settings);

        masterSlider.onValueChanged.AddListener(delegate { SliderOnChangeMasterSlider(); });
        bgmSlider.onValueChanged.AddListener(delegate { SliderOnChangeBGMSlider(); });
        sfxSlider.onValueChanged.AddListener(delegate { SliderOnChangeSfxSlider(); });


        Init();
        yield break;

    }


    public override void Init()
    {
        canvasObject.SetActive(false);
        buttonSelector = GetComponent<ButtonSelector>();
    }

    public override bool Open()
    {
        canvasObject.SetActive(true);

        buttonSelector.ForceSelect();
        return gameObject.activeSelf;
    }

    public override bool Close()
    {
        canvasObject.SetActive(false);

        buttonSelector.ForceSelect();
        return !gameObject.activeSelf;
    }
    private void SetData(Data_Settings data)
    {
        currentSettingsData = new Data_Settings(data);
        originalSettingsData = new Data_Settings(data);

        currentSettingsData = data;

        UpdateValue(data);

        ApplySettings(data);

        //TODO :
        //isFullScreen,  resolution...
        //languageSelector.SetIndex((int)this.settingsData.language);
    }
    public void SliderOnChangeMasterSlider()
    {
        Debug.Log("Change!!");
        audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, masterSlider.value)) * 20);
        masterPer.text = ChangePerText(masterSlider.value);
    }
    public void SliderOnChangeBGMSlider()
    {
        //  GameManager.Instance.settingsManager.
        audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, bgmSlider.value)) * 20);
        bgmPer.text = ChangePerText(bgmSlider.value);

    }

    public void SliderOnChangeSfxSlider()
    {
        // GameManager.Instance.settingsManager.
        audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, sfxSlider.value)) * 20);
        sfxPer.text = ChangePerText(sfxSlider.value);
    }


    public void SliderOnChangeBrightnessSlider()
    {


    }

    //public void ToggleScreen_WindowMode()
    //{
    //    currentSettingsData.isFullScreenMode = false;
    //}
    //public void ToggleScreen_FullScreenMode()
    //{

    //    currentSettingsData.isFullScreenMode = true;
    //}

    public void ButtonOnApply()
    {
        canvasGroup.interactable = false;

        ApplySetting(currentSettingsData);

        // GameManager.Instance.settingsManager.
        OnApply(currentSettingsData);
        canvasGroup.interactable = true;
        //Toggle(false);
    }

    public void ButtonOnCancel()
    {
        // GameManager.Instance.settingsManager.
        //OnApply(originalSettingsData);

        UpdateValue(originalSettingsData);
        ApplySettings(originalSettingsData);
        //Toggle(false);

    }

    /// <summary>
    /// 기본값으로 초기화하는 버튼입니다.
    /// </summary>
    public void ButtonOnDefault()
    {
        canvasGroup.interactable = false;

        // GameManager.Instance.settingsManager.
        OnApply(GetDefaultSettingsData());
        canvasGroup.interactable = true;
    }


    public void ButtonOnToggleMute_Master()
    {
        masterSlider.interactable = !masterSlider.interactable;
        UpdateMuteButtonImage(masterMute, masterSlider);

        if (masterSlider.interactable)
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(masterSlider.value))) * 20);
        }
        else
        {

            audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);
        }



    }
    public void ButtonOnToggleMute_Bgm()
    {
        bgmSlider.interactable = !bgmSlider.interactable;
        UpdateMuteButtonImage(bgmMute, bgmSlider);

        if (bgmSlider.interactable)
        {
            audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(bgmSlider.value))) * 20);
        }
        else
        {
            audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);
        }


    }
    public void ButtonOnToggleMute_Sfx()
    {
        sfxSlider.interactable = !sfxSlider.interactable;
        UpdateMuteButtonImage(sfxMute, sfxSlider);

        if (sfxSlider.interactable)
        {
            audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(sfxSlider.value))) * 20);
        }
        else
        {
            audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);

        }
    }



    /// <summary>
    /// 음소거 버튼의 모양을 바꿉니다.
    /// </summary>
    public void UpdateMuteButtonImage(Button _button, Slider _slider)
    {
        var fillImage =_slider.fillRect.gameObject.GetComponent<Image>();
        if (_slider.interactable)
        {
            _button.image.sprite = soundOnImage;
            fillImage.color = Color.white;
        }
        else
        {
            _button.image.sprite = soundMuteImage;
            fillImage.color = Color.gray;
        }
    }
    public void UpdateMuteButtonImage(Button _button, Slider _slider, bool _isMute)
    {
        var fillImage = _slider.fillRect.gameObject.GetComponent<Image>();

        if (_isMute == false)
        {
            _button.image.sprite = soundOnImage;
            fillImage.color = Color.white;
        }
        else
        {
            _button.image.sprite = soundMuteImage;
            fillImage.color = Color.gray;
        }
    }



    private string ChangePerText(float _slideValue)
    {
        return GetString(Mathf.Round(_slideValue * 100f));
    }
    /// <summary>
    /// UI에 보여지는 값들을 변경시킵니다.
    /// </summary>
    /// <param name="_data"></param>
    private void UpdateValue(Data_Settings _data)
    {
        masterSlider.value = GetFloat(_data.masterVolume);
        bgmSlider.value = GetFloat(_data.bgmVolume);
        sfxSlider.value = GetFloat(_data.sfxVolume);

        UpdateMuteButtonImage(masterMute, masterSlider, _data.muteMasterVolume);
        UpdateMuteButtonImage(bgmMute, bgmSlider, _data.muteBgmVolume);
        UpdateMuteButtonImage(sfxMute, sfxSlider, _data.muteSfxVolume);

        masterPer.text = ChangePerText(masterSlider.value);
        bgmPer.text = ChangePerText(bgmSlider.value);
        sfxPer.text = ChangePerText(sfxSlider.value);

        if (_data.isFullScreenMode == true)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            windowScreenToggle.isOn = true;
        }

        //  brightnessSlider.value = GetFloat(_data.brightness);

    }

    /// <summary>
    /// UI에 보여지고 있는 값들을 data로 옮깁니다.
    /// </summary>
    /// <param name="data"></param>
    private void ApplySetting(Data_Settings data)
    {
        data.masterVolume = GetString(masterSlider.value);
        data.sfxVolume = GetString(sfxSlider.value);
        data.bgmVolume = GetString(bgmSlider.value);
        // data.brightness = GetString(brightnessSlider.value);

        data.muteMasterVolume = !masterSlider.interactable;
        data.muteBgmVolume = !bgmSlider.interactable;
        data.muteSfxVolume = !sfxSlider.interactable;

        if (fullScreenToggle.isOn == true)
        {
            data.isFullScreenMode = true;
        }
        else
        {
            data.isFullScreenMode = false;
        }

        //data.language = (Language)languageSelector.GetCurrentIndex();

    }



    private string GetString(float input)
    {
        return input.ToString();
    }
    private string GetString(bool input)
    {
        return input.ToString();
    }

    private float GetFloat(string input)
    {
        return (float)System.Convert.ToDouble(input);
    }

    //여기서부터 세팅 매니저에서 가져옴
    /// <summary>
    /// 세팅데이터를 정말로, SaveLoadManager를 통해 저장합니다. 
    /// </summary>
    /// <param name="data"></param>
    public void OnApply(Data_Settings data)
    {
        //세팅을 확정하고...
        ApplySettings(data);
        if (data != originalSettingsData) // 오리지널 데이터와 가져온 데이터가 다를 경우에만 저장
        {//오리지널 데이터와 가져온 데이터가 같다는 것은, 그냥 변경점이 없다는 것이기 때문에...
            originalSettingsData = new Data_Settings(data);
            SaveLoadManager.Instance.SetCurrentData_Settings(data);
            StartCoroutine(SaveLoadManager.Instance.SaveData_Settings());
        }

        UpdateValue(data);

        //StartCoroutine(SaveSettingsData());
        //ㄹㅇ 저장시킨다
    }

    /// <summary>
    /// data를 오디오 믹서 등 실제 설정에 반역하고, currentData를  data로 변경합니다.
    /// </summary>
    /// <param name="data">변경할 세팅 데이터</param>
    private void ApplySettings(Data_Settings data)
    {
        if (data.muteMasterVolume == false)
        {
            audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.masterVolume))) * 20);
        }
        else
        {

            audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);
        }

        if (data.muteBgmVolume == false)
        {
            audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.bgmVolume))) * 20);
        }
        else
        {
            audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);
        }

        if (data.muteSfxVolume == false)
        {
            audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.sfxVolume))) * 20);
        }
        else
        {
            audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(0f))) * 20);

        }
        // GameManager.Instance.settingsManager.

        //   GameManager.Instance.settingsManager.



        UpdateFullScreen(data.isFullScreenMode);


        //LocalizationManager.Instance.SetLocalizationLanguage(data.language);
        //LocalizationManager.Instance.UpdateLocalization();
        currentSettingsData = new Data_Settings(data);
    }

    public void UpdateFullScreen(bool _b)
    {
        Screen.fullScreen = _b;

        if (_b == true)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            windowScreenToggle.isOn = true;
        }
    }

    //public IEnumerator SaveSettingsData()
    //{
    //    //string dataString = JsonUtility.ToJson(currentSettingsData, true); //true로 하면 제대로...그...띄어쓰기? 가 됨.

    //    //yield return StartCoroutine(GameManager.Instance.fileManager.WriteText("Settings.dat", dataString));

    //    yield break;
    //}

    //public IEnumerator LoadSettingsData()
    //{
    //    //yield return StartCoroutine(GameManager.Instance.fileManager.ReadText("Settings.dat"));
    //    //if (!string.IsNullOrEmpty(GameManager.Instance.fileManager.readText_Result))
    //    //{
    //    //    var loadedSettingsData = JsonUtility.FromJson<SettingsData>(GameManager.Instance.fileManager.readText_Result);
    //    //    ApplySettings(loadedSettingsData);
    //    //}
    //    yield break;
    //}

    /// <summary>
    /// 현재 세팅 데이터를 반환합니다. 현재 세팅 데이터가 null일 경우, 기본 세팅 데이터를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public Data_Settings GetCurrentSettingsData()
    {
        if (currentSettingsData == null)
        {
            GetDefaultSettingsData();
        }
        return currentSettingsData;
    }

    /// <summary>
    /// 기본 세팅 데이터를 만들어서 반환합니다.
    /// </summary>
    /// <returns></returns>
    public Data_Settings GetDefaultSettingsData()
    {
        currentSettingsData = new Data_Settings();
        return currentSettingsData;
    }
    //public override void Toggle(bool value)
    //{
    //    if (value)
    //    {
    //        canvasGroup.interactable = false;
    //        SettingsData data = GameManager.Instance.settingsManager.GetCurrentSettingsData();
    //        SetData(data);
    //        canvasGroup.interactable = true;

    //    }
    //    base.Toggle(value);
    //}
}
