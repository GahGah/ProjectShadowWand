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
    /// �������� ��ŸƮ�������ϰ�� ���� ����������, ���� �������� ��� ��ŸƮ ���������� �����ŵ�ϴ�.
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
    /// endPositiion���� ���մϴ�. �ǵ����̸� FixedUpdate�� ���� �̴ϴ�.
    /// </summary>
    public void ProcessMove()
    {
        if (canMoving)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentDestination, Time.fixedDeltaTime * moveSpeed);
        }

        if (Vector2.Distance(gameObject.transform.position, currentDestination) <= 1f) //�Ÿ��� 0.01���
        {
            if (isLoop)
            {
                if (currentDestination == endPosition) //���� �������� ���� �������̾��ٸ�
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
