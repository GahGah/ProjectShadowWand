using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private SettingsData currentSettingsData; //현재 세팅 데이터.

    public void OnApply(SettingsData data)
    {
        //세팅을 확정하고...
        ApplySettings(data);
        StartCoroutine(SaveSettingsData());
        //ㄹㅇ 저장시킨다
    }
    public void OnCancel(SettingsData data)
    {
        ApplySettings(data);
        //확정은 아는데 저장은 안함ㅋㅋ
    }


    private void ApplySettings(SettingsData data)
    {

        GameManager.Instance.settingsManager.audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.bgmVolume))) * 20);
        GameManager.Instance.settingsManager.audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1, (float)System.Convert.ToDouble(data.sfxVolume))) * 20);

        //LocalizationManager.Instance.SetLocalizationLanguage(data.language);
        //LocalizationManager.Instance.UpdateLocalization();
        currentSettingsData = new SettingsData(data);
    }

    public IEnumerator SaveSettingsData()
    {
        string dataString = JsonUtility.ToJson(currentSettingsData, true); //true로 하면 제대로...그...띄어쓰기? 가 됨.

        yield return StartCoroutine(GameManager.Instance.fileManager.WriteText("Settings.dat", dataString));

        yield break;
    }

    public IEnumerator LoadSettingsData()
    {
        yield return StartCoroutine(GameManager.Instance.fileManager.ReadText("Settings.dat"));
        if (!string.IsNullOrEmpty(GameManager.Instance.fileManager.ReadText_Result))
        {
            var loadedSettingsData = JsonUtility.FromJson<SettingsData>(GameManager.Instance.fileManager.ReadText_Result);
            ApplySettings(loadedSettingsData);
        }
    }
    public SettingsData GetCurrentSettingsData()
    {
        if (currentSettingsData == null)
        {
            GetDefaultSettingsData();
        }
        return currentSettingsData;
    }

    public SettingsData GetDefaultSettingsData()
    {
        currentSettingsData = new SettingsData();
        return currentSettingsData;
    }
}
