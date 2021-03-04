using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettings/* : UIView*/
{

    public Slider soundSFXSlider;
    public Slider bgmSlider;
    public Slider brightnessSlider;
    public CanvasGroup canvasGroup;
    //public UISelecter languageSelector;

    private SettingsData settingsData;
    private SettingsData originalSettingsData;
    public void Initialize()
    {

    }

    private void SetData(SettingsData data)
    {
        settingsData = new SettingsData(data);
        originalSettingsData = new SettingsData(data);

        settingsData = data;
        soundSFXSlider.value = GetFloat(this.settingsData.sfxVolume);
        bgmSlider.value = GetFloat(this.settingsData.bgmVolume);
        brightnessSlider.value = GetFloat(this.settingsData.brightness);
        //languageSelector.SetIndex((int)this.settingsData.language);
    }
    public void OnChangeSoundSFXSlider()
    {
        GameManager.Instance.settingsManager.audioMixer.SetFloat("sfxVolume",Mathf.Log(Mathf.Lerp(0.001f,1,soundSFXSlider.value))*20);
    }

    public void OnChangeBGMSlider()
    {
        GameManager.Instance.settingsManager.audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, bgmSlider.value)) * 20);

    }
    public void OnChangeBrightnessSlider()
    {


    }


    public void OnApply()
    {
        canvasGroup.interactable = false;

        ApplySetting(settingsData);

        GameManager.Instance.settingsManager.OnApply(settingsData);
        //Toggle(false);
    }

    public void OnCancel()
    {
        GameManager.Instance.settingsManager.OnApply(originalSettingsData);
        //Toggle(false);

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

    private void ApplySetting(SettingsData data)
    {
        data.sfxVolume = GetString(soundSFXSlider.value);
        data.bgmVolume = GetString(bgmSlider.value);
        data.brightness = GetString(brightnessSlider.value);
        //data.language = (Language)languageSelector.GetCurrentIndex();

    }
    private string GetString(float input)
    {
        return input.ToString();
    }

    private float GetFloat(string input)
    {
        return (float)System.Convert.ToDouble(input);
    }

}
