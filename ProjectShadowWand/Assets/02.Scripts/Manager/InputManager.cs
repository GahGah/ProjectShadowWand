﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class InputManager : Manager<InputManager>
{
    public ButtonControl buttonUp;// = Keyboard.current.wKey;
    public ButtonControl buttonDown;// = Keyboard.current.sKey;
    public ButtonControl buttonMoveLeft; //= Keyboard.current.aKey;
    public ButtonControl buttonMoveRight; //= Keyboard.current.dKey;
    public ButtonControl buttonMoveJump;// = Keyboard.current.spaceKey;
    public ButtonControl buttonCtrl;// = Keyboard.current.ctrlKey;
    public ButtonControl buttonPause;// = Keyboard.current.escapeKey;
    public ButtonControl buttonCatch; //= Keyboard.current.leftShiftKey;

    public ButtonControl buttonMouseLeft;// = Mouse.current.leftButton;
    public Vector2Control buttonScroll;// = Mouse.current.scroll;

    public ButtonControl buttonEscape;

    public ButtonControl buttonPush;

    public ButtonControl buttonSkillUse;

    public ButtonControl buttonSkillWater;
    public ButtonControl buttonSkillLightning;

    public ButtonControl buttonTalkNext;

    public ButtonControl buttonInteract;

    public ButtonControl buttonCapture;
    public Keyboard keyboard;
    public bool isDebugMode;

    public ButtonControl buttonAnyKey;
    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        if (Keyboard.current == null)
        {
            var playerSettings = new UnityEditor.SerializedObject(Resources.FindObjectsOfTypeAll<UnityEditor.PlayerSettings>()[0]);
            var newInputSystemProperty = playerSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
            bool newInputSystemEnabled = newInputSystemProperty != null ? newInputSystemProperty.boolValue : false;

            if (newInputSystemEnabled)
            {
                var msg = "New Input System backend is enabled but it requires you to restart Unity, otherwise the player controls won't work. Do you want to restart now?";
                if (UnityEditor.EditorUtility.DisplayDialog("Warning", msg, "Yes", "No"))
                {
                    UnityEditor.EditorApplication.ExitPlaymode();
                    var dataPath = Application.dataPath;
                    var projectPath = dataPath.Substring(0, dataPath.Length - 7);
                    UnityEditor.EditorApplication.OpenProject(projectPath);
                }
            }
        }
#endif
        keyboard = Keyboard.current;
        SetButtonsDefaultKey();
    }


    //private void Start()
    //{

    //}


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
        if (isDebugMode)
        {
            Debug.LogWarning(this.name + " : 입력된 키 [ " + c + " ]");
        }

    }

    /// <summary>
    /// 기본 조작키로 설정합니다. 
    /// </summary>
    public void SetButtonsDefaultKey()
    {
        buttonUp = Keyboard.current.upArrowKey;
        buttonMoveLeft = Keyboard.current.leftArrowKey;
        buttonDown = Keyboard.current.downArrowKey;
        buttonMoveJump = Keyboard.current.spaceKey;
        buttonMoveRight = Keyboard.current.rightArrowKey;
        buttonMouseLeft = Mouse.current.leftButton;
        buttonCtrl = Keyboard.current.ctrlKey;

        buttonPause = Keyboard.current.escapeKey;

        buttonScroll = Mouse.current.scroll;

        buttonEscape = Keyboard.current.escapeKey;
        buttonCatch = Keyboard.current.fKey;
        buttonSkillWater = Keyboard.current.aKey;
        buttonSkillLightning = Keyboard.current.sKey;

        buttonTalkNext = Keyboard.current.spaceKey;

        buttonInteract = Keyboard.current.fKey;

        buttonCapture = Keyboard.current.pKey;

        buttonAnyKey = Keyboard.current.anyKey;

        //이 밑 키들은 아마도 안씀
        buttonPush = Keyboard.current.cKey;
        buttonSkillUse = Keyboard.current.xKey;

    }

    /// <summary>
    /// 사용자가 설정한 조작키로 설정합니다.
    /// </summary>
    public void SetButtonsCustomKey()
    {
        //버튼1  = Keyboard.current.FindKeyOnCurrentKeyboardLayout("Json에서 읽어온 버튼1 스트링");
    }

}
