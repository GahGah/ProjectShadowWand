using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveSelectImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    public Image selectImage;
    private Transform selectImagePosition;

    [HideInInspector]
    public float goFillAmount;


    [Tooltip("몇 초 동안 변화할 것인가.")]
    public float filledSpeed;

    private float currentFilledSpeed;


    private EventSystem eventSystem;

    private Button testButton;


    void Awake()
    {
        if (selectImage == null)
        {


        }

        if (filledSpeed == 0)
        {
            filledSpeed = 3f;
        }

        currentFilledSpeed = 1f / filledSpeed;

        selectImage.fillAmount = 0f;
        goFillAmount = 0f;
    }

    private void Start()
    {
        eventSystem = EventSystem.current;
    }
    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {

        StartCoroutine(LerpImage());
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
        goFillAmount = 1f;
        //UIManager.Instance.isCanChangeCursor = false;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        // UIManager.Instance.isCanChangeCursor = true;
        goFillAmount = 0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
        //UIManager.Instance.isCanChangeCursor = false;
        goFillAmount = 1f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //  UIManager.Instance.isCanChangeCursor = true;
        goFillAmount = 0f;
        if (eventSystem.currentSelectedGameObject == gameObject)
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }

    IEnumerator LerpImage()
    {
        while (true)
        {

            if (selectImage.fillAmount != goFillAmount) // 현재 fillAmount와 가야할 fillAmount가 다르면
            {
                float _tempTimer = 0f;

                while (true)
                {
                    _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;
                    selectImage.fillAmount = Mathf.Lerp(selectImage.fillAmount, goFillAmount, _tempTimer);

                    yield return YieldInstructionCache.WaitForEndOfFrame;

                    if (Mathf.Abs(selectImage.fillAmount - goFillAmount) < 0.005f)
                    {
                        selectImage.fillAmount = goFillAmount;
                    }

                    if (selectImage.fillAmount == goFillAmount)
                    {
                        break;
                    }
                }

            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }


}
