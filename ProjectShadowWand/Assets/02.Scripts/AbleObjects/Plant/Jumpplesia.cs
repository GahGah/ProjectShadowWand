using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//밟으면 점프할 수 있는 오브젝트입니다.
public class Jumpplesia : Plant
{
    [HideInInspector]
    public PlayerController player;

    [Header("추가될 점프 파워~")]
    public float jumpPower;

    [Header("제한 각도")]
    [Range(0f, 180f)]
    public float limitAngle;
    [HideInInspector]
    public int playerMask;

    [Header("그 외")]
    public Transform myTransform;
    private float angleToPlayer;

    private bool isDrawGizmos;
    private void Start()
    {
        if (jumpPower == 0f)
        {
            jumpPower = 5f;
        }
        playerMask = LayerMask.NameToLayer("Player");
        //| (1 << LayerMask.NameToLayer("Default"));
        //  noPlayerMask = ~noPlayerMask;
        player = PlayerController.Instance;
        myTransform = GetComponent<Transform>();

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            angleToPlayer = Vector2.Angle(((Vector2)player.footPosition.position - (Vector2)myTransform.position), Vector2.up);

            if (angleToPlayer <= limitAngle)
            {
                player.SetExtraForce(new Vector2(0f, jumpPower));

            }
            else
            {
                PlayerController.Instance.SetExtraForce(Vector2.zero);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController.Instance.SetExtraForce(Vector2.zero);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Vector2 leftBoundary = DirFromAngle(-limitAngle / 2);
        Vector2 rightBoundary = DirFromAngle(limitAngle / 2);

        Gizmos.DrawLine(myTransform.position, (Vector2)myTransform.position + leftBoundary * 5f);
        Gizmos.DrawLine(myTransform.position, (Vector2)myTransform.position + rightBoundary * 5f);

    }


    public Vector2 DirFromAngle(float angleInDegrees)
    {
        //탱크의 좌우 회전값 갱신
        angleInDegrees += myTransform.eulerAngles.y;
        //경계 벡터값 반환
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    //public void DrawView()
    //{
    //    Vector2 leftBoundary = DirFromAngle(-limitAngle / 2);
    //    Vector2 rightBoundary = DirFromAngle(limitAngle / 2);
    //    Debug.DrawLine(myTransform.position, (Vector2)myTransform.position + leftBoundary * limitAngle, Color.blue);
    //    Debug.DrawLine(myTransform.position, (Vector2)myTransform.position + rightBoundary * limitAngle, Color.blue);
    //}
}
