using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 바람을 담당하는 컨트롤러
/// </summary>
public class WindController : MonoBehaviour
{
    [Header("바람의 영역")]
    public Collider2D windArea;

    [Header("파티클 전용 포스 필드"), Tooltip("파티클에게 영향을 줍니다.")]
    public ParticleSystemForceField forceField;

    [Header("바람의 방향"), Tooltip("현재 바람의 각도입니다.")]
    public eWindDirection windDirection;
    public eWindDirection originalWindDirection;

    [Header("오브젝트에 가해지는 속도")]
    public float windPower;

    private float forceDirection = 0.5f;
    private float originalForceDirection;

    [Header("오브젝트들의 최대 이동속도")]
    public float maxWindSpeed;

    [Header("바람의 영향을 받지 않는 레이어")]
    public LayerMask notAffectedLayer;
    public int noPlayerLayer;

    [Header("왼쪽 오른쪽 바람막이 콜라이더")]
    public GameObject leftColliderObject;
    public GameObject rightColliderObject;

    [HideInInspector]
    public EdgeCollider2D leftCollider;

    [HideInInspector]
    public EdgeCollider2D rightCollider;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        originalWindDirection = windDirection;
        originalForceDirection = forceField.directionX.constant;
        noPlayerLayer = ((1 << LayerMask.NameToLayer("Player") | (1 << LayerMask.NameToLayer("Ignore Raycast"))));
        noPlayerLayer = ~noPlayerLayer;
        //Debug.Log("Test");
        windArea = GetComponent<Collider2D>();
        //Debug.Log("Do Test Plz");

        SetWindDirection(windDirection);
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != notAffectedLayer.value && !collision.CompareTag("Player") && !collision.CompareTag("FireObject"))
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
                }

            }
            else if (windDirection == eWindDirection.RIGHT)
            {
                if (hit.collider == leftCollider)
                {
                    canMove = true;
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
            var finalDir = GetReverseDirection(windDirection) * 10000f;

            RaycastHit2D hit = Physics2D.Raycast(collision.gameObject.transform.position, finalDir, 10000f, noPlayerLayer);

            Debug.DrawRay(collision.gameObject.transform.position, GetReverseDirection(windDirection) * 10000f, Color.blue, 0.1f);

            //Debug.Log(hit.collider.name);
            //Debug.Log("hit : " + hit.collider.gameObject.name);
            bool canMove = false;

            if (windDirection == eWindDirection.LEFT)//왼쪽에서 불어오는 바람이라면
            {
                if (hit.collider == rightCollider)
                {
                    canMove = true;
                }

            }
            else if (windDirection == eWindDirection.RIGHT)
            {
                if (hit.collider == leftCollider)
                {
                    canMove = true;
                }
            }

            PlayerController player = PlayerController.Instance;
            if (canMove)
            {
                player.isWinding = true;
                player.windDirection = windDirection;

            }
            else
            {

                player.isWinding = false;

                player.windDirection = eWindDirection.NONE;

                //player.SetExtraForce(Vector2.zero);

                //player.animator.SetFloat("WindBlend", 0);
            }
        }

    }

    private void FixedUpdate()
    {
        if (windDirection != GetForceFieldDirection())
        {
            SetWindDirection(windDirection);
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

    private eWindDirection GetForceFieldDirection()
    {
        if (forceField.directionX.constant == forceDirection) //Right
        {
            return eWindDirection.RIGHT;
        }
        else if (forceField.directionX.constant == -forceDirection) //Left
        {
            return eWindDirection.LEFT;
        }
        else
        {
            return eWindDirection.NONE;

        }
    }
    public void SetWindDirection(eWindDirection _dir)
    {
        windDirection = _dir;

        switch (windDirection)
        {
            case eWindDirection.RIGHT:
                forceField.directionX = forceDirection;
                break;
            case eWindDirection.LEFT:
                forceField.directionX = -forceDirection;
                break;
            case eWindDirection.NONE:
                forceField.directionX = 0f;
                break;

            default:
                break;
        }

    }
}
