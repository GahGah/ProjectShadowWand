using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;


    [Tooltip("��ȣ�ۿ� �� �� �ִ����� ���մϴ�.")]
    public bool isTake;

    [Tooltip("��ȭ ����� ������ ���� ���մϴ�.")]
    public bool isEnd;

    public int currentTalkCode;


    [Header("ȹ�� ���� ����")]

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("����� ȹ���� �� ����� �ö󰡴� �����Դϴ�.")]
    private float upLength;

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("����� ȹ���� �� ����� �ö󰡴� �ð��Դϴ�.")]
    private float upTime;

    [SerializeField]
    [Range(0f, 5f)]
    [Tooltip("����� ȹ���� �� ȭ���� ���̵� �Ǵ� �ð��Դϴ�.")]
    private float fadeTime;


    private float runningTime;


    [Header("�յ� ���ִ°� ����")]
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

        if (stageManager.soulMemoryList.Contains(this) == false) // ����Ʈ �ȿ� �ڱⰡ �ȵ��ִٸ�
        {
            stageManager.AddSoulMemory(this);
            Debug.LogError(gameObject.name + " : �������� �Ŵ��� ��� ����Ʈ�� ���� ���淡 �־����ϴ�. ������ ������������ ������ ���� �����ϱ� ���� ��������.");

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
    /// ȭ���� ����� ��, ��ȭâ�� ��ϴ�.
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
    /// �ҿ� �޸��� ����� ���۴ϴ�. �ϴ��� SetActivefalse.
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
    /// ����� ���� �ö󰩴ϴ�.
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
            if (ReferenceEquals(PlayerController.Instance.currentSoulMemory, null))// �ҿ�޸𸮰� ���϶���
            {
                PlayerController.Instance.currentSoulMemory = this; //�������� ����
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerController.Instance.currentSoulMemory == this)//�ҿ� �޸𸮰� �����϶���
            {
                PlayerController.Instance.currentSoulMemory = null; // null�� �ع�����
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, length));
    }
}
