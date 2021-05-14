using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlackScreen : UIBase
{
    public CanvasGroup canvasGroup;

    private float waitTime;
    private float fadeTime;
    private bool goBlack;
    /// <summary>
    /// 페이드를 하는데 필요한 값들을 정합니다.
    /// </summary>
    /// <param name="_waitTIme">페이드 전 대기시간입니다.</param>
    /// <param name="_fadeTime">해당 초만큼 페이드를 진행합니다.</param>
    /// <param name="_goBlack">true일 경우, 페이드인을 합니다. false일 경우, 페이드아웃을 합니다.</param>
    public void SetFadeValue(float _waitTIme, float _fadeTime, bool _goBlack)
    {
        waitTime = _waitTIme;
        fadeTime = _fadeTime;
        goBlack = _goBlack;
    }

    private void Awake()
    {
        uiType = eUItype.BLACKSCREEN;
    }
    private void Start()
    {
        Init();
        UIManager.Instance.AddToDictionary(this);
    }
    public override void Init()
    {
        canvasGroup.gameObject.SetActive(false);
    }
    public override bool Open()
    {

        StartCoroutine(GoFadeScreen());
        return true;
    }

    public override bool Close()
    {
        return true;
    }

    public IEnumerator GoFadeScreen()
    {
        var timer = 0f;
        var progress = 0f;

        float startVal;
        float endVal;
        if (goBlack) 
        {
            startVal = 0f;
            endVal = 1f;
        }
        else 
        {
            startVal = 1f;
            endVal = 0f;
        }

        yield return new WaitForSeconds(waitTime);

        canvasGroup.gameObject.SetActive(true);


        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / fadeTime;

            canvasGroup.alpha = Mathf.Lerp(startVal, endVal, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        if (goBlack == false)
        {
            canvasGroup.gameObject.SetActive(false);
            yield break;
        }
    }
}
