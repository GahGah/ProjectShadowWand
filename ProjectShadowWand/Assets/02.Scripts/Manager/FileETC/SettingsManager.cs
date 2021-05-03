using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private Data_Settings currentSettingsData; //현재 세팅 데이터.

    public void OnApply(Data_Settings data)
    {
        //세팅을 확정하고...
        ApplySettings(data);
        StartCoroutine(SaveSettingsData());
        //ㄹㅇ 저장시킨다
    }
    public void OnCancel(Data_Settings data)
    {
        ApplySettings(data);
        //확정은 아는데 저장은 안함ㅋㅋ
        //그 이유는 게임매니저에 있다.
    }


    private void ApplySettings(Data_Settings data)
    {

        GameManager.Instance.settingsManager.audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.bgmVolume))) * 20);
        GameManager.Instance.settingsManager.audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.sfxVolume))) * 20);

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
    public Data_Settings GetCurrentSettingsData()
    {
        if (currentSettingsData == null)
        {
            GetDefaultSettingsData();
        }
        return currentSettingsData;
    }

    public Data_Settings GetDefaultSettingsData()
    {
        currentSettingsData = new Data_Settings();
        return currentSettingsData;
    }
}
