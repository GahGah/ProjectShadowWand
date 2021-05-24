using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 매니저를 조작합니다.
/// </summary>
public class YeonchoolManager : Manager<YeonchoolManager>
{
    CameraManager cameraManager;
    StageDoor stageDoor;

    public UICutScene cutScene_Wind;
    public UICutScene cutScene_Water;
    public UICutScene cutScene_Lightning;

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


    public bool isCutscenePlaying;
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
            comebackSpeed = 7f;
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
        isCutscenePlaying = true;
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.canMove = false;
        }


        Debug.Log("연출매니저 스타트 컷씬");
        yield return new WaitForSeconds(1f);
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

        yield return StartCoroutine(tempCutscene.ProcessCutScene());

        if (tempCutscene.isLastFade == false)
        {
            while (tempCutscene.isNext == false)
            {
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }

        Time.timeScale = 1f;

        yield return StartCoroutine(tempCutscene.ProcessClose_Fade());


        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.canMove = true;
        }

        isCutscenePlaying = false;

    }
    public void StartStageInYeonchool()
    {
        StartCoroutine(GoStageDoorAndComebackTarget());
    }
    private IEnumerator GoStageDoorAndComebackTarget()
    {
        PlayerController.Instance.canMove = false;

        UIBlackScreen blackScreen = UIManager.Instance.uiDicitonary[eUItype.BLACKSCREEN] as UIBlackScreen;

        yield return new WaitForSeconds(1.5f);

        cameraManager.followSpeed = goSpeed;
        cameraManager.SetTarget(stageDoor.transform);
        cameraManager.FollowTarget(); //혹시 모르니까 불러주기


        while (cameraManager.isStop == false) //멈추기 전까지 기다림
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        yield return new WaitForSeconds(waitTime_go);

        //다 멈췄으면 까만색으로...
        blackScreen.SetFadeValue(0f, 1f, true);
        yield return StartCoroutine(blackScreen.GoFadeScreen());

        cameraManager.SetTarget(PlayerController.Instance.transform);

        var currentTransform = cameraManager.currentTransform;
        var target = cameraManager.target;

        cameraManager.followSpeed = comebackSpeed;

        currentTransform.position = target.position;
        cameraManager.FollowTarget(); //혹시 모르니까 불러주기




        cameraManager.followSpeed = 5f;

        yield return new WaitForSeconds(waitTime_comeback);

        //다 멈췄으면 까만거 치우기
        blackScreen.SetFadeValue(0f, 1f, false);

        yield return StartCoroutine(blackScreen.GoFadeScreen());

        PlayerController.Instance.canMove = true;
        //다 멈췄으면 끝...

    }
}
