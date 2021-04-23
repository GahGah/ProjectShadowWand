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

        if (InputManager.Instance.buttonPush.wasPressedThisFrame) //ó�� ������ ��
        {
            if (PlayerController.Instance.GetCatchingObject() == null) // ����ִ� ������Ʈ�� ���ٸ�
            {
                if (isTouched)
                {

                    if (PlayerController.Instance.GetCatchingObject() != gameObject) // �� ������Ʈ�� ������� �ʴٸ�
                    {
                        GoPushReady();
                        Debug.Log("��� �غ�!!!");
                    }
                }
            }
        }


        if (InputManager.Instance.buttonPush.isPressed) //�� ������ �ִ� ���¿�����
        {
            GoPushThis();
        }


        if (InputManager.Instance.buttonPush.wasReleasedThisFrame) //����
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
    /// �� ������Ʈ�� �� �� �ִ� �����Դϴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            Debug.Log(gameObject.name + "�� ��Ҵ�.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //���� ���� �÷��̾��� Ĺġ����Ʈ�� �ڽ��� ����Ʈ���
            if (PlayerController.Instance.GetTouchedObject()==gameObject)
            {
                //�η� ����

                isTouched = false;
            }
            if (PlayerController.Instance.GetCatchingObject()==gameObject)
            {
                GoPutThis();
            }
            //�ƴϸ� �׳� ���־���
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
