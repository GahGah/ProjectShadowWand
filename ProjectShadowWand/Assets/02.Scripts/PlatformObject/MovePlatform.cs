using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MovableObject
{
    private PlayerController player;

    [Tooltip("움직일 플랫폼")]
    public Transform moveTransform;

    [Header("이동 속도")]
    public float moveSpeed;

    [Header("리프트의 종류")]
    [Tooltip("LEFTNRIGHT : 좌/우 이동을 하는 리프트. \b UPNDOWN  : 상/하 이동을 하는 리프트.")]
    public eLiftMoveType liftMoveType;


    [Header("첫번째 도착지")]
    public Transform firstPoint;
    [Header("두번째 도착지")]
    public Transform secondPoint;

    private Vector3 firstPosition = Vector3.zero;
    private Vector3 secondPosition = Vector3.zero;


    [Header("박스 콜라이더")]
    public BoxCollider2D boxCollider;

    public bool canMoving = false;


    [Header("작동 시 반복 이동 여부")]
    public bool isLoop = false;


    [Header("현재 목적지")]
    public eLiftState currentDestination;


    public bool isGoal = false;


    private Vector3 currentDestinationPosition;
    [Tooltip("커맨드스톤(레버)가 주는 이동 명령입니다.")]
    private eDirection commandDirection;


    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        player = PlayerController.Instance;
    }


    private void Init()
    {
        Init_Pos();

        if (myRigidbody == null)
        {
            myRigidbody = GetComponent<Rigidbody2D>();

        }

        movementSpeeds.x = moveSpeed;
        UpdateDestination(eLiftState.FIRST);
    }
    private void Init_Pos()
    {

        if (firstPoint == null)
        {
            LogError("해당 플랫폼의 퍼스트 포인트가 없습니다. 넣어주세요!!");
        }
        if (secondPoint == null)
        {
            LogError("해당 플랫폼의 세컨드 포인트가 없습니다. 넣어주세요!!");
        }
        if (moveTransform == null)
        {
            LogError("움직일 오브젝트가 없습니다. 넣어줘!!!");
        }

        firstPosition = firstPoint.position;
        secondPosition = secondPoint.position;

    }


    /// <summary>
    /// currentDes와 curentDesPos를 변경합니다.
    /// </summary>
    /// <param name="_ld">변경할 Destination</param>
    private void UpdateDestination(eLiftState _ld)
    {
        if (_ld == currentDestination)
        {
            return;
        }

        currentDestination = _ld;
        switch (currentDestination)
        {
            case eLiftState.FIRST:
                currentDestinationPosition = firstPosition;
                break;
            case eLiftState.SECOND:
                currentDestinationPosition = secondPosition;
                break;
            case eLiftState.STOP:
                currentDestinationPosition = moveTransform.position;
                break;

            default:
                break;
        }
    }



    private bool havePlayer = false;
    void FixedUpdate()
    {
        ProcessMove();

        if (player.groundHit && havePlayer == false)
        {
            if (player.groundHit.collider.gameObject == this.gameObject)
            {
                havePlayer = true;
                //player.gameObject.transform.SetParent(gameObject.transform);
                SetParents(player, this);
            }
            else
            {
  
                if (player.parentsObject == this)
                {
                    SetParents(player, null);
                    havePlayer = false;
                }

            }

        }
        else if (player.groundHit == false && havePlayer == true)
        {
            havePlayer = false;
            //player.gameObject.transform.SetParent(null);
            SetParents(player, null);

        }

    }

    private float distanceVal = 0.05f;
    private float currentDistance = 1f;
    /// <summary>
    /// endPositiion으로 향합니다. 되도록이면 FixedUpdate가 좋을 겁니다.
    /// </summary>
    public void ProcessMove()
    {

        if (!canMoving)
        {
            if (myRigidbody.velocity!=Vector2.zero)
            {
                SetMovement(eMovementType.SetVelocity,Vector2.zero);
            }
            return;

        }
        currentDistance = Vector2.Distance(myRigidbody.position, currentDestinationPosition);

        if (currentDistance <= distanceVal)
        {
            if (isLoop)
            {
                if (currentDestination == eLiftState.SECOND) //만약 목적지가 엔드 포지션이었다면
                {
                    UpdateDestination(eLiftState.FIRST);
                }
                else
                {
                    UpdateDestination(eLiftState.SECOND);
                }
            }
            else
            {
                isGoal = true;
                UpdateDestination(eLiftState.STOP);
            }
        }
        else
        {
            isGoal = false;
        }

        if (currentDestination != eLiftState.STOP)
        {

            SetMovement(eMovementType.SetVelocityDesiredPosition, currentDestinationPosition);
            //  moveTransform.position = Vector2.MoveTowards(moveTransform.position, currentDestinationPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            SetMovement(eMovementType.SetVelocity, Vector2.zero);
        }



    }


    /// <summary>
    /// 레버가 이동명령을할 때, 이동 가능한지를 먼저 체크해줍니다.
    /// </summary>
    /// <returns></returns>
    public bool TryCanMove(eDirection direction)
    {
        if (liftMoveType == eLiftMoveType.UPNDOWN)
        {
            if (direction == eDirection.UP || direction == eDirection.DOWN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (direction == eDirection.LEFT || direction == eDirection.RIGHT)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    /// <summary>
    /// 레버가 이동 명령을 실행할 때 쓰입니다.
    /// </summary>
    /// <param name="direction">이동하라고 명령하는 방향.</param>
    /// <returns></returns>
    public bool SetDirection(eDirection direction)
    {

        if (liftMoveType == eLiftMoveType.UPNDOWN) //상하일 경우
        {
            switch (direction)
            {
                case eDirection.UP:
                    UpdateDestination(eLiftState.FIRST);
                    return true;

                case eDirection.DOWN:
                    UpdateDestination(eLiftState.SECOND);
                    return true;

                default:
                    return false;
            }
        }
        else if (liftMoveType == eLiftMoveType.LEFTNRIGHT) // 좌우일 경우
        {
            switch (direction)
            {
                case eDirection.LEFT:
                    UpdateDestination(eLiftState.FIRST);
                    return true;

                case eDirection.RIGHT:
                    UpdateDestination(eLiftState.SECOND);
                    return true;

                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (PlayerController.Instance.isGrounded)
    //        {
    //            collision.gameObject.transform.SetParent(gameObject.transform);
    //        }
    //    }

    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        if (PlayerController.Instance.isGrounded)
    //        {
    //            collision.gameObject.transform.SetParent(gameObject.transform);
    //        }
    //        //else
    //        //{
    //        //    collision.gameObject.transform.SetParent(null);
    //        //}
    //    }

    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {

    //        collision.gameObject.transform.SetParent(null);

    //    }
    //}

    private void OnDrawGizmos()
    {
        //if (moveSelf)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(transform.position, endPosition);
        //}
        //else
        //{



        //    var trn = boxCollider.gameObject.transform;
        // var vec = new Vector3(trn.localScale.x, trn.localScale.y, trn.localScale.z);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(firstPoint.position, TestVectorPlus(boxCollider.size, boxCollider.offset));
        Gizmos.DrawSphere(firstPoint.position, 0.15f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(firstPoint.position, secondPoint.position);

        var dir = secondPoint.position - firstPoint.position;
        var dis = Vector2.Distance(firstPoint.position, secondPoint.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(firstPoint.position + dir.normalized * dis, boxCollider.size);
        Gizmos.DrawSphere(secondPoint.position, 0.15f);
        //}
    }

    private Vector3 TestVectorPlus(Vector3 _v1, Vector3 _v2)
    {
        return new Vector3(_v1.x + _v2.x, _v1.y + _v2.y, _v1.z + _v2.z);
    }
    private void LogError(string _string)
    {
        Debug.LogError(gameObject.name + " : " + _string);
    }

}
