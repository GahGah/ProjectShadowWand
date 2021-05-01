using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkSystemManager : MonoBehaviour
{

    [Tooltip("CSV로 불러온 파일의 내용이 담겨있는 딕셔너리 리스트입니다.")]
    public List<Dictionary<string, object>> talkData;

    [Tooltip("이름이 표시될 Text")]
    public Text nameText;

    [Tooltip("글자가 표시될 Text")]
    public Text talkText;

    public GameObject nextButton;

    [Tooltip("다음으로 가는 버튼을 눌렀는가?")]
    public bool isNextPressed;//다음으로 가는 버튼이 눌렸는가?

    [Tooltip("한 글자가 출력되는 속도입니다.")]
    public float talkSpeed;

    public Coroutine TalkCoroutine;

    private string filePath;

    [Tooltip("현재 캐릭터 이름")]
    private string currentCharName;

    [Tooltip("현재 토크 코드")]
    private int currentTalkCode;


    [Tooltip("현재 토크 내용.")]
    private string currentTalkText;

    [Tooltip("현재 토크 무브")]
    private int currentTalkMove;

    [Tooltip("현재 토크 페이스. 우선은 쓰이지 않습니다.")]
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
            Debug.Log("토크텍스트가 널");
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
        Debug.Log("불러오기 완료.");

        StartGoTalk(0);
    }

    /// <summary>
    /// Resources.Load를 이용하여 파일을 불러옵니다.
    /// </summary>
    /// <param name="path">불러올 파일 이름을 적어주시면 됩니다.</param>
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
    /// 대화를 종료시킵니다. 사실은 버튼, 캐릭터, 윈도우 등을 전부 비활성화 시킵니다. 또한,플레이어 컨트롤러의 isTalking을 false로 합니다.
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
    /// goNext를 트루로 설정합니다. goNext는 지정된 텍스트가 출력되면, GoTalk코루틴에서 자동으로 false가 됩니다.
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
    /// 한 대화(한 창에 나오는...)를 시작합니다.
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

            if (isNextPressed) //아직 텍스트가 다 나오지도 않았는데 다음 버튼이 눌렸다면 
            {
                isNextPressed = false; // 일단 펄스로 바고
                talkText.text = currentTalkText;
               // isSkip = true;//스킵을 했다고 처리한다.
                break; //for 벗어나기
            }
            yield return new WaitForSecondsRealtime(talkSpeed);
        }

        yield return new WaitUntil(() => isNextPressed); //TRUE일때까지 대기

        isNextPressed = false;//지나갔으면 분명 true인 상태일테니까, false로 변경

        switch (currentTalkMove)
        {
            case -1://다음으로 이동
                StartGoTalk(TALK_CODE + 1);
                break;
            case -25: // 종료
                SetTalkClose();
                break;
        }
    }
}
