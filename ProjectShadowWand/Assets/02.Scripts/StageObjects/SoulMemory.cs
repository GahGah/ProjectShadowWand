using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMemory : MonoBehaviour
{
    private StageManager stageManager;


    [Tooltip("��ȣ�ۿ� �� �� �ִ����� ���մϴ�.")]
    public bool isTake;

    [Tooltip("isEnd�� true�� ��� UI�� ����� �߰��˴ϴ�.")]
    public bool isEnd;

    public int currentTalkCode;

    private float runningTime;
    [SerializeField]
    [Range(0f, 5f)]
    private float upAndDownSpeed;

    [SerializeField]
    [Range(0f, 1f)]
    private float length;
    private float yPos;

    private Transform myTransform;
    private float originalYpos;

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

        if (stageManager.soulMemoryList.Contains(this) == false) // ����Ʈ �ȿ� �ڱⰡ �ȵ��ִٸ�
        {
            stageManager.AddSoulMemory(this);
            Debug.LogError(gameObject.name + " : �������� �Ŵ��� ��� ����Ʈ�� ���� ���淡 �־����ϴ�. ������ ������������ ������ ���� �����ϱ� ���� ��������.");

        }
    }
    public void TakeSoulMemory()
    {
        isTake = true;

        TalkSystemManager.Instance.StartReadSoulMemory(currentTalkCode, this);
        //gameObject.SetActive(false);
        //isEnd = true;
        //stageManager.CheckClearCondition_SoulMemory();
    }

    /// <summary>
    /// �ҿ� �޸��� ����� ���۴ϴ�. �ϴ��� SetActivefalse.
    /// </summary>
    public void DisappearSoulMemory()
    {
        isEnd = true;
        StageManager.Instance.CheckClearCondition_SoulMemory();
        gameObject.SetActive(false);
        //stageManager.CheckClearCondition_SoulMemory();
    }

    private void Update()
    {
        UpdateUpAndDownPosition();
    }
    public void UpdateUpAndDownPosition()
    {
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
