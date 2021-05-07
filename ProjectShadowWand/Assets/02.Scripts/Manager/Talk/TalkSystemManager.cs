using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkSystemManager : MonoBehaviour
{
    [Tooltip("CSV로 불러온 대화 파일의 내용이 담겨있는 딕셔너리 리스트입니다.")]
    public List<Dictionary<string, object>> talkData;

    [Tooltip("CSV로 불러온 이름 파일의 내용이 담겨있는 딕셔너리 리스트입니다.")]
    public List<Dictionary<string, object>> charData;

    [Tooltip("CSV로 불러온 사념 파일의 내용이 담겨있는 딕셔너리 리스트입니다.")]
    public List<Dictionary<string, object>> soulMemoryData;

    [Header("이름")]
    [Tooltip("이름이 표시될 Text")]
    public Text nameText;

    [Header("대화 내용")]
    [Tooltip("글자가 표시될 Text")]
    public Text talkText;

    [Header("스킵 버튼")]
    public Button nextButton;

    [Tooltip("다음으로 가는 버튼을 눌렀는가?")]
    public bool isNextPressed;//다음으로 가는 버튼이 눌렸는가?

    [Tooltip("한 글자가 출력되는 속도입니다.")]
    public float talkSpeed;

    public Coroutine TalkCoroutine;
    public Coroutine ReadSoulMemoryCoroutine;

    //public

    private string filePath;

    [Tooltip("현재 캐릭터 코드")]
    private int currentCharCode;

    [Tooltip("현재 캐릭터 이름")]
    private string currentCharName;



    [Tooltip("현재 토크 코드")]
    private int currentTalkCode;


    [Tooltip("현재 토크 내용.")]
    private string currentTalkText;

    [Tooltip("현재 토크 무브")]
    private int currentTalkMove;

    //[Tooltip("현재 토크 페이스. 우선은 쓰이지 않습니다.")]
    //private int currentTalkFace;

    public Text spaceTest;


    [HideInInspector]
    public bool isTalkStart;
    [HideInInspector]
    public bool isTalkEnd;

    //public TalkStarter currentTalkStarter;


    public NPC currentTalkNPC;

    [Tooltip("현재 소울메모리")]
    private SoulMemory currentSoulMemory;

    [Space(20)]

    public GameObject talkUI;

    private static TalkSystemManager instance;
    public static TalkSystemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TalkSystemManager>();
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
        Init();
    }
    public void Init()
    {
        if (talkText == null)
        {
            Debug.Log("토크텍스트가 널");
        }
        //currentTalkStarter = null;
        currentTalkNPC = null;
        currentSoulMemory = null;
        isTalkEnd = false;
        isTalkStart = false;

    }

    void Start()
    {
        talkUI.SetActive(false);
        StartCoroutine(ProcessStart());
        spaceTest.text = "다음";
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
    /// <summary>
    /// 외부에서 호출하는 대화시작 함수입니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <param name="_npc"></param>
    public void StartGoTalk(int TALK_CODE, NPC _npc)
    {
        if (TalkCoroutine != null)
        {
            StopCoroutine(TalkCoroutine);
            TalkCoroutine = null;
        }
        Debug.Log("TalkSystemManager에서 StartGoTalk(외부호출대화함수) 실행.");
        QuestManager.Instance.QuestSystem_TalkStart(currentTalkNPC);
        TalkCoroutine = StartCoroutine(ProcessTalk(TALK_CODE, _npc));
    }

    /// <summary>
    /// 외부에서 호출하는 사념 읽기 시작 함수입니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <param name="_npc"></param>
    public void StartReadSoulMemory(int TALK_CODE, SoulMemory _soulMemory)
    {
        if (ReadSoulMemoryCoroutine != null)
        {
            StopCoroutine(ReadSoulMemoryCoroutine);
            ReadSoulMemoryCoroutine = null;
        }

        ReadSoulMemoryCoroutine = StartCoroutine(ProcessSoulMemory(TALK_CODE, _soulMemory));
    }



    /// <summary>
    /// 내부에서 사용하는 대화 시작 함수입니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <param name="_npc"></param>
    private void ProcessGoTalk(int TALK_CODE, NPC _npc)
    {
        //PlayerController.Instance.isTalking = true;

        if (TalkCoroutine != null)
        {
            StopCoroutine(TalkCoroutine);
            TalkCoroutine = null;
        }
        TalkCoroutine = StartCoroutine(ProcessTalk(TALK_CODE, _npc));
    }


    /// <summary>
    /// 내부에서 사용하는 소울메모리 읽기 시작 함수입니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <param name="_npc"></param>
    private void ProcessGoSoulMemory(int TALK_CODE, SoulMemory _soulMemory)
    {
        //PlayerController.Instance.isTalking = true;

        if (ReadSoulMemoryCoroutine != null)
        {
            StopCoroutine(ReadSoulMemoryCoroutine);


            ReadSoulMemoryCoroutine = null;
        }
        ReadSoulMemoryCoroutine = StartCoroutine(ProcessSoulMemory(TALK_CODE, _soulMemory));
    }


    public IEnumerator ProcessStart()
    {
        yield return StartCoroutine(GoReadCharData("CharData"));
        yield return StartCoroutine(GoReadTalkData("TalkData_Stage_00"));

        yield return StartCoroutine(GoReadSoulMemoryData("SoulMemoryData_Stage_01"));
    }

    /// <summary>
    /// Resources.Load를 이용하여 대화 파일을 불러옵니다.
    /// </summary>
    /// <param name="path">불러올 파일 이름을 적어주시면 됩니다.</param>
    public IEnumerator GoReadTalkData(string path)
    {
        filePath = "DataFiles/TalkData/" + path;
        talkData = CsvReader.Read(filePath);
        Debug.Log(path + "을 불러왔습니다.");
        yield return null;
    }

    /// <summary>
    ///  Resources.Load를 이용하여 캐릭터 이름 파일을 불러옵니다.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerator GoReadCharData(string path)
    {
        filePath = "DataFiles/TalkData/" + path;
        charData = CsvReader.Read(filePath);
        Debug.Log(path + "을 불러왔습니다.");
        yield return null;
    }

    /// <summary>
    ///  Resources.Load를 이용하여 사념 파일을 불러옵니다.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public IEnumerator GoReadSoulMemoryData(string path)
    {
        filePath = "DataFiles/SoulMemoryData/" + path;
        soulMemoryData = CsvReader.Read(filePath);
        Debug.Log(path + "을 불러왔습니다.");
        yield return null;
    }


    /// <summary>
    /// 대화를 종료시킵니다. 사실은 버튼, 캐릭터, 윈도우 등을 전부 비활성화 시킵니다. 또한,플레이어 컨트롤러의 isTalking을 false로 합니다.
    /// </summary>
    public void SetTalkClose()
    {
        talkUI.SetActive(false);
        if (TalkCoroutine != null)
        {
            StopCoroutine(TalkCoroutine);
            TalkCoroutine = null;
        }
        //isTalkEnd = true;
        //isTalkStart = false;

        //if (currentTalkStarter != null)
        //{
        //    currentTalkStarter.isEnd = true;
        //    currentTalkStarter.isStart = false;
        //    currentTalkStarter = null;
        //}
        if (currentTalkNPC != null)
        {
            QuestManager.Instance.QuestSystem_TalkEnd(currentTalkNPC);
        }

        PlayerController.Instance.isTalking = false;
        currentTalkNPC = null;
    }


    public void SetSoulMemoryClose()
    {
        //talkUI.SetActive(false);
        if (ReadSoulMemoryCoroutine != null)
        {
            StopCoroutine(ReadSoulMemoryCoroutine);
            ReadSoulMemoryCoroutine = null;
        }

        if (currentSoulMemory != null)
        {
            //TODO : UI에 추가라던가 그런걸 해야함...혹은 그냥 end만 하던가?
            currentSoulMemory.isEnd = true;
        }

        PlayerController.Instance.isInteractingSoulMemory = false;
        currentSoulMemory.DisappearSoulMemory();
        currentSoulMemory = null;
        StageManager.Instance.CheckClearCondition_SoulMemory();
    }
    /// <summary>
    /// goNext를 트루로 설정합니다. goNext는 지정된 텍스트가 출력되면, GoTalk코루틴에서 자동으로 false가 됩니다.
    /// </summary>
    public void SetTrueGoNext()
    {
        isNextPressed = true;

    }


    /// <summary>
    /// 한 대화(한 창에 나오는...)를 시작합니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <returns></returns>
    IEnumerator ProcessTalk(int TALK_CODE, NPC _npc)
    //int TALK_CODE_START, int TALK_CODE_END)
    {
        talkUI.SetActive(true);
        //isTalkStart = true;
        //isTalkEnd = false;

        currentTalkNPC = _npc;


        //if (currentTalkStarter != null)
        //{
        //    currentTalkStarter.isStart = true;
        //}

        PlayerController.Instance.isTalking = true;
        isNextPressed = false;

        currentTalkCode = (int)talkData[TALK_CODE]["TALK_CODE"];
        currentCharCode = (int)talkData[TALK_CODE]["TALK_CHAR_NAME"];
        currentTalkMove = (int)talkData[TALK_CODE]["TALK_MOVE"];

        //currentTalkFace = (int)talkData[TALK_CODE]["TALK_FACE"];

        currentCharName = charData[currentCharCode]["CHAR_NAME"] as string;
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
                ProcessGoTalk(TALK_CODE + 1, currentTalkNPC);
                break;
            case -25: // 종료
                SetTalkClose();
                break;
        }
    }
    /// <summary>
    /// 소울 메모리 데이터를 읽습니다.
    /// </summary>
    /// <param name="TALK_CODE"></param>
    /// <param name="_soulMemory"></param>
    /// <returns></returns>
    IEnumerator ProcessSoulMemory(int TALK_CODE, SoulMemory _soulMemory)
    {
        talkUI.SetActive(true);

        currentSoulMemory = _soulMemory;

        PlayerController.Instance.isInteractingSoulMemory = true;
        isNextPressed = false;

        currentTalkCode = (int)soulMemoryData[TALK_CODE]["TALK_CODE"];
        currentCharCode = (int)soulMemoryData[TALK_CODE]["TALK_CHAR_NAME"];
        currentTalkMove = (int)soulMemoryData[TALK_CODE]["TALK_MOVE"];

        //currentTalkFace = (int)talkData[TALK_CODE]["TALK_FACE"];

        currentCharName = charData[currentCharCode]["CHAR_NAME"] as string;
        currentTalkText = soulMemoryData[TALK_CODE]["TALK_NAEYONG"] as string;

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
                ProcessGoSoulMemory(TALK_CODE + 1, currentSoulMemory);
                break;
            case -25: // 종료
                SetSoulMemoryClose();
                break;
        }
    }







}
