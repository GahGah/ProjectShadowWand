using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchableObject : MonoBehaviour, ICatchable
{
    [Space(10)]
    public Collider2D currentCollider;

    //[Space(10)]
    //public FixedJoint2D fixedJoint;

    public Rigidbody2D rigidBody;

    public bool autoAnchorBool;

    [Tooltip("이 오브젝트가 플레이어와 닿아있는 상태인지를 체크합니다.")]
    public bool isTouched;

    [Tooltip("이 오브젝트가 잡혀있는 상태인지 체크합니다.")]
    public bool isCatched;

    void Start()
    {
        Init();
    }


    public void Init()
    {
        if (currentCollider == null)
        {

            var _tempColls = GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < _tempColls.Length; i++)
            {
                var _tempColl = _tempColls[i];

                if (_tempColl.isTrigger == false)
                {
                    currentCollider = _tempColl;
                    break;
                }
            }
        }

        //if (fixedJoint == null)
        //{
        //    fixedJoint = GetComponentInChildren<FixedJoint2D>();
        //}

        if (rigidBody == null)
        {
           rigidBody =  GetComponentInChildren<Rigidbody2D>();
        }

        //fixedJoint.autoConfigureConnectedAnchor = false;
        //fixedJoint.enabled = false;

        Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider);

    }

    private void Update()
    {
        if (PlayerController.Instance.GetTouchedObject() == gameObject)
        {
            PlayerController.Instance.CheckCatchInput(this);
        }
        else if(PlayerController.Instance.GetCatchedObject() == this)
        {
            PlayerController.Instance.CheckCatchInput(this);
        }
    }
    //private void Update()
    //{
    //isTouched = (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint);

    //if (PlayerController.Instance.isInputCatchKey)
    //{
    //    if (PlayerController.Instance.GetCatchingObject() == null)
    //    {
    //        if (isTouched)
    //        {
    //            if (PlayerController.Instance.GetCatchingObject() != gameObject) // 이 오브젝트를 잡고있지 않다면
    //            {
    //                GoCatchThis();
    //            }
    //        }

    //    }
    //    else if (PlayerController.Instance.GetCatchingObject() == gameObject) // 이 오브젝트가 잡혀있다면
    //    {

    //        GoPutThis();

    //    }


    //}

    //}
    public void GoCatchThis()
    {
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    /// <summary>
    /// 위치를 정합니다.
    /// </summary>
    /// <param name="_pos"></param>
    public void SetPosition(Vector2 _pos)
    {
        rigidBody.position = _pos;
    }
    public void GoPutThis()
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }



    /// <summary>
    /// 이 오브젝트를 잡을 수 있는 범위입니다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player");
        if (collision.CompareTag("Player"))
        {
            //닿았다면 터지 오브젝트를 자신으로
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            Debug.Log(gameObject.name + "와 닿았다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //자신이 터치 오브젝트일 경우(다른 오브젝트에 닿지 않은 경우)
            if (PlayerController.Instance.GetTouchedObject() == gameObject)
            {
                //터치 오브젝트를 null로
                PlayerController.Instance.SetTouchedObject(null);

            }
            //if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            //{
            //    //널로 변경
            //    PlayerController.Instance.SetCurrentJointThis(null);
            //}

            //if (PlayerController.Instance.GetCatchedObject() == this)
            //{
            //    PlayerController.Instance.SetCatchedObject(null);
            //}


            isTouched = false;
            //만약 현재 플레이어의 캣치조인트가 자신의 조인트라면
            //if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            //{
            //    //널로 변경
            //    PlayerController.Instance.SetCurrentJointThis(null);
            //    isTouched = false;
            //}
            //아니면 그냥 놔둬야지
        }
    }

    /// <summary>
    /// 플레이어의 캣치조인트가 자신의 조인트인지 판단합니다.
    /// </summary>
    /// <returns>맞으면 트루, 틀리면 펄스</returns>
    //private bool CheckPlayerJoint()
    //{
    //    return PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint;
    //}

}
