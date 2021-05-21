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
