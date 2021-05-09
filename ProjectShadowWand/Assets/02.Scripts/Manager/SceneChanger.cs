using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string moveSceneName;

    public Image goBlackImage;

    public Image progressBar;
    public bool isLoading;

    public static SceneChanger Instance;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        // Screen.SetResolution(1920, 1080, true);
    }
    private void Start()
    {

    }
    public void ButtonGoPlayerScene()
    {
        SceneManager.LoadScene(moveSceneName);
    }

    private void Update()
    {

    }
    public void PlayerDie_SceneReload()
    {
        StartCoroutine(ProcessDie());
    }

    public void Test()
    {
        Screen.fullScreen = false;
    }


    /// <summary>
    /// 해당 씬을 로딩합니다.
    /// </summary>
    /// <param name="_sceneName"></param>
    /// <returns></returns>
    public IEnumerator LoadThisScene(string _sceneName)
    {
        isLoading = true;

        SceneManager.sceneLoaded += LoadSceneEnd;

        moveSceneName = _sceneName;

        Time.timeScale = 0f;
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(GoColorScreen(0.5f, 1f, true));


        //비동기로 로드 씬
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        asyncOperation.allowSceneActivation = false;  //씬 활성화를 false로. 이제 로딩이 끝나도 씬이 활성화되지 않음.

        float timer = 0f;
        while (!asyncOperation.isDone) // 로딩이 완료되기 전 까지만
        {
            timer += Time.unscaledDeltaTime;

            if (asyncOperation.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress, timer);

                if (progressBar.fillAmount >= asyncOperation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount >= 1f)
                {
                    asyncOperation.allowSceneActivation = true;
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        yield return null;

        //yield return StartCoroutine(GoColorScreen(0.5f, 1f, false));
        //Debug.Log("됨?");
        //Time.timeScale = 1f;
        //goBlackImage.gameObject.SetActive(false);
        //progressBar.gameObject.SetActive(false);

    }
    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {

        if (scene.name == moveSceneName)
        {
            Debug.Log("End");

            StartCoroutine(SceneChanger.Instance.GoColorScreen(0.5f, 1f, false));

            SceneManager.sceneLoaded -= LoadSceneEnd;

            Time.timeScale = 1f;

            isLoading = false;

        }

    }




    /// <summary>
    /// _waitTime 뒤 서서히, _goingTime까지 화면을 특정 색으로 물들입니다.
    /// </summary>
    /// <param name="_waitTime">변화 전 기다리는 시간입니다.</param>
    /// <param name="_goingTime">변화하는 시간입니다.</param>
    /// <param name="_goBlack">true라면 화면이 까매지고, false라면 화면이 투명해집니다.</param>
    /// <returns></returns>
    public IEnumerator GoColorScreen(float _waitTime, float _goingTime, bool _goBlack)
    {
        var timer = 0f;
        var progress = 0f;
        var colorTwoMyeong = new Color32(0, 0, 0, 0);
        var colorBlack = new Color32(0, 0, 0, 255);

        var colorWhite = Color.white;
        Color32 goingColor;
        Color32 startColor;
        Color32 progressStartColor;
        Color32 progressGoingColor;
        if (_goBlack) //씬 로드를 해야하는 상황일 때
        {
            startColor = colorTwoMyeong;
            goingColor = colorBlack;
            progressStartColor = colorTwoMyeong;
            progressGoingColor = colorWhite;
        }
        else //씬 로드 완료일때
        {
            startColor = colorBlack;
            goingColor = colorTwoMyeong;
            progressStartColor = colorWhite;
            progressGoingColor = colorTwoMyeong;
        }

        yield return new WaitForSecondsRealtime(_waitTime);

        goBlackImage.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / _goingTime;

            goBlackImage.color = Color32.Lerp(startColor, goingColor, progress);
            progressBar.color = Color32.Lerp(progressStartColor, progressGoingColor, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        if (_goBlack == false)
        {
            goBlackImage.gameObject.SetActive(false);
            progressBar.gameObject.SetActive(false);
            isLoading = false;

        }
    }

    IEnumerator ProcessDie()
    {
        var timer = 0f;
        var maxTime = 2f;
        var progress = 0f;
        var twoMyeong = new Color32(0, 0, 0, 0);
        var black = new Color32(0, 0, 0, 255);

        yield return new WaitForSecondsRealtime(2f);

        goBlackImage.gameObject.SetActive(true);
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / maxTime;
            Debug.Log("Progress : " + progress);

            goBlackImage.color = Color32.Lerp(twoMyeong, black, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        SceneManager.LoadScene(moveSceneName);

    }
}
