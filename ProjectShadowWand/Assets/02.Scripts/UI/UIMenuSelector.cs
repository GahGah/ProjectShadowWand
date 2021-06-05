using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMenuSelector : MonoBehaviour
{
    private UIMainMenu mainMenu;

    [Header("������ ��ư")]
    public Button selectorButton;

    [Header("�޴� Ʈ������")]
    public Transform menuTransform;

    [Header("�޴� ����Ʈ")]
    public Button[] menuList;

    [Header("���̾ƿ� �׷�")]
    public HorizontalLayoutGroup layoutGroup;

    [Header("�� ĭ�� ������ ��...")]
    public float moveValue;


    private int menuCount;
    private int currentMenuCount;

    private List<Vector2> menuPosList;
    private EventSystem eventSystem;
    private Vector2 originalPos;

    private void Start()
    {
        eventSystem = EventSystem.current;

        menuPosList = new List<Vector2>();
        menuCount = menuList.Length;

        currentMenuCount = 0;

        originalPos = menuTransform.position;
        originalSpacing = layoutGroup.spacing;
        for (int i = 0; i < menuCount; i++)
        {
            menuPosList.Add(new Vector2(menuList[i].transform.position.x, menuList[i].transform.position.y));
        }

    }

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
            startPos = menuTransform.position;
            timer = 0f;
            progress = 0f;
        }
        else
        {
            StartCoroutine(UpdateMenuPosition());
        }
    }

    [Tooltip("���� �̵��ؾ��� ��ġ")]
    private Vector2 currentMovePos;
    private Vector2 currentPos;
    private Vector2 startPos;

    private void UpdateMovePosition()
    {
        currentMovePos = new Vector2(originalPos.x, originalPos.y + (moveValue * currentMenuCount));
        Debug.Log("Ŀ��Ʈ ���� ���� : " + currentMovePos + "/ ����Ʈ������������ : " + menuTransform.position);
    }

    public bool isMoving;

    float timer = 0f;
    float progress = 0f;

    float originalSpacing = 70.5f;
    float scalingSpacing = 200f;


    float tempSpacing = 0f;
    float onTimer = 0f;
    private void Update()
    {
        if (isMoving)
        {
            return;
        }
        onTimer += Time.unscaledDeltaTime;
        tempSpacing = Mathf.Sin(onTimer * 4f) * 30f;
        layoutGroup.spacing = originalSpacing + tempSpacing;

    }
    private IEnumerator UpdateMenuPosition()
    {
        isMoving = true;
        startPos = menuTransform.position;

        timer = 0f;
        progress = 0f;
        while (true)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / 0.4f;

            currentPos = Vector2.Lerp(startPos, currentMovePos, progress);
            layoutGroup.spacing = scalingSpacing;//Mathf.Lerp(scalingSpacing, originalSpacing, progress);

            menuTransform.position = currentPos;

            if (Vector2.Distance(menuTransform.position, currentMovePos) < 0.1f)
            {
                menuTransform.position = currentMovePos;
                layoutGroup.spacing = originalSpacing;
                break;
            }
            Debug.Log("Test");
            yield return null;
        }

        isMoving = false;

        //selectorButton. = menuList[currentMenuCount];
    }


}
