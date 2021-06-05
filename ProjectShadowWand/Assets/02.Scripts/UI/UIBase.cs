using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI의 기본이 되는 Class
/// </summary>
public class UIBase : MonoBehaviour
{
    [Tooltip("현재 직접 구분해서 type을 정하는 기능이 없습니다. 직접 넣어라!!")]
    public eUItype uiType;

    public GameObject canvasObject;
    public CanvasGroup canvasGroup = null;

    // protected Canvas canvas;
    //private void Start()
    //{
    //    // canvas = canvasObject.GetComponent<Canvas>();
    //}

    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }
    public virtual void Init()
    {
        if (canvasGroup == null)
        {
            if ((canvasGroup = canvasObject.GetComponent<CanvasGroup>()) == false)
            {
                Debug.Log(gameObject.name + " : CanvasGroup을 가지고 있지 않습니다.");
            }

        }
    }
    /// <summary>
    /// UI가 켜졌을 때를 말합니다. 근데 이게 쓰이려나
    /// </summary>
    public virtual void OnActive()
    {

    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    /// <summary>
    /// UI를 열기 위해 호출되는 함수. 성공적으로 열렸다면 true를 반환합니다.
    /// </summary>
    public virtual bool Open()
    {
        if (isFading)
        {
            return false;
        }
        else
        {
            StartCoroutine(ProcessFadeAlpha_Open());
            return true;
        }
    }

    /// <summary>
    /// UI를 닫기 위해 호출되는 함수. 성공적으로 닫혔다면 false를 반환합니다.
    /// </summary>
    public virtual bool Close()
    {
        if (isFading)
        {
            return false;
        }
        else
        {
            StartCoroutine(ProcessFadeAlpha_Close());
            return true;
        }
    }

    /// <summary>
    /// 유아이매니저의 딕셔너리 안에 들어있는지 확인합니다. 없으면 추가합니다.
    /// </summary>  
    protected void DictionaryCheck()
    {
        if (UIManager.Instance.uiDicitonary.ContainsKey(uiType) == false)//목록에 안들어가 있다면
        {
            Debug.Log("Add..." + this);
            UIManager.Instance.AddToDictionary(this);
        }
        else
        {
            Debug.Log(gameObject.name + "는 이미 추가되어있습니다.");
        }
    }


    [Tooltip("페이드 중인지를 뜻합니다.")]
    protected bool isFading = false;
    protected virtual IEnumerator ProcessFadeAlpha_Open()
    {

        canvasGroup.interactable = false;
        yield return StartCoroutine(FadeAlphaCanvasGroup(0f, 1f, 0.1f));
        canvasGroup.interactable = true;
    }


    protected virtual IEnumerator ProcessFadeAlpha_Close()
    {

        canvasGroup.interactable = false;
        yield return StartCoroutine(FadeAlphaCanvasGroup(1f, 0f, 0.1f));
        canvasGroup.interactable = true;
        canvasObject.SetActive(false);


    }
    /// <summary>
    /// 캔버스 그룹의 알파값을 _time 동안 조절시켜줍니다.
    /// </summary>
    /// <param name="_start">시작하는 프로그레스. 0~1</param>
    /// <param name="_end">끝나는 프로그레스. 0~1</param>
    /// <param name="_time">해당 시간동안 알파값을 조절합니다.</param>
    /// <returns></returns>
    protected IEnumerator FadeAlphaCanvasGroup(float _start, float _end, float _time)
    {
        isFading = true;

        float timer = 0f;

        float progress = 0f;

        canvasGroup.alpha = _start;

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;

            progress = timer / _time;

            canvasGroup.alpha = Mathf.Lerp(_start, _end, progress);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        canvasGroup.alpha = _end;
        isFading = false;
    }
}
