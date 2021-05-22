using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CutScene의 한 컷입니다.
/// </summary>
public class UICut : UIBase
{
    Image cutImage;
    [Tooltip("컷이 나타날때의 효과가 진행되는 시간입니다.")]
    [Range(0f, 10f)]
    public float processTime = 0.5f;

    [Range(0f, 10f)]
    public float waitTime = 1f;

    [Tooltip("컷이 전부 나타났다면 true가 됩니다.")]
    public bool isOn;

    public bool isSkip;

    [Tooltip("애니메이션을 사용하는가?")]
    public bool useAnimation;

    private void Awake()
    {
        cutImage = GetComponent<Image>();

        canvasObject = gameObject;

        if (useAnimation)
        {
            anim = gameObject.GetComponent<Animator>();
        }
    }
    private void Start()
    {
        Init();
    }
    [HideInInspector]
    public Animator anim;
    public override void Init()
    {
        base.Init();
        isOn = false;


    }




    public override void OnActive()
    {
        base.OnActive();
    }

    public override bool Open()
    {
        StartCoroutine(ProcessOpen_Fade());
        return true;
    }

    private void SetColor(Color32 _c)
    {
        cutImage.color = _c;
    }
    public override bool Close()
    {
        cutImage.CrossFadeAlpha(1f, processTime, true);
        return true;
        //return base.Close();
    }

    public void SetOn()
    {
        Color32 tempColor = cutImage.color;
        SetColor(new Color32(tempColor.r, tempColor.g, tempColor.b, 255));
        isOn = true;
    }
    //컷이 서서히 나타납니다.
    public IEnumerator ProcessOpen_Fade()
    {
        float timer = 0f;
        float progress = 0f;

        Color32 tempColor;
        tempColor = cutImage.color;
        Color32 startColor = new Color32(tempColor.r, tempColor.g, tempColor.b, 0);
        Color32 endColor = new Color32(tempColor.r, tempColor.g, tempColor.b, 255);

        while (progress < 1f)
        {
            if (isSkip)
            {
                break;
            }
            timer += Time.unscaledDeltaTime;
            progress = timer / processTime;

            tempColor = Color32.Lerp(startColor, endColor, progress);

            SetColor(tempColor);

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        SetColor(endColor);

        isOn = true;
    }
}
