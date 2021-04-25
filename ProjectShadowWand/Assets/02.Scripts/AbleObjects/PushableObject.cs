using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour, IPushable
{

    public bool isTouched = false;
    public Collider2D currentCollider;
    public Rigidbody2D rigidBody;

    public float originalMass;

    private Vector2 prevPosition;
    public void GoPushReady()
    {
        rigidBody.mass = 1f;
        PlayerController.Instance.movementSpeed = PlayerController.Instance.pushMoveSpeed;
    }
    public void SetDirection(Vector3 _dir)
    {

    }
    /// <summary>
    /// 지속적으로 불립니다. 주의해!!
    /// </summary>
    public void GoPushThis()
    {
    }

    public void GoPutThis()
    {
        rigidBody.mass = originalMass;
        rigidBody.velocity = Vector2.zero;

        PlayerController.Instance.movementSpeed = PlayerController.Instance.originalMoveSpeed;
    }

    public void SetAutoAnchor(bool _b)
    {
    }

    public void SetConnectedAnchor(Vector2 _vec)
    {
    }

    public void SetConnectedBody(Rigidbody2D _rb)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    public void Init()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        if (currentCollider == null)
        {
            Debug.Log("커런트 콜라이더가 없음. 일단 자동으로 넣어볼게");

            var _tempColls = GetComponents<Collider2D>();
            for (int i = 0; i < _tempColls.Length; i++)
            {
                var _tempColl = _tempColls[i];

                if (_tempColl.isTrigger == false)
                {
                    Debug.Log("자동으로 콜라이더 넣기 성공. ");
                    currentCollider = _tempColl;
                    break;
                    
                }
            }
        }
        originalMass = rigidBody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.GetTouchedObject() == gameObject)
        {
            PlayerController.Instance.CheckPushInput(this);
        }
        else if (PlayerController.Instance.GetPushedObject() == this)
        {
            PlayerController.Instance.CheckPushInput(this);
        }



    }

    private void FixedUpdate()
    {

    }

    /// <summary>
    /// 이 오브젝트를 잡을 수 있는 범위입니다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //닿았다면 터지 오브젝트를 자신으로
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            //Debug.Log(gameObject.name + "와 닿았다.");
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
                rigidBody.mass = originalMass;

            }

            if (PlayerController.Instance.GetPushedObject() == this)
            {
                PlayerController.Instance.SetPushedObject(null);
                PlayerController.Instance.movementSpeed = PlayerController.Instance.originalMoveSpeed;
                rigidBody.mass = originalMass;

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

}
