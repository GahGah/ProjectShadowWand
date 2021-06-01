using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UICut을 재생시켜주는 클래스.
/// </summary>
public class UICutScene : UIBase
{

    public CanvasGroup canvasGroup;

    [Tooltip("컷 목록")]
    public List<UICut> cutList;

    [SerializeField]
    private UICut currentCut;

    private int cutCount;
    private int currentCutNumber;

    [Range(0f, 5f)]
    public float fadeTime = 2f;

    [Tooltip("마지막 씬의 isOn까지 true가 되었다면, true가 됩니다.")]
    public bool isEnd;

    [Header("씬의 종료를 검은색 페이드로")]
    [Tooltip("씬의 맨 마지막에서 검은색으로 Fade가 됩니다.")]
    public bool isLastFade;
    public UICut frontBlackCut;

    public bool isNext;
    //private void Start()
    //{
    //    Init();

    //}

    private void Awake()
    {
        Init();
        uiType = eUItype.CUTSCENE;
    }
    private void Start()
    {
        UIBase tempUI;
        if (UIManager.Instance.uiDicitonary.TryGetValue(uiType, out tempUI) == false)
        {
            UIManager.Instance.AddToDictionary(this);
        }

        for (int i = 0; i < cutCount; i++)
        {
            cutList[i].SetActive(false);
        }

        //    canvasObject = gameObject;
        canvasObject.SetActive(false);
    }

    public override void Init()
    {
        cutCount = cutList.Count;
        isEnd = false;
        currentCut = null;
        currentCutNumber = 0;
    }

    public override bool Open()
    {

        StartPlayCutScene();
        return true;
    }
    public override bool Close()
    {
        StartCloseCutScene();
        return true;
    }

    //이미지컷씬을 재생시작합니다.
    public void StartPlayCutScene()
    {
        canvasObject.SetActive(true);
        StartCoroutine(ProcessCutScene());
    }

    private void Update()
    {
        if (InputManager.Instance.buttonTalkNext.wasPressedThisFrame || InputManager.Instance.buttonMouseLeft.wasPressedThisFrame)
        {
            isNext = true;
        }

    }
    public void StartCloseCutScene()
    {
        StartCoroutine(ProcessClose_Fade());
    }

    private IEnumerator ProcessOpen_Fade()
    {

        canvasObject.SetActive(true);
        canvasGroup.alpha = 0f;

        float timer = 0f;
        float progress = 0f;
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeTime;
            canvasGroup.alpha = Mathf.Clamp(progress, 0f, 1f);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        StartCoroutine(ProcessCutScene());
    }

    public IEnumerator ProcessCutScene()
    {
        Debug.Log("StartCutscene");
        canvasObject.SetActive(true);
        UIBase tempUI;
        UIManager.Instance.uiDicitonary.TryGetValue(uiType, out tempUI);
        if (tempUI != this)
        {
            UIManager.Instance.RemoveToDictionary(tempUI);
            UIManager.Instance.AddToDictionary(this);
        }


        if (cutCount == 0) //갯수가 0이면
        {
            Debug.LogWarning("UICutScene : 이상하다...재생시킬 컷이 없어! 추가를 깜빡한거 아니야?");
            yield break;
        }
        isNext = false;
        isEnd = false;

        currentCut = null;
        currentCutNumber = -1;

        while (currentCutNumber < cutCount - 1) //작을 때 까지 반복
        {
            isNext = false;
            currentCutNumber += 1;
            Debug.Log("현재 컷 넘버 : " + currentCutNumber);

            currentCut = cutList[currentCutNumber];

            currentCut.SetActive(true);


            currentCut.Open();

            while (!currentCut.isOn)
            {
                if (isNext)
                {
                    currentCut.isSkip = true;
                    break;
                }
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            float nextTimer = 0f;
            if (currentCutNumber != cutCount - 1)
            {
                if (isNext == false)
                {
                    while (nextTimer < currentCut.waitTime)
                    {
                        if (isNext)
                        {
                            isNext = false;
                            break;
                        }
                        nextTimer += Time.unscaledDeltaTime;

                        yield return YieldInstructionCache.WaitForEndOfFrame;
                    }

                }
            }
            
            if (currentCut.useAnimation == true)
            {
                currentCut.anim.enabled = false;
            }

        }

        yield return null;




        if (isLastFade == true)
        {
            isNext = false;


            while (isNext == false)
            {
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }


            frontBlackCut.SetActive(true);
            frontBlackCut.Open();

            while (!frontBlackCut.isOn)
            {
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            for (int i = 0; i < cutCount; i++)
            {
                cutList[i].SetActive(false);
            }

        }


        isEnd = true;
        isNext = false;
        //        StartCloseCutScene();
    }

    public IEnumerator ProcessClose_Fade()
    {

        float timer = 0f;
        float progress = 0f;
        canvasGroup.alpha = 0f;
        float alphaVal = 1f;
        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / fadeTime;
            alphaVal = 1f - progress;
            canvasGroup.alpha = Mathf.Clamp(alphaVal, 0f, 1f);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        isEnd = true;
        canvasObject.SetActive(false);

    }

}
