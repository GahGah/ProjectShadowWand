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
    private Button button;
    private ActiveSelectImage activer;
    private EventSystem eventSystem;
    // Start is called before the first frame update
    private void Awake()
    {
        button = GetComponent<Button>();
        activer = GetComponent<ActiveSelectImage>();
    }
    void Start()
    {
        eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(gameObject);
        activer.goFillAmount = 1f;
    }

}
