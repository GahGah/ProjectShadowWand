using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkSystemManager : MonoBehaviour
{

    [Tooltip("CSV�� �ҷ��� ������ ������ ����ִ� ��ųʸ� ����Ʈ�Դϴ�.")]
    public List<Dictionary<string, object>> talkData;

    [Tooltip("�̸��� ǥ�õ� Text")]
    public Text nameText;

    [Tooltip("���ڰ� ǥ�õ� Text")]
    public Text talkText;

    public GameObject nextButton;

    [Tooltip("�������� ���� ��ư�� �����°�?")]
    public bool isNextPressed;//�������� ���� ��ư�� ���ȴ°�?

    [Tooltip("�� ���ڰ� ��µǴ� �ӵ��Դϴ�.")]
    public float talkSpeed;

    public Coroutine TalkCoroutine;

    private string filePath;

    [Tooltip("���� ĳ���� �̸�")]
    private string currentCharName;

    [Tooltip("���� ��ũ �ڵ�")]
    private int currentTalkCode;


    [Tooltip("���� ��ũ ����.")]
    private string currentTalkText;

    [Tooltip("���� ��ũ ����")]
    private int currentTalkMove;

    [Tooltip("���� ��ũ ���̽�. �켱�� ������ �ʽ��ϴ�.")]
    private int currentTalkFace;

    public Text spaceTest;
    private void Awake()
    {
        Init();
    }

    void Start()
    {
        StartCoroutine(ProcessStart());
        spaceTest.text = "Test\nTest";
    }

    public void Init()
    {

        if (talkText == null)
        {
            Debug.Log("��ũ�ؽ�Ʈ�� ��");
        }

        SetTalkClose();
    }


    // Update is called once per frame
    void Update()
    {
        //if (PlayerController.Instance.isTalking)
        //{
            if (InputManager.Instance.buttonTalkNext.wasPressedThisFrame)
            {
                SetTrueGoNext();
            }
        //}
    }

    public IEnumerator ProcessStart()
    {
        yield return StartCoroutine(GoReadTalkData("TestTalkData"));
        Debug.Log("�ҷ����� �Ϸ�.");

        StartGoTalk(0);
    }

    /// <summary>
    /// Resources.Load�� �̿��Ͽ� ������ �ҷ��ɴϴ�.
    /// </summary>
    /// <param name="path">�ҷ��� ���� �̸��� �����ֽø� �˴ϴ�.</param>
    public IEnumerator GoReadTalkData(string path)
    {
        filePath = "TalkDataFiles/" + path;
        talkData = CsvReader.Read(filePath);
        Debug.Log("OK?...");
        yield return null;
    }

    public void TalkDataLoad(string path)
    {
        //selectButtonObj = new GameObject[3];
        filePath = "TalkDataFiles/" + path;
        talkData = CsvReader.Read(filePath);
        Debug.Log("OK?...");
    }

    /// <summary>
    /// ��ȭ�� �����ŵ�ϴ�. ����� ��ư, ĳ����, ������ ���� ���� ��Ȱ��ȭ ��ŵ�ϴ�. ����,�÷��̾� ��Ʈ�ѷ��� isTalking�� false�� �մϴ�.
    /// </summary>
    public void SetTalkClose()
    {
        if (TalkCoroutine != null)
        {
            StopCoroutine(TalkCoroutine);
            TalkCoroutine = null;
        }
        //PlayerController.Instance.isTalking = false;
    }

    /// <summary>
    /// goNext�� Ʈ��� �����մϴ�. goNext�� ������ �ؽ�Ʈ�� ��µǸ�, GoTalk�ڷ�ƾ���� �ڵ����� false�� �˴ϴ�.
    /// </summary>
    public void SetTrueGoNext()
    {
        isNextPressed = true;

    }
    public void StartGoTalk(int TALK_CODE)
    {
        //PlayerController.Instance.isTalking = true;

        if (TalkCoroutine != null)
        {
            StopCoroutine(TalkCoroutine);
            TalkCoroutine = null;
        }
        TalkCoroutine = StartCoroutine(ProcessTalk(TALK_CODE));
    }

    /// <summary>
    /// �� ��ȭ(�� â�� ������...)�� �����մϴ�.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <returns></returns>
    IEnumerator ProcessTalk(int TALK_CODE)
    //int TALK_CODE_START, int TALK_CODE_END)
    {
        isNextPressed = false;

        currentTalkCode = (int)talkData[TALK_CODE]["TALK_CODE"];
        currentCharName = talkData[TALK_CODE]["TALK_CHAR_NAME"] as string;
        currentTalkMove = (int)talkData[TALK_CODE]["TALK_MOVE"];

        currentTalkFace = (int)talkData[TALK_CODE]["TALK_FACE"];

        currentTalkText = talkData[TALK_CODE]["TALK_NAEYONG"] as string;

        //talkWindow.SetActive(true);

        nameText.text = currentCharName;

       // bool isSkip = false;
        for (int s = 0; s < currentTalkText.Length + 1; s++)
        {
            talkText.text = currentTalkText.Substring(0, s);

            if (isNextPressed) //���� �ؽ�Ʈ�� �� �������� �ʾҴµ� ���� ��ư�� ���ȴٸ� 
            {
                isNextPressed = false; // �ϴ� �޽��� �ٰ�
                talkText.text = currentTalkText;
               // isSkip = true;//��ŵ�� �ߴٰ� ó���Ѵ�.
                break; //for �����
            }
            yield return new WaitForSecondsRealtime(talkSpeed);
        }

        yield return new WaitUntil(() => isNextPressed); //TRUE�϶����� ���

        isNextPressed = false;//���������� �и� true�� �������״ϱ�, false�� ����

        switch (currentTalkMove)
        {
            case -1://�������� �̵�
                StartGoTalk(TALK_CODE + 1);
                break;
            case -25: // ����
                SetTalkClose();
                break;
        }
    }
}
