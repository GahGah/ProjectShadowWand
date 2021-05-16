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

    [Tooltip("�� ������Ʈ�� ���� �� �ִ� ���������� üũ�մϴ�.")]
    public bool canCatched;
    [Tooltip("�� ������Ʈ�� �÷��̾�� ����ִ� ���������� üũ�մϴ�.")]
    public bool isTouched;

    [Tooltip("�� ������Ʈ�� �����ִ� �������� üũ�մϴ�.")]
    public bool isCatched;


    public bool isColliderOn;
    public float positionFix;

    private PlatformEffector2D platformEffector;
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
            rigidBody = GetComponentInChildren<Rigidbody2D>();
        }

        //fixedJoint.autoConfigureConnectedAnchor = false;
        //fixedJoint.enabled = false;

        if (isColliderOn == false)
        {
            Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider);
        }

    }

    //private void Update()
    //{
    //    if (PlayerController.Instance.GetTouchedObject() == gameObject)
    //    {
    //        PlayerController.Instance.CheckCatchInput(this);
    //    }
    //    else if(PlayerController.Instance.GetCatchedObject() == this)
    //    {
    //        PlayerController.Instance.CheckCatchInput(this);
    //    }
    //}


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
        gameObject.transform.SetParent(PlayerController.Instance.transform);
        if (PlayerController.Instance.isRight) //�������� �����־��ٸ�
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            positionFix = 1f;
        }
        else
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            positionFix = 1f;
        }

        if (isColliderOn == true)
        {
            Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider);
        }

    }

    /// <summary>
    /// ��ġ�� ���մϴ�.
    /// </summary>
    /// <param name="_pos"></param>
    public void SetPosition(Vector2 _pos)
    {
        transform.localPosition = new Vector2(_pos.x * positionFix, _pos.y);
    }
    public void GoPutThis()
    {
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        gameObject.transform.SetParent(null);

    }



    /// <summary>
    /// �� ������Ʈ�� ���� �� �ִ� �����Դϴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (canCatched == true)
            {
                //��Ҵٸ� ��ġ ������Ʈ�� �ڽ�����
                PlayerController.Instance.SetTouchedObject(this);
                isTouched = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //�ڽ��� ��ġ ������Ʈ�� ���(�ٸ� ������Ʈ�� ���� ���� ���)
            if (PlayerController.Instance.GetTouchedObject() == this)
            {
                //��ġ ������Ʈ�� null��
                PlayerController.Instance.SetTouchedObject(null);

            }

            if (isColliderOn == true)
            {
                Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider, false);
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
