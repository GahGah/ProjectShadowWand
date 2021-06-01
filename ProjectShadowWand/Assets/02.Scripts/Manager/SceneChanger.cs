using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string moveSceneName;

    public CanvasGroup canvasGroup;
    public TMP_Text test;
    public Image goBlackImage;

    public Image progressBar;
    public bool isLoading;

    public string nowSceneName;

    [Header("툴팁 텍스트")]
    public TMP_Text tooltipText;

    [Header("회전하는 로딩 이미지")]
    public RotateThis rotateImage;

    //[Tooltip("프로그레스 이미지를 넣어라!! ")]
    //public Image[] progressImages;



    [Space(20)]
    [Header("[TBC]")]
    public CanvasGroup tbc_canvasGroup;
    public RotateThis tbc_rotateImage;
    public Image tbc_blackImage;

    private static SceneChanger instance;
    public static SceneChanger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneChanger>();
            }
            return instance;
        }
    }

    public float waitTime;
    public float fadeTime;
    private void Awake()
    {
        nowSceneName = UpdateStageName();
    }
    private void OnEnable()
    {
        if (Instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(Instance.gameObject);
        }

    }

    //protected void Awake()
    //{

    //    //if (Instance == this)
    //    //{
    //    //    DontDestroyOnLoad(this.gameObject);
    //    //}

    //    // Screen.SetResolution(1920, 1080, true);

    //}
    public string UpdateStageName()
    {
        return SceneManager.GetActiveScene().name;
    }
    //private void Start()
    //{
    //}
    public void ButtonGoPlayerScene()
    {
        SceneManager.LoadScene(moveSceneName);
    }

    //private void Update()
    //{

    //}

    public void Test()
    {
        Screen.fullScreen = false;
    }


    public void LoadThisScene_Start()
    {
        StartCoroutine(LoadThisScene("Stage_00", true));
    }
    /// <summary>
    /// 해당 씬을 로딩합니다.
    /// </summary>
    /// <param name="_sceneName">로딩할 씬 이름</param>
    /// <param name="_doSave">저장할 것인가?</param>
    /// <returns></returns>
    public void LoadThisSceneName(string _sceneName, bool _doSave)
    {
        if (isLoading)
        {
            return;
        }
        tooltipText.text = RandomTooltipText();
        StartCoroutine(LoadThisScene(_sceneName, _doSave));
    }


    private string _TOOLTIP_CODE = "TOOLTIP_CODE";
    private string _TOOLTIP_NAEYONG = "TOOLTIP_NAEYONG";
    private string RandomTooltipText()
    {
        int count = SaveLoadManager.Instance.tooltipData.Count;
        int randomCode = Random.Range(0, count);

        return SaveLoadManager.Instance.tooltipData[randomCode][_TOOLTIP_NAEYONG] as string;
    }

    /// <summary>
    /// 해당 씬을 로딩합니다.
    /// </summary>
    /// <param name="_sceneName">로딩할 씬 이름</param>
    /// <param name="_doSave">저장할 것인가?</param>
    /// <returns></returns>

    private void LoadSceneEnd_StartCoroutine(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(SceneChanger.Instance.LoadSceneEnd_Coroutine(scene, loadSceneMode));
    }
    private IEnumerator LoadSceneEnd_Coroutine(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == moveSceneName)
        {
            Debug.Log("LoadSceneEnd");
            waitTime = 0.5f;
            fadeTime = 1f;

            if (Instance == null)
            {
                Debug.Log("인스턴스가 null");
                instance = FindObjectOfType<SceneChanger>();

                if (instance == null)
                {
                    Debug.Log("찾지못함.");
                }
            }

            yield return new WaitUntil(() => Instance != null);
            StartCoroutine(SceneChanger.Instance.GoColorScreen(waitTime, fadeTime, false));

            SceneManager.sceneLoaded -= LoadSceneEnd_StartCoroutine;

            Time.timeScale = 1f;

            isLoading = false;
        }

    }

    public void LoadScene_Die()
    {
        StartCoroutine(LoadScene_ProcessDie());
    }

    public IEnumerator LoadThisScene(string _sceneName, bool _doSave)
    {
        isLoading = true;

        // SceneManager.sceneLoaded += LoadSceneEnd;

        moveSceneName = _sceneName;

        Time.timeScale = 0f;

        progressBar.fillAmount = 0f;
        waitTime = 0.5f;
        fadeTime = 1.5f;
        AudioManager.Instance.Stop_Bgm();
        rotateImage.gameObject.SetActive(false);
        yield return StartCoroutine(GoColorScreen(waitTime, fadeTime, true));

        rotateImage.gameObject.SetActive(true);
        rotateImage.Init();
        StartCoroutine(rotateImage.ProcessRotate());

        if (_doSave == true)
        {
            if (SaveLoadManager.Instance != null)
            {
                //이동할 스테이지를 저장하고
                SaveLoadManager.Instance.SetCurrentData_Stage(new Data_Stage(moveSceneName));

                yield return StartCoroutine(SaveLoadManager.Instance.SaveData_Stage());

            }

        }
        //비동기로 로드 씬
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        asyncOperation.allowSceneActivation = false;  //씬 활성화를 false로. 이제 로딩이 끝나도 씬이 활성화되지 않음.

        SceneManager.sceneLoaded += LoadSceneEnd;

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
                    break;
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        Debug.Log("SceneLoad");

        float tempTimer = 0f;
        while (tempTimer < 2f)
        {
            tempTimer += Time.unscaledDeltaTime;
            yield return null;
        }
        rotateImage.isStop = true;
        rotateImage.gameObject.SetActive(false);

        asyncOperation.allowSceneActivation = true;

        yield break;
        //yield return StartCoroutine(GoColorScreen(0.5f, 1f, false));
        //Debug.Log("됨?");
        //Time.timeScale = 1f;
        //goBlackImage.gameObject.SetActive(false);
        //progressBar.gameObject.SetActive(false);

    }


    //private void SetFillAmount(float _ )

    /// <summary>
    /// 플레이어가 죽을 때 
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadScene_ProcessDie()
    {
        isLoading = true;
        rotateImage.gameObject.SetActive(false);

        // SceneManager.sceneLoaded += LoadSceneEnd;

        moveSceneName = StageManager.Instance.nowStageName;

        progressBar.fillAmount = 0f;
        waitTime = 2f;
        fadeTime = 2f;

        AudioManager.Instance.Stop_Bgm();

        yield return StartCoroutine(GoColorScreen(waitTime, fadeTime, true));
        Time.timeScale = 0f;

        //비동기로 로드 씬
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(moveSceneName);
        asyncOperation.allowSceneActivation = false;  //씬 활성화를 false로. 이제 로딩이 끝나도 씬이 활성화되지 않음.

        SceneManager.sceneLoaded += LoadSceneEnd;
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

        Debug.Log("SceneLoad");
        yield break;

    }


    private IEnumerator LoadScene_ToBeContinue()
    {
        isLoading = true;

        // SceneManager.sceneLoaded += LoadSceneEnd;

        moveSceneName = "Scene_Credit";

        Time.timeScale = 0f;

        progressBar.fillAmount = 0f;
        waitTime = 0.5f;
        fadeTime = 1.5f;
        AudioManager.Instance.Stop_Bgm();
        rotateImage.gameObject.SetActive(false);
        yield return StartCoroutine(GoColorScreen(waitTime, fadeTime, true));

        rotateImage.gameObject.SetActive(true);
        rotateImage.Init();
        StartCoroutine(rotateImage.ProcessRotate());


        //비동기로 로드 씬
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(moveSceneName);
        asyncOperation.allowSceneActivation = false;  //씬 활성화를 false로. 이제 로딩이 끝나도 씬이 활성화되지 않음.

        SceneManager.sceneLoaded += LoadSceneEnd;

        float timer = 0f;
        while (!asyncOperation.isDone) // 로딩이 완료되기 전 까지만
        {
            timer += Time.unscaledDeltaTime;

            if (asyncOperation.progress < 0.9f)
            {

            }
            else
            {
                asyncOperation.allowSceneActivation = true;
                break;
            }
        }
        yield return YieldInstructionCache.WaitForEndOfFrame;


        Debug.Log("SceneLoad");

        rotateImage.isStop = true;
        rotateImage.gameObject.SetActive(false);

        yield break;

    }


    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {

        if (scene.name == moveSceneName)
        {
            Debug.Log("End");
            waitTime = 0.5f;
            fadeTime = 1f;

            if (Instance == null)
            {
                Debug.Log("인스턴스가 null");
                instance = FindObjectOfType<SceneChanger>();

                if (instance == null)
                {
                    Debug.Log("찾지못함.");
                }
            }


            StartGoColorScreen(waitTime, fadeTime, false);
            SceneManager.sceneLoaded -= LoadSceneEnd;

            Time.timeScale = 1f;

            isLoading = false;

        }

    }
    public void StartGoColorScreen(float _w, float _f, bool _b)
    {
        StartCoroutine(SceneChanger.Instance.GoColorScreen(_w, _f, _b));
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

        float startVal;
        float endVal;
        if (_goBlack) //씬 로드를 해야하는 상황일 때
        {
            startColor = colorTwoMyeong;
            goingColor = colorBlack;
            progressStartColor = colorTwoMyeong;
            progressGoingColor = colorWhite;
            startVal = 0f;
            endVal = 1f;
        }
        else //씬 로드 완료일때
        {
            startColor = colorBlack;
            goingColor = colorTwoMyeong;
            progressStartColor = colorWhite;
            progressGoingColor = colorTwoMyeong;
            startVal = 1f;
            endVal = 0f;
        }

        yield return new WaitForSecondsRealtime(_waitTime);

        canvasGroup.gameObject.SetActive(true);

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / _goingTime;

            //goBlackImage.color = Color32.Lerp(startColor, goingColor, progress);
            //progressBar.color = Color32.Lerp(progressStartColor, progressGoingColor, progress);
            canvasGroup.alpha = Mathf.Lerp(startVal, endVal, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        if (_goBlack == false)
        {
            canvasGroup.gameObject.SetActive(false);
            isLoading = false;

        }
    }
}
