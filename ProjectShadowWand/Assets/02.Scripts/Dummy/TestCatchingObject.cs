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

    [Tooltip("�� ������Ʈ�� �÷��̾�� ����ִ� ���������� üũ�մϴ�.")]
    public bool isTouched;

    [Tooltip("�� ������Ʈ�� �����ִ� �������� üũ�մϴ�.")]
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
            Log("Ŀ��Ʈ �ݶ��̴��� ����. �ϴ� �ڵ����� �־��");

            var _tempColls = GetComponents<Collider2D>();


            for (int i = 0; i < _tempColls.Length; i++)
            {
                var _tempColl = _tempColls[i];

                if (_tempColl.isTrigger == false)
                {
                    Log("�ڵ����� �ݶ��̴� �ֱ� ����. ");
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
                    if (PlayerController.Instance.GetCatchingObject() != gameObject) // �� ������Ʈ�� ������� �ʴٸ�
                    {
                        GoCatchThis();
                    }
                }

            }
            else if (PlayerController.Instance.GetCatchingObject() == gameObject) // �� ������Ʈ�� �����ִٸ�
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

        //�÷��̾�� �浹�ϰ�...���ص� �� �� ������ �ϴ� �ϱ�

        Log("��ҽ��ϴ�.");
    }

    public void GoPutThis()
    {
        //�÷��̾�� �浹���� �ʰ� �Ѵ�.
        //Physics2D.IgnoreCollision(PlayerController.Instance.playerCollider, currentCollider);

        SetConnectedBody(null);
        SetConnectedAnchor(Vector2.zero); ;
        SetAutoAnchor(false);

        PlayerController.Instance.SetCatchingObject(null);

        fixedJoint.enabled = false;


        Log("���ƺ��ô�.");

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
    /// �� ������Ʈ�� ���� �� �ִ� �����Դϴ�.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            PlayerController.Instance.SetCurrentJointThis(fixedJoint);
            isTouched = true;
            Debug.Log(gameObject.name + "�� ��Ҵ�.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //���� ���� �÷��̾��� Ĺġ����Ʈ�� �ڽ��� ����Ʈ���
            if (PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint)
            {
                //�η� ����
                PlayerController.Instance.SetCurrentJointThis(null);
                isTouched = false;
            }
            //�ƴϸ� �׳� ���־���
        }
    }

    /// <summary>
    /// �÷��̾��� Ĺġ����Ʈ�� �ڽ��� ����Ʈ���� �Ǵ��մϴ�.
    /// </summary>
    /// <returns>������ Ʈ��, Ʋ���� �޽�</returns>
    private bool CheckPlayerJoint()
    {

        return PlayerController.Instance.GetCurrentCatchJoint() == fixedJoint;
    }


}
