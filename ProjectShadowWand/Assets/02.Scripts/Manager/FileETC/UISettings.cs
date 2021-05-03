using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class UISettings : UIBase
{
    [Header("소리")]
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider soundEffectSlider;
    public Slider bgmSlider;


    public Slider brightnessSlider;
    public CanvasGroup canvasGroup;
    //public UISelecter languageSelector;

    private Data_Settings currentSettingsData;
    // private Data_Settings settingsData;
    private Data_Settings originalSettingsData;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {

        SetActive(false);
    }

    public override bool Open()
    {
        SetActive(true);
        SetData(SaveLoadManager.Instance.currentData_Settings);
        return gameObject.activeSelf;
    }

    public override bool Close()
    {
        SetActive(false);
        return !gameObject.activeSelf;
    }
    private void SetData(Data_Settings data)
    {
        currentSettingsData = new Data_Settings(data);
        originalSettingsData = new Data_Settings(data);

        currentSettingsData = data;
        masterSlider.value = GetFloat(this.currentSettingsData.masterVolume);
        soundEffectSlider.value = GetFloat(this.currentSettingsData.sfxVolume);
        bgmSlider.value = GetFloat(this.currentSettingsData.bgmVolume);
        brightnessSlider.value = GetFloat(this.currentSettingsData.brightness);
        //languageSelector.SetIndex((int)this.settingsData.language);
    }
    public void SliderOnChangeMasterSlider()
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, masterSlider.value)) * 20);
    }
    public void SliderOnChangeSoundEffectSlider()
    {
        // GameManager.Instance.settingsManager.
        audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, soundEffectSlider.value)) * 20);
    }

    public void SliderOnChangeBGMSlider()
    {
        //  GameManager.Instance.settingsManager.
        audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, bgmSlider.value)) * 20);

    }
    public void SliderOnChangeBrightnessSlider()
    {


    }


    public void ButtonOnApply()
    {
        canvasGroup.interactable = false;

        ApplySetting(currentSettingsData);

        // GameManager.Instance.settingsManager.
        OnApply(currentSettingsData);
        //Toggle(false);
    }

    public void ButtonOnCancel()
    {
        // GameManager.Instance.settingsManager.
        OnApply(originalSettingsData);
        //Toggle(false);

    }

    private void ApplySetting(Data_Settings data)
    {
        data.masterVolume = GetString(masterSlider.value);
        data.sfxVolume = GetString(soundEffectSlider.value);
        data.bgmVolume = GetString(bgmSlider.value);
        data.brightness = GetString(brightnessSlider.value);

        //TODO :
        //data.isFullScreenMode = GetString()

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
    /// 세팅데이터를 정말로 저장합니다. 
    /// </summary>
    /// <param name="data"></param>
    public void OnApply(Data_Settings data)
    {
        //세팅을 확정하고...
        ApplySettings(data);
        if (data != originalSettingsData)
        {
            originalSettingsData = new Data_Settings(data);
            SaveLoadManager.Instance.SetCurrentData_Settings(data);
            StartCoroutine(SaveLoadManager.Instance.SaveData_Settings());
        }

        //StartCoroutine(SaveSettingsData());
        //ㄹㅇ 저장시킨다
    }

    /// <summary>
    /// 현재 데이터를 변경된 세팅 데이터로 변경합니다.
    /// </summary>
    /// <param name="data">변경된 세팅 데이터</param>
    private void ApplySettings(Data_Settings data)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.masterVolume))) * 20);
        // GameManager.Instance.settingsManager.
        audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.bgmVolume))) * 20);
        //   GameManager.Instance.settingsManager.
        audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.sfxVolume))) * 20);

        //LocalizationManager.Instance.SetLocalizationLanguage(data.language);
        //LocalizationManager.Instance.UpdateLocalization();
        currentSettingsData = new Data_Settings(data);
    }

    public IEnumerator SaveSettingsData()
    {
        //string dataString = JsonUtility.ToJson(currentSettingsData, true); //true로 하면 제대로...그...띄어쓰기? 가 됨.

        //yield return StartCoroutine(GameManager.Instance.fileManager.WriteText("Settings.dat", dataString));

        yield break;
    }

    public IEnumerator LoadSettingsData()
    {
        //yield return StartCoroutine(GameManager.Instance.fileManager.ReadText("Settings.dat"));
        //if (!string.IsNullOrEmpty(GameManager.Instance.fileManager.readText_Result))
        //{
        //    var loadedSettingsData = JsonUtility.FromJson<SettingsData>(GameManager.Instance.fileManager.readText_Result);
        //    ApplySettings(loadedSettingsData);
        //}
        yield break;
    }

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
