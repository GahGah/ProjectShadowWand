using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("UI를 열면 해당 UI는 가장 마지막 자식으로 변경됩니다.\n" +
        "즉, 가장 위에 그려지게 됩니다.")]
    public bool SetAsLastSiblingToOpen;

    public Dictionary<eUItype, UIBase> uiDicitonary = new Dictionary<eUItype, UIBase>();

    public UIDiary uiDiary;
    public Stack<UIBase> uiStack;


    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
        }

        Init();
    }

    public void Init()
    {
        uiStack = new Stack<UIBase>();
        uiStack.Clear();
    }

    public void AddToDictionary(UIBase _uiBase)
    {
        uiDicitonary.Add(_uiBase.uiType, _uiBase);
    }

    public void GoUIDiaryToggle()
    {
        uiDiary.Toggle();
    }


    private void Update()
    {
        if (InputManager.Instance.buttonEscape.wasPressedThisFrame)
        {
            if (uiStack.Count != 0)
            {
                CloseTop();
            }
            else
            {
                OpenThis(uiDicitonary[eUItype.PAUSE]);
            }

        }

    }
    public void OpenThis(UIBase _uiBase)
    {
        if (_uiBase.Open() == true) //정상적으로 열렸을 때만
        {

            uiStack.Push(_uiBase);
            if (SetAsLastSiblingToOpen)
            {
                _uiBase.transform.SetAsLastSibling();
            }

        }

    }

    /// <summary>
    /// 스택의 맨 위에 있는 UI를 닫습니다.
    /// </summary>
    public void CloseTop()
    {
        UIBase nowUI = null;

        if (uiStack.Count != 0)
        {
            nowUI = uiStack.Pop();
            if (nowUI.Close() == true) // 정상적으로 닫혔을 때만
            {

            }
            else // 정상적으로 안닫혔다면 다시 넣기...Push
            {
                uiStack.Push(nowUI);
            }
        }
        else
        {
            Debug.Log("닫을 UI가 없어");
        }
    }

    public void CloseThis(UIBase _uiBase)
    {
        if (_uiBase.Close() == true)
        {

        }
    }
}
