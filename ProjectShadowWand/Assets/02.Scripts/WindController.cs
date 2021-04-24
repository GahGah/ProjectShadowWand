using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 바람을 담당하는 컨트롤러
/// </summary>
public class WindController : MonoBehaviour
{
    [Header("바람 영역")]
    public Collider2D windArea;

    [Header("바람의 방향")]
    public eWindDirection windDirection;

    [Header("바람 세기")]
    public float windPower;

    [Header("오브젝트들의 최대 이동속도")]
    public float maxWindSpeed;

    [Header("바람의 영향을 받지 않는 레이어")]
    public LayerMask notAffectedLayer;

    public GameObject leftColliderObject;
    public GameObject rightColliderObject;

    public EdgeCollider2D leftCollider;
    public EdgeCollider2D rightCollider;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Debug.Log("Test");
        windArea = GetComponent<Collider2D>();
        Debug.Log("Do Test Plz");

        CreateEdgeCollider();
    }
    public void CreateEdgeCollider()
    {


        leftCollider = leftColliderObject.gameObject.AddComponent<EdgeCollider2D>();
        rightCollider = rightColliderObject.gameObject.AddComponent<EdgeCollider2D>();

        //leftCollider.offset = new Vector2(-windArea.bounds.extents.x, windArea.bounds.extents.y);
        //rightCollider.offset = new Vector2(windArea.bounds.extents.x, windArea.bounds.center.y);

        var leftTop = new Vector2(-windArea.bounds.extents.x, windArea.bounds.extents.y);
        var leftBottom = new Vector2(-windArea.bounds.extents.x, -windArea.bounds.extents.y);

        var rightTop = new Vector2(windArea.bounds.extents.x, windArea.bounds.extents.y);
        var rightBottom = new Vector2(windArea.bounds.extents.x, -windArea.bounds.extents.y);

        leftCollider.points = new Vector2[2]
        {
            leftTop,leftBottom
        };

        rightCollider.points = new Vector2[2]
        {
            rightTop,rightBottom
        };
        leftCollider.offset -= Vector2.left;
        rightCollider.offset -= Vector2.right;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != notAffectedLayer.value)
        {

            var finalDir = GetReverseDirection(windDirection) * 10000f;
            RaycastHit2D hit = Physics2D.Raycast(collision.gameObject.transform.position, finalDir, 10000f);
            Debug.DrawRay(collision.gameObject.transform.position, GetReverseDirection(windDirection) * 10000f, Color.red, 0.1f);

            //Debug.Log("hit : " + hit.collider.gameObject.name);
            bool canMove = false;

            if (windDirection == eWindDirection.LEFT)//왼쪽에서 불어오는 바람이라면
            {
                if (hit.collider == rightCollider)
                {
                    canMove = true;
                    Debug.Log("Right True");
                }
                
            }
            else if (windDirection == eWindDirection.RIGHT)
            {
                if (hit.collider == leftCollider)
                {
                    canMove = true;
                    Debug.Log("Left True");
                }
            }

            if (canMove)
            {
                collision.attachedRigidbody.AddForce(GetDirection(windDirection) * windPower, ForceMode2D.Force);
                collision.attachedRigidbody.velocity =
                    new Vector2(Mathf.Clamp(collision.attachedRigidbody.velocity.x, -maxWindSpeed, maxWindSpeed),
                                Mathf.Clamp(collision.attachedRigidbody.velocity.y, -maxWindSpeed, maxWindSpeed));
            }


        }
        else if (collision.CompareTag("Player"))
        {
            //뭔가 하자
        }
    }
    private Vector2 GetReverseDirection(eWindDirection _dir)
    {
        if (_dir == eWindDirection.LEFT)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }

    }
    private Vector2 GetDirection(eWindDirection _dir)
    {
        if (_dir == eWindDirection.LEFT)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }
}
