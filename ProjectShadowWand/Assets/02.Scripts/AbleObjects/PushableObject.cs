using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour, IPushable
{

    public bool isTouched = false;
    public Collider2D currentCollider;
    public Rigidbody2D rigidBody;

    public float originalMass;
    public void GoPushReady()
    {
        rigidBody.mass = 1f;
    }
    /// <summary>
    /// ���������� �Ҹ��ϴ�. ������!!
    /// </summary>
    public void GoPushThis()
    {
    }

    public void GoPutThis()
    {
        rigidBody.mass = originalMass;
        rigidBody.velocity = Vector2.zero;
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

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.GetTouchedObject()==gameObject)
        {
            PlayerController.Instance.CheckPushInput(this);
        }
        else if (PlayerController.Instance.GetPushedObject() == this)
        {
            PlayerController.Instance.CheckPushInput(this);
        }
    }
    public void Init()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }
        if (currentCollider == null)
        {
            Debug.Log("Ŀ��Ʈ �ݶ��̴��� ����. �ϴ� �ڵ����� �־��");

            var _tempColls = GetComponents<Collider2D>();
            for (int i = 0; i < _tempColls.Length; i++)
            {
                var _tempColl = _tempColls[i];

                if (_tempColl.isTrigger == false)
                {
                    Debug.Log("�ڵ����� �ݶ��̴� �ֱ� ����. ");
                    currentCollider = _tempColl;
                    break;
                }
            }
        }
        originalMass = rigidBody.mass;
    }

    /// <summary>
    /// �� ������Ʈ�� ���� �� �ִ� �����Դϴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //��Ҵٸ� ���� ������Ʈ�� �ڽ�����
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            //Debug.Log(gameObject.name + "�� ��Ҵ�.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //�ڽ��� ��ġ ������Ʈ�� ���(�ٸ� ������Ʈ�� ���� ���� ���)
            if (PlayerController.Instance.GetTouchedObject() == gameObject)
            {
                Debug.LogError("����");
                //��ġ ������Ʈ�� null��
                PlayerController.Instance.SetTouchedObject(null);
                rigidBody.mass = originalMass;

            }

            if (PlayerController.Instance.GetPushedObject() == this)
            {
                PlayerController.Instance.SetPushedObject(null);
                rigidBody.mass = originalMass;

            }
            //if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            //{
            //    //�η� ����
            //    PlayerController.Instance.SetCurrentJointThis(null);
            //}

            //if (PlayerController.Instance.GetCatchedObject() == this)
            //{
            //    PlayerController.Instance.SetCatchedObject(null);
            //}


            isTouched = false;
            //���� ���� �÷��̾��� Ĺġ����Ʈ�� �ڽ��� ����Ʈ���
            //if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            //{
            //    //�η� ����
            //    PlayerController.Instance.SetCurrentJointThis(null);
            //    isTouched = false;
            //}
            //�ƴϸ� �׳� ���־���
        }
    }

}
