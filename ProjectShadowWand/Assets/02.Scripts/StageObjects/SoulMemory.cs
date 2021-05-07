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
    private void Awake()
    {
        isTake = false;
        isEnd = false;
    }



    private void Start()
    {
        stageManager = StageManager.Instance;

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
        gameObject.SetActive(false);
        //stageManager.CheckClearCondition_SoulMemory();
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
}
