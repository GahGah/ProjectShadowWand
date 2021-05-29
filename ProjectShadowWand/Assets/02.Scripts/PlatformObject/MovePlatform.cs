using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [Header("트랜스폼")]

    [Tooltip("움직일 플랫폼")]
    public Transform moveTransform;

    [Tooltip("도착 위치")]
    public Transform endPoint;
    private Vector3 startPosition;
    private Vector3 endPosition = Vector3.zero;


    [Header("이동 속도")]
    public float moveSpeed;


    public bool canMoving = false;


    [Header("작동 시 반복 이동 여부")]
    public bool isLoop = false;


    [Header("현재 목적지")]
    [SerializeField]
    private eLiftState currentDestination;


    public bool isGoal = false;


    private Vector3 currentDestinationPosition;


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
                currentDestinationPosition = startPosition;
                break;
            case eLiftState.SECOND:
                currentDestinationPosition = endPosition;
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
        if (endPoint == null)
        {
            LogError("해당 플랫폼은 목적지가 없습니다. 넣어주세요!!");
        }
        if (moveTransform == null)
        {
            LogError("움직일 오브젝트가 없습니다. 넣어줘!!!");
        }
        startPosition = moveTransform.position;
        endPosition = endPoint.position;
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
    void FixedUpdate()
    {
        ProcessMove();

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

        if (currentDestination == eLiftState.STOP)
        {
            moveTransform.position = Vector2.MoveTowards(moveTransform.position, currentDestinationPosition, Time.deltaTime * moveSpeed);
        }




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.Instance.isGrounded)
            {
                collision.gameObject.transform.SetParent(gameObject.transform);
            }
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerController.Instance.isGrounded)
            {
                collision.gameObject.transform.SetParent(gameObject.transform);
            }
            else
            {
                collision.gameObject.transform.SetParent(null);
            }
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.transform.SetParent(null);

        }
    }

    private void OnDrawGizmosSelected()
    {
        //if (moveSelf)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(transform.position, endPosition);
        //}
        //else
        //{
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, endPoint.position);
        //}
    }
    private void LogError(string _string)
    {
        Debug.LogError(gameObject.name + " : " + _string);
    }

}
