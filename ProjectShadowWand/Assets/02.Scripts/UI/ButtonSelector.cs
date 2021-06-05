using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 맨 처음에 선택될 버튼입니다.
/// </summary>
public class ButtonSelector : MonoBehaviour
{
    [Tooltip("처음에 선택될 버튼 오브젝트입니다.")]
    public Button activeButton;

    private ActiveSelectImage_Sprout activer;
    //private CustomButtonAnimation cbActiver;

    [HideInInspector]
    public EventSystem eventSystem;

    // Start is called before the first frame update
    private void Awake()
    {
        if (ReferenceEquals(activer, null))
        {
            activer = activeButton.GetComponent<ActiveSelectImage_Sprout>();
        }

        //if (ReferenceEquals(cbActiver, null))
        //{
        //    cbActiver = activeButton.GetComponent<CustomButtonAnimation>();
        //}

    }
    void Start()
    {
        eventSystem = EventSystem.current;
    }


    //public void ForceSelect_BasicButton()
    //{
    //    if (eventSystem == null)
    //    {
    //        eventSystem = EventSystem.current;
    //    }

    //    if (activer != null)
    //    {
    //        eventSystem.SetSelectedGameObject(null);
    //        eventSystem.SetSelectedGameObject(activeButton.gameObject);
    //        cbActiver.isBigScale = true;
    //    }
    //}
    //public void StaySelect_BasicButton()
    //{
    //    if (eventSystem.currentSelectedGameObject != activeButton.gameObject)
    //    {
    //        ForceDeSelect_BasicButton();
    //    }

    //}

    //public void ForceDeSelect_BasicButton()
    //{
    //    if (eventSystem == null)
    //    {
    //        eventSystem = EventSystem.current;
    //    }
    //    if (activer != null)
    //    {
    //        //if (eventSystem.currentSelectedGameObject == activeButton.gameObject)
    //        //{
    //        eventSystem.SetSelectedGameObject(null);
    //         =
    //        //}
    //    }
    //}




    public void ForceSelect()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(activeButton.gameObject);

        if (activer != null)
        {
            activer.goFillAmount = 1f;
        }

    }

    public void StaySelect()
    {
        if (eventSystem.currentSelectedGameObject != activeButton.gameObject)
        {
            ForceDeSelect();
        }

    }

    public void ForceDeSelect()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }

        eventSystem.SetSelectedGameObject(null);
        if (activer != null)
        {
            //if (eventSystem.currentSelectedGameObject == activeButton.gameObject)
            //{

            activer.goFillAmount = 0f;
            //}
        }
    }
}
