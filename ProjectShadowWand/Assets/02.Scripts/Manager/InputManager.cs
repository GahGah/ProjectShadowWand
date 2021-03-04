using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class InputManager : MonoBehaviour
{
    public ButtonControl buttonUp;// = Keyboard.current.wKey;
    public ButtonControl buttonDown;// = Keyboard.current.sKey;
    public ButtonControl buttonLeft; //= Keyboard.current.aKey;
    public ButtonControl buttonRight; //= Keyboard.current.dKey;
    public ButtonControl buttonJump;// = Keyboard.current.spaceKey;
    public ButtonControl buttonCtrl;// = Keyboard.current.ctrlKey;
    public ButtonControl buttonPause;// = Keyboard.current.escapeKey;
    public ButtonControl buttonCatch; //= Keyboard.current.leftShiftKey;

    public ButtonControl buttonMouseLeft;// = Mouse.current.leftButton;
    public Vector2Control buttonScroll;// = Mouse.current.scroll;

    public static InputManager Instance;


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
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        SetButtonsDefaultKey();
    }


    public void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    public void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }
    public void OnTextInput(char c)
    {
        Debug.LogWarning(this.name + " : 입력된 키 [ " + c + " ]");
    }

    /// <summary>
    /// 기본 조작키로 설정합니다. 
    /// </summary>
    public void SetButtonsDefaultKey()
    {
        buttonUp = Keyboard.current.wKey;
        buttonLeft = Keyboard.current.aKey;
        buttonDown = Keyboard.current.sKey;
        buttonRight = Keyboard.current.dKey;
        buttonJump = Keyboard.current.spaceKey;
        buttonMouseLeft = Mouse.current.leftButton;
        buttonCtrl = Keyboard.current.ctrlKey;
        buttonPause = Keyboard.current.escapeKey;
        buttonCatch = Keyboard.current.leftShiftKey;
        buttonScroll = Mouse.current.scroll;
    }

    /// <summary>
    /// 사용자가 설정한 조작키로 설정합니다.
    /// </summary>
    public void SetButtonsCustomKey()
    {
        //버튼1  = Keyboard.current.FindKeyOnCurrentKeyboardLayout("Json에서 읽어온 버튼1 스트링");
    }

}
