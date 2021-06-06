using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuSelector : MonoBehaviour
{
    private UIMainMenu mainMenu;

    [Header("셀렉터 버튼")]
    public Button selectorButton;

    [Header("메뉴 트랜스폼")]
    public Transform selectorTransform;

    [Header("메뉴 리스트")]
    public Button[] menuList;
    private List<Button.ButtonClickedEvent> buttonEventList;

    [Header("레이아웃 그룹")]
    public HorizontalLayoutGroup layoutGroup;


    [Header("커서 움직이는 정도")]
    float originalSpacing = 0f;
    public float scalingSpacing = 200f;
    //[Header("한 칸당 움직일 값...")]
    //public float moveValue;

    private int menuCount;
    private int currentMenuCount;

    private List<Vector2> menuPosList;

    private EventSystem eventSystem;

    private Vector2 originalPos;

    private List<bool> soundCheckList;

    private void Start()
    {
        eventSystem = EventSystem.current;

        menuPosList = new List<Vector2>();
        menuCount = menuList.Length;

        currentMenuCount = 0;

        originalPos = selectorTransform.position;
        originalSpacing = layoutGroup.spacing;

        buttonEventList = new List<Button.ButtonClickedEvent>();


        for (int i = 0; i < menuCount; i++)
        {
            var index = i;

            menuPosList.Add(new Vector2(menuList[i].transform.position.x, menuList[i].transform.position.y));

            var tempEvents = new Button.ButtonClickedEvent();

            tempEvents = menuList[index].onClick;


            buttonEventList.Add(tempEvents);

            menuList[index].onClick = new Button.ButtonClickedEvent();

            menuList[index].onClick.AddListener(delegate { ButtonOnMoveTo(index); });
        }

        if (menuCount != 0)
        {
            selectorButton.onClick = new Button.ButtonClickedEvent();
            selectorButton.onClick = buttonEventList[currentMenuCount];
        }
        soundCheckList = new List<bool>();
        for (int i = 0; i < menuCount; i++)
        {
            var tb = false;
            soundCheckList.Add(tb);
        }


    }


    public bool isMoving;

    float timer = 0f;
    float progress = 0f;




    int tempMenuCount = 0;
    float tempSpacing = 0f;
    float onTimer = 0f;
    private void Update()
    {
        if (isMoving)
        {
            return;
        }

        if (SceneChanger.Instance.isLoading)
        {
            return;
        }

        if (InputManager.Instance.buttonScroll.ReadValue().y > 0)
        {

            if (UIManager.Instance.uiStack.Count != 0)
            {
                if (UIManager.Instance.uiStack.Peek().GetType() == typeof(UIPopup))
                {
                    return;
                }
            }

            if (currentMenuCount == 0)
            {
                currentMenuCount = menuCount - 1;
            }
            else
            {
                currentMenuCount = Mathf.Abs((currentMenuCount - 1) % menuCount);
            }
            SetFalseSoundChecker();
            UpdateMovePosition();
            StartCoroutine(UpdateMenuPosition());

        }
        else if (InputManager.Instance.buttonScroll.ReadValue().y < 0)
        {
            if (UIManager.Instance.uiStack.Count != 0)
            {
                if (UIManager.Instance.uiStack.Peek().GetType() == typeof(UIPopup))
                {
                    return;
                }
            }

            currentMenuCount = Mathf.Abs((currentMenuCount + 1) % menuCount);
            SetFalseSoundChecker();
            UpdateMovePosition();
            StartCoroutine(UpdateMenuPosition());
        }



        onTimer += Time.unscaledDeltaTime;
        tempSpacing = Mathf.Sin(onTimer * 4f) * 30f;
        layoutGroup.spacing = originalSpacing + tempSpacing;

    }




    bool isActive;
    public void ButtonOnMoveTo(int _index)
    {
        if (currentMenuCount == _index)
        {

            return;
        }

        currentMenuCount = _index;
        UpdateMovePosition();

        if (isMoving)
        {
            startPos = selectorTransform.position;
            timer = 0f;
            progress = 0f;
            SetFalseSoundChecker();
        }
        else
        {
            if (currentMenuCount == realMoveCount)
            {
                Debug.Log(currentMenuCount);


            }
            else
            {
                SetFalseSoundChecker();
                StartCoroutine(UpdateMenuPosition());
            }

        }
    }

    [Tooltip("현재 이동해야할 위치")]
    private Vector2 currentMovePos;
    private Vector2 currentPos;
    private Vector2 startPos;

    private void UpdateMovePosition()
    {
        Debug.Log(currentMenuCount);
        currentMovePos = new Vector2(originalPos.x, menuList[currentMenuCount].transform.position.y);//new Vector2(originalPos.x, originalPos.y + (moveValue * currentMenuCount));

    }


    private void SetFalseSoundChecker()
    {
        //for (int i = 0; i < soundCheckList.Count; i++)
        //{
        //    soundCheckList[i] = false;
        //}
    }
    private void CheckSound()
    {
        for (int i = 0; i < menuList.Length; i++)
        {


            if (i == currentMenuCount)
            {
                continue;
            }
            if (Mathf.Abs(selectorTransform.position.y - menuList[i].transform.position.y) < 3f)
            {
                if (soundCheckList[i] == false)
                {
                    soundCheckList[i] = true;
                    AudioManager.Instance.Play_UI_Selector_Move();
                    break;
                }
            }
            else
            {
                Debug.Log(Mathf.Abs(selectorTransform.position.y - menuList[i].transform.position.y));
            }
        }
    }
    int realMoveCount;
    private IEnumerator UpdateMenuPosition()
    {
        isMoving = true;
        isActive = false;
        startPos = selectorTransform.position;

        //for (int i = 0; i < menuCount; i++)
        //{
        //    menuList[i].interactable = false;
        //}


        timer = 0f;
        progress = 0f;

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / 0.2f;

            currentPos = Vector2.Lerp(startPos, currentMovePos, progress);
            layoutGroup.spacing = scalingSpacing;//Mathf.Lerp(scalingSpacing, originalSpacing, progress);

            selectorTransform.position = new Vector3(selectorTransform.position.x, currentPos.y,0);

            //  CheckSound();


            if (Vector2.Distance(selectorTransform.position, currentMovePos) < 0.1f)
            {
                selectorTransform.position = currentMovePos;
                layoutGroup.spacing = originalSpacing;
                break;
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        //for (int i = 0; i < menuCount; i++)
        //{
        //    menuList[i].interactable = false;
        //}
        AudioManager.Instance.Play_UI_Selector_Move();

        realMoveCount = currentMenuCount;
        isActive = true;
        //menuList[currentMenuCount].interactable = true;

        selectorButton.onClick = new Button.ButtonClickedEvent();
        selectorButton.onClick = buttonEventList[currentMenuCount];

        isMoving = false;

        //selectorButton. = menuList[currentMenuCount];
    }

    public void ButtonPushSound()
    {
        AudioManager.Instance.Play_UI_Button_Push();
    }
}
