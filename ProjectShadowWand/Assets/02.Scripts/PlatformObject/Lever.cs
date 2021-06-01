using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableObject
{

    private Transform myTransform;

    [Header("명령 대기 시간")]
    [Tooltip("회전한 후, 해당 시간만큼 대기했다가 명령을 합니다.(움직일 수 있는 플랫폼이 있을 경우)")]
    public float waitCommandTime_On;

    [Tooltip("회전한 후, 해당 시간만큼 대기했다가 명령을 합니다.(움직일 수 있는 플랫폼이 없을 경우)")]
    public float waitCommandTime_Stop;

    private float waitCommandTime;
    [Header("연결된 플랫폼")]
    [Space(20)]
    public MovePlatform[] movePlatforms;



    [Header("현재 명령하고 있는 방향")]
    [Tooltip("현재 명령하고 있는 방향")]
    public eDirection currentDirection;


    [Tooltip("나머지 연산을 할 때 쓰입니다.")]
    private int directionValue;

    [Header("그 외")]

    [Tooltip("레버가 플랫폼에 하나라도 관여하고 있는 상태인지를 뜻합니다.")]
    public bool isOn;

    //  public int count;

    private List<MovePlatform> pList_LeftAndRight;
    private List<MovePlatform> pList_UpAndDown;

    private List<MovePlatform> currentPlatformList;

    float timer = 0f;
    public float progress = 0f;

    Quaternion beginRotation;
    Quaternion goRotation;

    private float originalYpos;

    private void Awake()
    {
        Init_Awake();
    }
    private void Init_Awake()
    {
        waitCommandTimer = 0f;
        waitCommandTime = waitCommandTime_On;
        myTransform = gameObject.transform;
        //   count = movePlatforms.Length;
        currentDirection = eDirection.UP;
        directionValue = (int)currentDirection;
        originalYpos = myTransform.position.y;
    }
    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        Init_PlatformList();

        player = PlayerController.Instance;
    }

    private void Init_PlatformList()
    {
        int arrayCount = movePlatforms.Length;

        pList_LeftAndRight = new List<MovePlatform>();
        pList_UpAndDown = new List<MovePlatform>();
        for (int i = 0; i < arrayCount; i++)
        {
            if (movePlatforms[i].liftMoveType == eLiftMoveType.UPNDOWN)
            {
                pList_UpAndDown.Add(movePlatforms[i]);
            }
            else if (movePlatforms[i].liftMoveType == eLiftMoveType.LEFTNRIGHT)
            {
                pList_LeftAndRight.Add(movePlatforms[i]);
            }
            else { }
        }
    }


    private void Update()
    {
        UpdateUpAndDownPosition();
    }

    public void SetIsOn(bool _b)
    {
        Debug.Log(gameObject.name + " : SetIsOn -" + _b);
        isOn = _b;
    }

    /// <summary>
    /// 플랫폼들의 SetDirection 함수를 호출합니다.
    /// </summary>
    private void UpdatePlatformDirection()
    {
        UpdateCurrentPlatformList();

        for (int i = 0; i < movePlatforms.Length; i++)
        {
            movePlatforms[i].canMoving = movePlatforms[i].TryCanMove(currentDirection);
        }
        for (int i = 0; i < currentPlatformList.Count; i++)
        {
            currentPlatformList[i].SetDirection(currentDirection);
        }

    }

    public override void DoInteract()
    {

        Debug.Log("DoInteract");
        canInteract = false;
        player.isInteracting = true;

        directionValue += 1;
        if (directionValue == 5)
        {
            directionValue = 1;
        }

        int tempVal = directionValue % 4;

        currentDirection = (eDirection)tempVal;
        Debug.Log("방향 : " + currentDirection);

        StartCoroutine(ProcessInteract());
    }


    float rotateTime = 1f;
    float speed = 3f;
    private IEnumerator ProcessInteract()
    {
        goRotation = Quaternion.Euler(0f, 0f, GetDirectionRotation());
        beginRotation = myTransform.rotation;
        timer = 0f;
        progress = 0f;
        while (progress < 1f)
        {
            timer += Time.deltaTime * speed;

            progress = timer / rotateTime;

            myTransform.rotation = Quaternion.Lerp(beginRotation, goRotation, progress);
            yield return null;
        }

        canInteract = true;


        if (directionTimerCoroutine == null)// null이라면 
        {
            waitCommandTimer = 0f;
            directionTimerCoroutine = SetDirectionTimer();
            StartCoroutine(directionTimerCoroutine);
        }
        else
        { //시간만 초기화
            waitCommandTimer = 0f;

        }
    }


    private IEnumerator directionTimerCoroutine;
    public float waitCommandTimer;
    private IEnumerator SetDirectionTimer()
    {
        while (waitCommandTimer < waitCommandTime)
        {
            UpdateCurrentPlatformList();
            UpdateWaitComandTime();
            waitCommandTimer += Time.deltaTime;
            yield return null;
        }

        UpdatePlatformDirection();



        if (currentPlatformList.Count != 0)
        {
            UpdateIsOn(true);
        }
        else
        {
            UpdateIsOn(false);
        }


        directionTimerCoroutine = null;
    }

    private void UpdateWaitComandTime()
    {
        if (currentPlatformList.Count != 0)
        {
            waitCommandTime = waitCommandTime_On;
            //  UpdateIsOn(true);
        }
        else
        {
            waitCommandTime = waitCommandTime_Stop;
            //UpdateIsOn(false);
        }


    }

    /// <summary>
    /// isOn과 애니메이션을 변경시킵니다.
    /// </summary>
    /// <param name="_b"></param>
    private void UpdateIsOn(bool _b)
    {
        if (isOn == _b) //현재 isOn과 _b가 다를 경우에만
        {
            return;
        }
        isOn = _b;
    }
    private float GetDirectionRotation()
    {
        switch (currentDirection)
        {
            case eDirection.UP:
                return 0f;

            case eDirection.RIGHT:
                return -90f;

            case eDirection.DOWN:
                return -180f;

            case eDirection.LEFT:
                return -270f;

            default:
                return 0f;
        }
    }



    /// <summary>
    /// currentPlatformList에 두 리스트 중 하나를 복사합니다.
    /// </summary>
    private void UpdateCurrentPlatformList()
    {
        if (currentDirection == eDirection.UP || currentDirection == eDirection.DOWN)
        {
            currentPlatformList = pList_UpAndDown;
        }
        else //(currentDirection == eDirection.LEFT || currentDirection == eDirection.RIGHT)
        {
            currentPlatformList = pList_LeftAndRight;
        }
    }

    private bool isAllStop = false;
    ///// <summary>
    ///// 플랫폼의 정지 상태에 따라 isOn를 변경합니다.
    ///// </summary>
    //private void UpdateIsOn()
    //{
    //    isAllStop = true;
    //    UpdateCurrentPlatformList();

    //    for (int i = 0; i < currentPlatformList.Count; i++)
    //    {
    //        if (currentPlatformList[i].currentDestination != eLiftState.STOP)
    //        {
    //            isAllStop = false;
    //        }

    //    }


    //    isOn = !isAllStop;
    //}

    private float onTimer = 0f;
    private float offTimer = 0f;
    private float tempYpos = 0f;
    private float offProgress = 0f;

    private float udSpeed = 2f;
    private float udLength = 0.05f;
    public void UpdateUpAndDownPosition()
    {

        if (isOn)
        {
            onTimer += Time.deltaTime;
            tempYpos = Mathf.Sin(onTimer * udSpeed) * udLength;
            myTransform.position = new Vector3(myTransform.position.x, originalYpos + tempYpos, myTransform.position.z);

        }
        //else
        //{
        //    offTimer += Time.deltaTime;
        //    tempYpos = Mathf.Lerp(myTransform.position.y, originalYpos - udLength, Time.deltaTime);
        //    myTransform.position = new Vector3(myTransform.position.x, originalYpos - tempYpos, myTransform.position.z);
        //}



        //runningTime = Time.time;
        //yPos = Mathf.Sin(runningTime * upAndDownSpeed) * length;
        //myTransform.position = new Vector2(myTransform.position.x, originalYpos + yPos);

        //if (runningTime > 10000f)
        //{
        //    runningTime = 0f;
        //}


        //        runningTime += Time.deltaTime * upAndDownSpeed;

        //      yPos = Mathf.Sin(runningTime) * originalYpos * length;


    }


    public override void SetTouchedObject(bool _b)
    {
        base.SetTouchedObject(_b);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagPlayer))
        {
            SetTouchedObject(false);
        }

    }

}
