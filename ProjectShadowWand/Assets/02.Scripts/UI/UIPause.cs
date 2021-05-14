using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임의 일시정지 UI
/// </summary>
public class UIPause : UIBase
{
    public GameObject pauseGroup;
    private void Awake()
    {
        uiType = eUItype.PAUSE;
    }
    private void Start()
    {
        UIManager.Instance.AddToDictionary(this);
        Init();
    }

    public override void Init()
    {

        pauseGroup.SetActive(false);
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public override bool Open()
    {
        pauseGroup.SetActive(true);
        Time.timeScale = 0f;
        return true;
    }
    public override bool Close()
    {
        Time.timeScale = 1f;
        pauseGroup.SetActive(false);
        return true;
    }
    public void ButtonRestart()
    {
        Debug.Log("SceneChange");
        SceneChanger.Instance.LoadThisSceneName(StageManager.Instance.nowStageName, false);
        Close();
        // StageManager.Instance.UpdateStageName();

    }

    public void ButtonReturnMain()
    {

        SceneChanger.Instance.LoadThisSceneName("Stage_Main", false);
        Close();
    }



}
