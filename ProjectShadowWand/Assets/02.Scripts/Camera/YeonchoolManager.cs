using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ī�޶� �Ŵ����� �����մϴ�.
/// </summary>
public class YeonchoolManager : MonoBehaviour
{

    CameraManager cameraManager;
    StageDoor stageDoor;


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
    void Start()
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
        StartStageInYeonchool();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void FixedUpdate()
    {
        cameraManager.FollowTarget();
    }


    public void StartStageInYeonchool()
    {
        StartCoroutine(GoStageDoorAndComebackTarget());
    }
    private IEnumerator GoStageDoorAndComebackTarget()
    {
        cameraManager.followSpeed = goSpeed;
        cameraManager.SetTarget(stageDoor.transform);
        cameraManager.FollowTarget(); //Ȥ�� �𸣴ϱ� �ҷ��ֱ�

        PlayerController.Instance.canMove = false;
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
