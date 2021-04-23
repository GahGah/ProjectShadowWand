using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FixedJoint2D))]
public class TestCatchingObject : WeatherInteractionObject, ICatchable
{
    [Space(10)]
    public Collider2D currentCollider;
    [Space(10)]
    public FixedJoint2D fixedJoint;
    public Rigidbody2D rigidBody;

    public bool autoAnchorBool;

    [Tooltip("이 오브젝트가 플레이어와 닿아있는 상태인지를 체크합니다.")]
    public bool isTouched;

    [Tooltip("이 오브젝트가 잡혀있는 상태인지 체크합니다.")]
    public bool isCatched;

    private Rigidbody2D rb;
    void Start()
    {
        Init();

    }



    public override void Init()
    {
        rb = PlayerController.Instance.catchBody;
        if (currentCollider == null)
        {
            Log("커런트 콜라이더가 없음. 일단 자동으로 넣어볼게");

            var _tempColls = GetComponents<Collider2D>();


            for (int i = 0; i < _tempColls.Length; i++)
            {
                var _tempColl = _tempColls[i];

                if (_tempColl.isTrigger == false)
                {
                    Log("자동으로 콜라이더 넣기 성공. ");
                    currentCollider = _tempColl;
                    break;
                }
            }
        }
        if (fixedJoint == null)
        {
            fixedJoint = GetComponent<FixedJoint2D>();
        }

        if (rigidBody == null)
        {
            GetComponent<Rigidbody2D>();
        }
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.enabled = false;

    }

    private void Update()
    {
        isTouched = (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint);

        if (PlayerController.Instance.isInputCatchKey)
        {
            if (PlayerController.Instance.GetCatchingObject() == null)
            {
                if (isTouched)
                {
                    if (PlayerController.Instance.GetCatchingObject() != gameObject) // 이 오브젝트를 잡고있지 않다면
                    {
                        GoCatchThis();
                    }
                }

            }
            else if (PlayerController.Instance.GetCatchingObject() == gameObject) // 이 오브젝트가 잡혀있다면
            {

                GoPutThis();


            }


        }

    }
    public void GoCatchThis()
    {
        //Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider, false);

        SetConnectedBody(rb);
        SetConnectedAnchor(PlayerController.Instance.catchPosition_Lift.transform.localPosition);
        SetAutoAnchor(false);

        PlayerController.Instance.SetCatchingObject(gameObject);
        fixedJoint.enabled = true;

        //플레이어와 충돌하게...안해도 될 것 같은데 일단 하기

        Log("잡았습니다.");
    }

    public void GoPutThis()
    {
        //플레이어와 충돌하지 않게 한다.
        //Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider);

        SetConnectedBody(null);
        SetConnectedAnchor(Vector2.zero); ;
        SetAutoAnchor(false);

        PlayerController.Instance.SetCatchingObject(null);

        fixedJoint.enabled = false;


        Log("놓아봅시다.");

    }
    public void SetConnectedBody(Rigidbody2D _rb)
    {
        fixedJoint.connectedBody = _rb;
    }
    public void SetConnectedAnchor(Vector2 _vec)
    {
        fixedJoint.connectedAnchor = _vec;
    }
    public void SetAutoAnchor(bool _b)
    {
        fixedJoint.autoConfigureConnectedAnchor = _b;
    }


    /// <summary>
    /// 이 오브젝트를 잡을 수 있는 범위입니다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController.Instance.SetCurrentJointThis(fixedJoint);
            isTouched = true;
            Debug.Log(gameObject.name + "와 닿았다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //만약 현재 플레이어의 캣치조인트가 자신의 조인트라면
            if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            {
                //널로 변경
                PlayerController.Instance.SetCurrentJointThis(null);
                isTouched = false;
            }
            //아니면 그냥 놔둬야지
        }
    }

    /// <summary>
    /// 플레이어의 캣치조인트가 자신의 조인트인지 판단합니다.
    /// </summary>
    /// <returns>맞으면 트루, 틀리면 펄스</returns>
    private bool CheckPlayerJoint()
    {

        return PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint;
    }


}
