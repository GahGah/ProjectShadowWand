using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SettingsManager settingsManager;


    public FileManager fileManager;

    public bool isGameStarted;


    private string stageSceneName;
    private int gold;

    public bool isGameOver;
    public bool isAllEnemySpawned;
    public bool isAllEnemyDestroyed;
    public bool isStageClear;

    // 싱글톤 패턴
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject); // 씬 로딩을 할 때(옮겨다닐 때) 지우지마라 
        }

        isGameOver = false;
        isGameStarted = false;
    }

    private void Start()
    {
        StartCoroutine(ProcessStartGame());
    }


    IEnumerator ProcessStartGame()
    {



        yield return StartCoroutine(fileManager.IsExist("Settings.dat"));

        if (fileManager.IsExist_Result)
        {
            yield return StartCoroutine(settingsManager.LoadSettingsData());
        }
        //check if settingsData Exist
        else
        {
            settingsManager.GetDefaultSettingsData();
            yield return StartCoroutine(settingsManager.SaveSettingsData());
            //LocalizationManager.Instance.SetLocalizationLanguage(settingsManager.GetCurrentSettingsData().language);
            //LocalizationManager.Instance.UpdateLocalization();

        }

        yield break;
    }

    public void StartStage(string stageName)
    {

        stageSceneName = stageName;
        StartCoroutine(ProcessStage(stageSceneName));
    }

    private IEnumerator ProcessStage(string stageName)
    {
        AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync(stageName, LoadSceneMode.Single);
        yield return sceneLoadAsync;

        yield break;
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(ProcessReturnToMainMenu());
    }
    /// <summary>
    /// 이름은 넥스트스테이지지만 사실 stage01씬을 재 로드함
    /// </summary>
    public void GoNextStage()
    {
        StartCoroutine(ProcessNextStage());
    }
    private IEnumerator ProcessNextStage()
    {
        AsyncOperation sceneLoadAync = SceneManager.LoadSceneAsync("Stage01", LoadSceneMode.Single);
        yield return sceneLoadAync;
        yield break;
    }
    private IEnumerator ProcessReturnToMainMenu()
    {
        AsyncOperation sceneLoadAync = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        yield return sceneLoadAync;
        yield break;
    }

    public void ExitGame()
    {


        if (Application.isPlaying)
        {
#if UNITY_EDITOR //유니티 에디터 상에서의 종료
            EditorApplication.isPlaying = false;//using 유니티 에디터 필요함.

#else          
            Application.Quit(); 
             //실제 빌드 때 돌아가는 코드. 회색 인 이유는...ㄹㅇ 지금 에디터에서 하고 있잖아 ㅋㅋㅋㅋㅋㅋ
#endif

            // #if UNIT_STANDALONE_WIN || UNITY_SWITCH || UNITY_PS4 이런거 다 있다고 함 ㅅㅂ 개신기;;
            //Application.platform = RuntimePlatform.어떤거... 이것도 있다함 아 시바 진짜 신기하넼ㅋㅋㅋㅋ
        }


    }
}
