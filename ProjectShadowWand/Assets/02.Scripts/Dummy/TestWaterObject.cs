using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterObject : MonoBehaviour, IPushable
{

    [Space(10)]
    public Collider2D currentCollider;
    [Space(10)]
    public Rigidbody2D rigidBody;

    public bool isTouched = false;
    private float originalMass;
    private float pushingMass;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        currentCollider = GetComponent<Collider2D>();
        var testMass = rigidBody.mass;
        originalMass = testMass;
        pushingMass = 10f;
    }


    private void Update()
    {

        var touchObject = PlayerController.Instance.GetTouchedObject();
        isTouched = touchObject == gameObject;

        if (InputManager.Instance.buttonPush.wasPressedThisFrame) //처음 눌렀을 때
        {
            if (PlayerController.Instance.GetCatchingObject() == null) // 잡고있는 오브젝트가 없다면
            {
                if (isTouched)
                {

                    if (PlayerController.Instance.GetCatchingObject() != gameObject) // 이 오브젝트를 잡고있지 않다면
                    {
                        GoPushReady();
                        Debug.Log("잡기 준비!!!");
                    }
                }
            }
        }


        if (InputManager.Instance.buttonPush.isPressed) //꾹 누르고 있는 상태에서는
        {
            GoPushThis();
        }


        if (InputManager.Instance.buttonPush.wasReleasedThisFrame) //떼면
        {
            GoPutThis();
        }
    }
    public void GoPushReady()
    {
        PlayerController.Instance.SetCatchingObject(gameObject);

        rigidBody.mass = pushingMass;
    }
    public void GoPushThis()
    {
    }

    public void GoPutThis()
    {
        PlayerController.Instance.SetCatchingObject(null);
        rigidBody.mass = originalMass;
    }



    /// <summary>
    /// 이 오브젝트를 밀 수 있는 범위입니다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            Debug.Log(gameObject.name + "와 닿았다.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //만약 현재 플레이어의 캣치조인트가 자신의 조인트라면
            if (PlayerController.Instance.GetTouchedObject()==gameObject)
            {
                //널로 변경

                isTouched = false;
            }
            if (PlayerController.Instance.GetCatchingObject()==gameObject)
            {
                GoPutThis();
            }
            //아니면 그냥 놔둬야지
        }



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
}
