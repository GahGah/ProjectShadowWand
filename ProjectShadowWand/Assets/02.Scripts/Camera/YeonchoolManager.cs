using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶� �Ŵ����� �����մϴ�.
/// </summary>
public class YeonchoolManager : Manager<YeonchoolManager>
{
    CameraManager cameraManager;
    StageDoor stageDoor;

    public UICutScene cutScene_Wind;
    public UICutScene cutScene_Water;
    public UICutScene cutScene_Lightning;

    [Header("���帶ũ�� ���� ��")]
    [Tooltip("���帶ũ�� ���ϴ� �ӵ��Դϴ�.")]
    [Range(0f, 7f)]
    public float goSpeed;

    [Tooltip("ī�޶� ���帶ũ�� �����ϰ� ���� �ش� �ð����� ī�޶� �����ֽ��ϴ�.")]
    [Range(0f, 5f)]
    public float waitTime_go;

    [Header("�׹ٿ��� ���ƿ� ��")]

    [Tooltip("�÷��̾�� ���ƿ��� �ӵ��Դϴ�.")]
    [Range(0f, 7f)]
    public float comebackSpeed;

    [Tooltip("ī�޶� ���ƿ��� ���� �ش� �ð����� �÷��̾�� ������ �� �����ϴ�.")]
    [Range(0f, 5f)]
    public float waitTime_comeback;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        cameraManager = CameraManager.Instance;

        cameraManager.StartZoomMode();
        stageDoor = StageManager.Instance.stageDoor;


        if (goSpeed <= 0f)
        {
            goSpeed = 0.6f;
            waitTime_go = 2f;
            comebackSpeed = 0.6f;
            waitTime_comeback = 1f;
        }
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void FixedUpdate()
    {
        cameraManager.FollowTarget();
    }


    public IEnumerator StartCutscene(eCutsceneType _cutsceneType)
    {
        Debug.Log("����Ŵ��� ��ŸƮ �ƾ�");
        Time.timeScale = 0f;
        UICutScene tempCutscene = null;
        switch (_cutsceneType)
        {
            case eCutsceneType.INTRO:
                break;
            case eCutsceneType.UNLOCK_WIND:
                tempCutscene = cutScene_Wind;
                break;
            case eCutsceneType.UNLOCK_WATER:
                tempCutscene = cutScene_Water;
                break;
            case eCutsceneType.UNLOCK_LIGHTNING:
                tempCutscene = cutScene_Lightning;
                break;
            default:
                break;
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.canMove = false;
        }

        yield return StartCoroutine(tempCutscene.ProcessCutScene());

        Time.timeScale = 1f;

        yield return StartCoroutine(tempCutscene.ProcessClose_Fade());


        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.canMove = true;
        }

    }
    public void StartStageInYeonchool()
    {
        StartCoroutine(GoStageDoorAndComebackTarget());
    }
    private IEnumerator GoStageDoorAndComebackTarget()
    {
        PlayerController.Instance.canMove = false;

        yield return new WaitForSeconds(1.5f);

        cameraManager.followSpeed = goSpeed;
        cameraManager.SetTarget(stageDoor.transform);
        cameraManager.FollowTarget(); //Ȥ�� �𸣴ϱ� �ҷ��ֱ�


        while (cameraManager.isStop == false) //���߱� ������ ��ٸ�
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        yield return new WaitForSeconds(waitTime_go);

        //�� �������� �÷��̾�� �̵�
        cameraManager.SetTarget(PlayerController.Instance.transform);
        cameraManager.FollowTarget(); //Ȥ�� �𸣴ϱ� �ҷ��ֱ�

        cameraManager.followSpeed = comebackSpeed;

        while (cameraManager.isStop == false) //���߱� ������ ��ٸ�
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }


        cameraManager.followSpeed = 5f;

        yield return new WaitForSeconds(waitTime_comeback);

        PlayerController.Instance.canMove = true;
        //�� �������� ��...

    }
}
