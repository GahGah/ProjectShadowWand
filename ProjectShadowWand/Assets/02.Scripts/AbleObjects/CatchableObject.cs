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

    [Tooltip("�� ������Ʈ�� �÷��̾�� ����ִ� ���������� üũ�մϴ�.")]
    public bool isTouched;

    [Tooltip("�� ������Ʈ�� �����ִ� �������� üũ�մϴ�.")]
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
    //            if (PlayerController.Instance.GetCatchingObject() != gameObject) // �� ������Ʈ�� ������� �ʴٸ�
    //            {
    //                GoCatchThis();
    //            }
    //        }

    //    }
    //    else if (PlayerController.Instance.GetCatchingObject() == gameObject) // �� ������Ʈ�� �����ִٸ�
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
    /// ��ġ�� ���մϴ�.
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
    /// �� ������Ʈ�� ���� �� �ִ� �����Դϴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player");
        if (collision.CompareTag("Player"))
        {
            //��Ҵٸ� ���� ������Ʈ�� �ڽ�����
            PlayerController.Instance.SetTouchedObject(gameObject);
            isTouched = true;
            Debug.Log(gameObject.name + "�� ��Ҵ�.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //�ڽ��� ��ġ ������Ʈ�� ���(�ٸ� ������Ʈ�� ���� ���� ���)
            if (PlayerController.Instance.GetTouchedObject() == gameObject)
            {
                //��ġ ������Ʈ�� null��
                PlayerController.Instance.SetTouchedObject(null);

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

    /// <summary>
    /// �÷��̾��� Ĺġ����Ʈ�� �ڽ��� ����Ʈ���� �Ǵ��մϴ�.
    /// </summary>
    /// <returns>������ Ʈ��, Ʋ���� �޽�</returns>
    //private bool CheckPlayerJoint()
    //{
    //    return PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint;
    //}

}
