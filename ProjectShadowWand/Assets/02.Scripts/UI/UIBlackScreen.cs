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
    /// ���̵带 �ϴµ� �ʿ��� ������ ���մϴ�.
    /// </summary>
    /// <param name="_waitTIme">���̵� �� ���ð��Դϴ�.</param>
    /// <param name="_fadeTime">�ش� �ʸ�ŭ ���̵带 �����մϴ�.</param>
    /// <param name="_goBlack">true�� ���, ���̵����� �մϴ�. false�� ���, ���̵�ƿ��� �մϴ�.</param>
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
