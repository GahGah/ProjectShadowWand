using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public Dictionary<eUItype, UIBase> uiDicitonary = new Dictionary<eUItype, UIBase>();
    public UIDiary uiDiary;

    public static UIManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
        }
    }

    public void AddToDictionary(UIBase _uiBase)
    {
        uiDicitonary.Add(_uiBase.uiType, _uiBase);
    }

    public void GoUIDiaryToggle()
    {
        uiDiary.Toggle();
    }

}
