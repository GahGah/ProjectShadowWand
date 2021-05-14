using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;


    [Tooltip("상호작용 한 적 있는지를 뜻합니다.")]
    public bool isTake;

    [Tooltip("대화 출력이 끝났는 가를 뜻합니다.")]
    public bool isEnd;

    public int currentTalkCode;


    [Header("획득 연출 관련")]

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("사념을 획득한 뒤 사념이 올라가는 정도입니다.")]
    private float upLength;

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("사념을 획득한 뒤 사념이 올라가는 시간입니다.")]
    private float upTime;

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("사념을 획득한 뒤 화면이 페이드 되는 시간입니다.")]
    private float fadeTime;


    private float runningTime;


    [Header("둥둥 떠있는거 관련")]
    [SerializeField]
    [Range(0f, 5f)]
    private float upAndDownSpeed;

    [SerializeField]
    [Range(0f, 1f)]
    private float length;
    private float yPos;

    private Transform myTransform;
    private float originalYpos;




    private UIBlackScreen blackScreen;
    private void Awake()
    {
        isTake = false;
        isEnd = false;
    }



    private void Start()
    {

        Init();
    }

    public void Init()
    {
        myTransform = gameObject.transform;
        originalYpos = myTransform.position.y;
        stageManager = StageManager.Instance;
        yPos = originalYpos;

        if (upTime <= 0f)
        {
            upTime = 2f;
        }

        if (upLength <= 0f)
        {
            upLength = 1f;
        }

        if (stageManager.soulMemoryList.Contains(this) == false) // 리스트 안에 자기가 안들어가있다면
        {
            stageManager.AddSoulMemory(this);
            Debug.LogError(gameObject.name + " : 스테이지 매니저 사념 리스트에 제가 없길래 넣었습니다. 문제는 없어졌겠지만 오류날 수도 있으니까 직접 넣으세요.");

        }
    }

    private void Update()
    {
        UpdateUpAndDownPosition();
    }
    public void TakeSoulMemory()
    {
        isTake = true;

        PlayerController.Instance.isInteractingSoulMemory = true;

        StartCoroutine(ProcessTakeSoulMemory());
        //gameObject.SetActive(false);
        //isEnd = true;
        //stageManager.CheckClearCondition_SoulMemory();
    }
    /// <summary>
    /// 화면이 까매진 후, 대화창이 뜹니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ProcessTakeSoulMemory()
    {
        yield return null;

        blackScreen = UIManager.Instance.uiDicitonary[eUItype.BLACKSCREEN] as UIBlackScreen;
        blackScreen.SetFadeValue(0f, fadeTime, true);
        yield return StartCoroutine(blackScreen.GoFadeScreen());

        TalkSystemManager.Instance.StartReadSoulMemory(currentTalkCode, this);
    }

    /// <summary>
    /// 소울 메모리의 모습을 없앱니다. 일단은 SetActivefalse.
    /// </summary>
    public void DisappearSoulMemory()
    {
        StartCoroutine(ProcessDisappearSoulMemory());
        //stageManager.CheckClearCondition_SoulMemory();
    }

    private IEnumerator ProcessDisappearSoulMemory()
    {
        yield return null;

        blackScreen = UIManager.Instance.uiDicitonary[eUItype.BLACKSCREEN] as UIBlackScreen;

        blackScreen.SetFadeValue(0f, fadeTime, false);
        yield return StartCoroutine(blackScreen.GoFadeScreen());

        isEnd = true;

        yield return StartCoroutine(MoveUpSoulMemory());


        gameObject.SetActive(false);
        PlayerController.Instance.isInteractingSoulMemory = false;
        StageManager.Instance.CheckClearCondition_SoulMemory();
    }

    /// <summary>
    /// 사념이 위로 올라갑니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveUpSoulMemory()
    {
        float upPos = originalYpos + upLength;
        float timer = 0f;
        float progress = 0f;

        float xPos = myTransform.position.x;
        float yPos = myTransform.position.y;
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / upTime;

            myTransform.position = new Vector2(xPos, Mathf.Lerp(yPos, upPos, progress));
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }
    }


    public void UpdateUpAndDownPosition()
    {
        if (isEnd)
        {
            return;
        }
        //        runningTime += Time.deltaTime * upAndDownSpeed;

        //      yPos = Mathf.Sin(runningTime) * originalYpos * length;

        yPos = Mathf.Sin(Time.time * upAndDownSpeed) * length;
        myTransform.position = new Vector2(myTransform.position.x, originalYpos + yPos);

        if (runningTime > 10000f)
        {
            runningTime = 0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ReferenceEquals(PlayerController.Instance.currentSoulMemory, null))// 소울메모리가 널일때만
            {
                PlayerController.Instance.currentSoulMemory = this; //본인으로 설정
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.currentSoulMemory == this)//소울 메모리가 본인일때만
            {
                PlayerController.Instance.currentSoulMemory = null; // null로 해버리기
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, length));
    }
}
