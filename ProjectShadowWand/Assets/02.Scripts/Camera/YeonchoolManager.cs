using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 매니저를 조작합니다.
/// </summary>
public class YeonchoolManager : MonoBehaviour
{

    CameraManager cameraManager;
    StageDoor stageDoor;


    [Header("랜드마크로 향할 때")]
    [Tooltip("랜드마크로 향하는 속도입니다.")]
    [Range(0f, 7f)]
    public float goSpeed;

    [Tooltip("카메라가 랜드마크에 도달하고 나서 해당 시간동안 카메라가 멈춰있습니다.")]
    [Range(0f, 5f)]
    public float waitTime_go;

    [Header("테바에게 돌아올 때")]

    [Tooltip("플레이어에게 돌아오는 속도입니다.")]
    [Range(0f, 7f)]
    public float comebackSpeed;

    [Tooltip("카메라가 돌아오고 나서 해당 시간동안 플레이어는 움직일 수 없습니다.")]
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
        cameraManager.FollowTarget(); //혹시 모르니까 불러주기

        PlayerController.Instance.canMove = false;
        while (cameraManager.isStop == false) //멈추기 전까지 기다림
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        yield return new WaitForSeconds(waitTime_go);

        //다 멈췄으면 플레이어에게 이동
        cameraManager.SetTarget(PlayerController.Instance.transform);
        cameraManager.FollowTarget(); //혹시 모르니까 불러주기

        cameraManager.followSpeed = comebackSpeed;

        while (cameraManager.isStop == false) //멈추기 전까지 기다림
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }


        cameraManager.followSpeed = 5f;

        yield return new WaitForSeconds(waitTime_comeback);

        PlayerController.Instance.canMove = true;
        //다 멈췄으면 끝...

    }
}
