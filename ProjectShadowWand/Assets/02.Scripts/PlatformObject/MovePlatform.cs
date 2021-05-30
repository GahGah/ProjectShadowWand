using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    private PlayerController player;

    [Tooltip("움직일 플랫폼")]
    public Transform moveTransform;

    [Header("이동 속도")]
    public float moveSpeed;



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

    private void Start()
    {
        player = PlayerController.Instance;
    }
    /// <summary>
    /// currentDes와 curentDesPos를 변경합니다.
    /// </summary>
    /// <param name="_ld">변경할 Destination</param>
    public void UpdateDestination(eLiftState _ld)
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

    private void Init()
    {
        Init_Pos();


        UpdateDestination(eLiftState.FIRST);
    }

    private void Awake()
    {
        Init();
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
                player.gameObject.transform.SetParent(gameObject.transform);
            }


        }
        else if (player.groundHit == false && havePlayer == true)
        {
            havePlayer = false;
            player.gameObject.transform.SetParent(null);

        }

    }

    private float distanceVal = 0.01f;
    /// <summary>
    /// endPositiion으로 향합니다. 되도록이면 FixedUpdate가 좋을 겁니다.
    /// </summary>
    public void ProcessMove()
    {

        if (!canMoving)
        {
            return;
        }

        if (Vector2.Distance(moveTransform.position, currentDestinationPosition) <= distanceVal)
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
            moveTransform.position = Vector2.MoveTowards(moveTransform.position, currentDestinationPosition, Time.deltaTime * moveSpeed);
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
        Gizmos.DrawWireCube(firstPoint.position, boxCollider.size);
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
    private void LogError(string _string)
    {
        Debug.LogError(gameObject.name + " : " + _string);
    }

}
