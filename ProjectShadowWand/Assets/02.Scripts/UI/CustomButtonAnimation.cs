using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 버튼 애니메이션입니다.
/// </summary>
public class CustomButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    private EventSystem eventSystem;
    private RectTransform rectTransform;

    private Vector3 oneScale;
    private Vector3 bigScale;

    [Tooltip("속도")]
    private float speed = 1f;

    [HideInInspector]
    public bool isBigScale;


    //  private bool isChangeColor;
    //   private bool currentColor;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }

        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
        isBigScale = true;
        AudioManager.Instance.Play_Button();

        //     isChangeColor = true;

    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (eventSystem.currentSelectedGameObject == gameObject)
        {
            eventSystem.SetSelectedGameObject(null);
        }

        isBigScale = false;
        //        isChangeColor = false;
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }


        isBigScale = true;
        //isChangeColor = true;
    }
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        //if (eventSystem.currentSelectedGameObject == gameObject)
        //{
        //    eventSystem.SetSelectedGameObject(null);
        //}

        //     isChangeColor = false;
        isBigScale = false;
    }


    private void Awake()
    {
        bigScale = new Vector3(1.1f, 1.1f, 1f);
        oneScale = Vector3.one;
        //color_White = Color.white;
        //color_Gray = Color.gray;
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        ChangeScale();
    }

    private void ChangeScale()
    {
        if (isBigScale)
        {
            if (Mathf.Abs(rectTransform.localScale.x - bigScale.x) > 0.01f)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, bigScale, Time.fixedUnscaledDeltaTime * speed);

            }
        }
        else
        {
            if (Mathf.Abs(rectTransform.localScale.x - oneScale.x) > 0.01f)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, oneScale, Time.fixedUnscaledDeltaTime * speed);
            }


        }
    }
}
