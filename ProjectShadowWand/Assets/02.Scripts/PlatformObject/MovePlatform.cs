using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public bool canMoving = false;
    public bool isLoop;
    public bool moveSelf;

    public float moveSpeed;

    public Transform endPoint;
    private Vector3 startPosition;
    private Vector3 endPosition = Vector3.zero;

    private Vector3 currentDestination;

    public bool isGoal;
    void Start()
    {
        if (moveSelf)
        {
            Init();
        }

    }



    public void Init()
    {
        startPosition = transform.position;
        endPosition = endPoint.position;
        isGoal = false;
        //if (moveSelf)
        //{
        currentDestination = endPosition;
        //}
    }
    void FixedUpdate()
    {
        if (canMoving && moveSelf)
        {
            if (isLoop == false)
            {
                if (!isGoal)
                {
                    ProcessMove();
                }

            }
            else
            {
                ProcessMove();
            }

        }
    }
    public void SetDestination(Vector3 _pos)
    {
        currentDestination = _pos;
    }

    /// <summary>
    /// 목적지가 스타트포지션일경우 엔드 포지션으로, 엔드 포지션일 경우 스타트 포지션으로 변경시킵니다.
    /// </summary>
    public void ToggleDestination()
    {
        if (currentDestination == startPosition)
        {
            currentDestination = endPosition;
        }
        else
        {
            currentDestination = startPosition;
        }
        isGoal = false;
    }
    /// <summary>
    /// endPositiion으로 향합니다. 되도록이면 FixedUpdate가 좋을 겁니다.
    /// </summary>
    public void ProcessMove()
    {
        if (canMoving)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentDestination, Time.fixedDeltaTime * moveSpeed);
        }

        if (Vector2.Distance(gameObject.transform.position, currentDestination) <= 1f) //거리가 0.01라면
        {
            if (isLoop)
            {
                if (currentDestination == endPosition) //만약 목적지가 엔드 포지션이었다면
                {
                    currentDestination = startPosition;
                }
                else
                {
                    currentDestination = endPosition;
                }
            }
            else
            {
                isGoal = true;
            }
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

}
