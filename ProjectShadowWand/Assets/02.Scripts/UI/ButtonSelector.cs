using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �� ó���� ���õ� ��ư�Դϴ�.
/// </summary>
public class ButtonSelector : MonoBehaviour
{
    [Tooltip("ó���� ���õ� ��ư ������Ʈ�Դϴ�.")]
    public Button activeButton;


    private ActiveSelectImage_Sprout activer;

    [HideInInspector]
    public EventSystem eventSystem;

    // Start is called before the first frame update
    private void Awake()
    {

        activer = activeButton.GetComponent<ActiveSelectImage_Sprout>();
        Debug.Log("AWAKE!!");
    }
    void Start()
    {
        eventSystem = EventSystem.current;
    }
    public void ForceSelect()
    {
        if (eventSystem == null)
        {
            eventSystem = EventSystem.current;
        }
        if (activer != null)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(activeButton.gameObject);
            activer.goFillAmount = 1f;
        }

    }
}
