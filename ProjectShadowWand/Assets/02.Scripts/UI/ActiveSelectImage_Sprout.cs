using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveSelectImage_Sprout : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image selectImage_Stem;

    public Image[] selectImage_Leaf;


    [HideInInspector]
    public float goFillAmount;


    [Tooltip("�� �� ���� ��ȭ�� ���ΰ�.")]
    public float filledSpeed;

    private float currentFilledSpeed;

    private float currentProgress;
    private bool isWorking;

    private EventSystem eventSystem;

    private Button testButton;


    void Awake()
    {
        if (selectImage_Stem == null)
        {


        }

        if (filledSpeed == 0)
        {
            filledSpeed = 3f;
        }

        currentFilledSpeed = 1f / filledSpeed;

        selectImage_Stem.fillAmount = 0f;

        for (int i = 0; i < selectImage_Leaf.Length; i++)
        {
            selectImage_Leaf[i].fillAmount = 0f;
        }
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

        StartCoroutine(LerpSproutImage());
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

    private bool isStartLerpLeaf;


    private IEnumerator LerpSproutImage()
    {
        while (true)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (isWorking)
            {
                continue;
            }
            else
            {
                if (goFillAmount > currentProgress) //���콺�� �÷ȴµ� ������ �ڶ��� ���� �����϶�
                {
                    StartCoroutine(AppearSprout());
                }
                else if (goFillAmount < currentProgress) //���콺�� �������µ� ������ ������� ���� �����϶�
                {
                    StartCoroutine(DisappearSprout());
                }
            }

        }
    }


    //������ �ڶ��ϴ�.
    private IEnumerator AppearSprout()
    {
        isWorking = true;
        float timer = 0f;
        float progress = 0f;

        int length = selectImage_Leaf.Length;

        while (progress < filledSpeed)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / filledSpeed;

            selectImage_Stem.fillAmount = Mathf.Lerp(0f, 1f, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        timer = 0f;
        progress = 0f;

        while (progress < filledSpeed)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / filledSpeed;

            for (int i = 0; i < length; i++)
            {
                selectImage_Leaf[i].fillAmount = Mathf.Lerp(0f, 1f, progress);
            }

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        currentProgress = 1f;
        isWorking = false;
    }

    //������ ������ϴ�.

    private IEnumerator DisappearSprout()
    {
        isWorking = true;
        float timer = 0f;
        float progress = 0f;

        int length = selectImage_Leaf.Length;

        while (progress < filledSpeed)
        {
            timer += Time.unscaledDeltaTime;
            progress = 1f - (timer / filledSpeed);

            selectImage_Stem.fillAmount = Mathf.Lerp(1f, 0f, progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        timer = 0f;
        progress = 0f;

        while (progress < filledSpeed)
        {
            timer += Time.unscaledDeltaTime;
            progress = 1f - (timer / filledSpeed);

            for (int i = 0; i < length; i++)
            {
                selectImage_Leaf[i].fillAmount = Mathf.Lerp(1f, 0f, progress);
            }

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        currentProgress = 0f;
        isWorking = false;
    }
    private IEnumerator LerpImage_Sprout()
    {
        float tempfillAmount = 0f;
        while (true)
        {
            tempfillAmount = goFillAmount;
            if (isStartLerpLeaf == false)//���� ���� ���� ���� ���¶��
            {
                if (selectImage_Stem.fillAmount != tempfillAmount) // ���� fillAmount�� ������ fillAmount�� �ٸ���
                {
                    float _tempTimer = 0f;

                    while (true)
                    {
                        _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;
                        selectImage_Stem.fillAmount = Mathf.Lerp(selectImage_Stem.fillAmount, tempfillAmount, _tempTimer);

                        yield return YieldInstructionCache.WaitForEndOfFrame;

                        if (Mathf.Abs(selectImage_Stem.fillAmount - goFillAmount) < 0.005f)
                        {
                            selectImage_Stem.fillAmount = tempfillAmount;
                        }

                        if (selectImage_Stem.fillAmount == tempfillAmount)
                        {
                            break;
                        }
                    }

                    if (selectImage_Leaf[0].fillAmount != tempfillAmount)
                    {
                        _tempTimer = 0f;
                        isStartLerpLeaf = true;
                        while (true)
                        {
                            _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;

                            for (int i = 0; i < selectImage_Leaf.Length; i++)
                            {
                                selectImage_Leaf[i].fillAmount = Mathf.Lerp(selectImage_Leaf[i].fillAmount, tempfillAmount, _tempTimer);
                            }

                            yield return YieldInstructionCache.WaitForEndOfFrame;


                            for (int i = 0; i < selectImage_Leaf.Length; i++)
                            {
                                selectImage_Leaf[i].fillAmount = Mathf.Lerp(selectImage_Leaf[i].fillAmount, tempfillAmount, _tempTimer);


                                if (Mathf.Abs(selectImage_Leaf[i].fillAmount - tempfillAmount) < 0.005f)
                                {
                                    selectImage_Leaf[i].fillAmount = tempfillAmount;
                                }
                            }

                            if (selectImage_Leaf[0].fillAmount == tempfillAmount)
                            {
                                break;
                            }



                        }

                    }

                }
            }
            else //���� �� ���¶��
            {
                if (selectImage_Leaf[0].fillAmount != goFillAmount)
                {
                    float _tempTimer = 0f;
                    isStartLerpLeaf = true;
                    while (true)
                    {
                        _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;

                        for (int i = 0; i < selectImage_Leaf.Length; i++)
                        {
                            selectImage_Leaf[i].fillAmount = Mathf.Lerp(selectImage_Leaf[i].fillAmount, goFillAmount, _tempTimer);
                        }

                        yield return YieldInstructionCache.WaitForEndOfFrame;


                        for (int i = 0; i < selectImage_Leaf.Length; i++)
                        {
                            selectImage_Leaf[i].fillAmount = Mathf.Lerp(selectImage_Leaf[i].fillAmount, goFillAmount, _tempTimer);


                            if (Mathf.Abs(selectImage_Leaf[i].fillAmount - goFillAmount) < 0.005f)
                            {
                                selectImage_Leaf[i].fillAmount = goFillAmount;
                            }
                        }

                        if (selectImage_Leaf[0].fillAmount == goFillAmount)
                        {
                            break;
                        }

                    }

                    isStartLerpLeaf = false;

                    if (selectImage_Stem.fillAmount != goFillAmount) // ���� fillAmount�� ������ fillAmount�� �ٸ���
                    {
                        _tempTimer = 0f;

                        while (true)
                        {
                            _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;
                            selectImage_Stem.fillAmount = Mathf.Lerp(selectImage_Stem.fillAmount, goFillAmount, _tempTimer);

                            yield return YieldInstructionCache.WaitForEndOfFrame;

                            if (Mathf.Abs(selectImage_Stem.fillAmount - goFillAmount) < 0.005f)
                            {
                                selectImage_Stem.fillAmount = goFillAmount;
                            }

                            if (selectImage_Stem.fillAmount == goFillAmount)
                            {
                                break;
                            }
                        }



                    }
                }

            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
    private IEnumerator LerpImage()
    {
        while (true)
        {
            if (selectImage_Stem.fillAmount != goFillAmount) // ���� fillAmount�� ������ fillAmount�� �ٸ���
            {
                float _tempTimer = 0f;

                while (true)
                {
                    _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;
                    selectImage_Stem.fillAmount = Mathf.Lerp(selectImage_Stem.fillAmount, goFillAmount, _tempTimer);

                    yield return YieldInstructionCache.WaitForEndOfFrame;

                    if (Mathf.Abs(selectImage_Stem.fillAmount - goFillAmount) < 0.005f)
                    {
                        selectImage_Stem.fillAmount = goFillAmount;
                    }

                    if (selectImage_Stem.fillAmount == goFillAmount)
                    {
                        break;
                    }
                }





            }
            yield return YieldInstructionCache.WaitForEndOfFrame;

        }
    }


}

