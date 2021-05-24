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
        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            goFillAmount = 0f;
        }
   
    }
    //private void OnDisable()
    //{
    //    eventSystem.SetSelectedGameObject(null);
    //}
    public void Init()
    {

        StartCoroutine(UpdataSprout());
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
        if (eventSystem.currentSelectedGameObject != gameObject)
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
        AudioManager.Instance.Play_Button();
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
        //if (eventSystem.currentSelectedGameObject == gameObject)
        //{
        //    eventSystem.SetSelectedGameObject(null);
        //}
    }


    private IEnumerator UpdataSprout()
    {
        float tempfillAmount;

        float _tempTimer = 0f;


        while (true)
        {
            tempfillAmount = goFillAmount;

            if (selectImage_Stem.fillAmount == 1f)//�� ������ ���� 
            {
                _tempTimer = 0f;

                while (true)
                {
                    if (selectImage_Leaf[0].fillAmount != tempfillAmount) //�ٸ��ٸ�
                    {
                        _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;

                        ChangeLeafFillAmount(tempfillAmount, _tempTimer);
                    }

                    yield return YieldInstructionCache.WaitForEndOfFrame;

                    if (Mathf.Abs(selectImage_Leaf[0].fillAmount - tempfillAmount) < 0.005f)
                    {
                        for (int i = 0; i < selectImage_Leaf.Length; i++)
                        {
                            selectImage_Leaf[i].fillAmount = tempfillAmount;
                        }
                    }

                    if (selectImage_Leaf[0].fillAmount == tempfillAmount)
                    {
                        break;
                    }


                }
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }

            if (selectImage_Stem.fillAmount != tempfillAmount && selectImage_Leaf[0].fillAmount == 0f) // ���� fillAmount�� ������ fillAmount�� �ٸ���
            {
                if (tempfillAmount == 1f) //���ܳ����ϰ�, ������ ���ڶ��������� 
                {
                    _tempTimer = 0f;

                    while (true)
                    {
                        _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;

                        selectImage_Stem.fillAmount = Mathf.Lerp(selectImage_Stem.fillAmount, tempfillAmount, _tempTimer);

                        yield return YieldInstructionCache.WaitForEndOfFrame;

                        if (Mathf.Abs(selectImage_Stem.fillAmount - tempfillAmount) < 0.005f)
                        {
                            selectImage_Stem.fillAmount = tempfillAmount;
                        }

                        if (selectImage_Stem.fillAmount == tempfillAmount)
                        {
                            break;
                        }
                    }

                }
                else//��������ϰ�, �� ���� ���ڶ����� ���� 
                {
                    _tempTimer = 0f;
                    while (true)
                    {
                        if (selectImage_Stem.fillAmount != tempfillAmount)
                        {
                            _tempTimer += Time.unscaledDeltaTime * currentFilledSpeed;
                            selectImage_Stem.fillAmount = Mathf.Lerp(selectImage_Stem.fillAmount, tempfillAmount, _tempTimer);

                            yield return YieldInstructionCache.WaitForEndOfFrame;

                            if (Mathf.Abs(selectImage_Stem.fillAmount - tempfillAmount) < 0.005f)
                            {
                                selectImage_Stem.fillAmount = tempfillAmount;
                            }

                            if (selectImage_Stem.fillAmount == tempfillAmount)
                            {
                                break;
                            }
                        }

                        yield return YieldInstructionCache.WaitForEndOfFrame;
                    }
                }

            }
            yield return YieldInstructionCache.WaitForEndOfFrame;

        }

    }
    private void ChangeLeafFillAmount(float _goalAmount, float _timer)
    {

        for (int i = 0; i < selectImage_Leaf.Length; i++)
        {
            selectImage_Leaf[i].fillAmount = Mathf.Lerp(selectImage_Leaf[i].fillAmount, _goalAmount, _timer);

            if (Mathf.Abs(selectImage_Leaf[i].fillAmount - _goalAmount) < 0.005f)
            {
                selectImage_Leaf[i].fillAmount = _goalAmount;
            }
        }
    }

}

