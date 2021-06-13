using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �ִ� ������Ʈ�� Ŭ�����Դϴ�. [�켱 SetVelocity, AddForce�� ����մϴ�.]
/// </summary>
public class MovableObject : MonoBehaviour
{

    //[Header("�̵� ����")]

    //[Tooltip("�����̴� �ӵ�")]
    //public float movementSpeed;

    [Tooltip("�����̴� �ӵ�(����)")]
    public Vector2 movementSpeeds;
    //[Tooltip("������")]
    //public float jumpForce;

    [HideInInspector]
    [Tooltip("������ �ٵ�")]
    public Rigidbody2D myRigidbody;

    [HideInInspector]
    [Tooltip("�θ� ������Ʈ�Դϴ�.")]
    public MovableObject parentsObject = null;

    [HideInInspector]
    [Tooltip("�θ� ������ �ִ� �����ΰ�?")]
    public bool haveParents = false;


    [Tooltip("���� �������� ��ġ�Դϴ�.")]
    [HideInInspector]
    public Vector2 lastPosition = Vector2.zero;

    [HideInInspector]
    [Tooltip("���� �����ӿ� ���� �ӷ°�.")]
    public Vector2 lastVelocity = Vector2.zero;


    [HideInInspector]
    public eMovementType currentMovementType;
    /// <summary>
    /// �θ� �����մϴ�.
    /// </summary>
    public void SetParents(MovableObject _parents)
    {
        parentsObject = _parents;
        haveParents = (_parents != null);

        Log("�θ� �����߽��ϴ� - " + haveParents);
    }



    /// <summary>
    /// _moveVector��ŭ Rigidbody�� �̵���ŵ�ϴ�( FixedUpdate ����...).
    /// </summary>
    /// <param name="_moveType">�̵� Ÿ���Դϴ�.</param>
    /// <param name="_moveVector">���� * ���ϴ� �ӵ��� ���Դϴ�. Ÿ�� VDP�� ���, ������ ��ġ�� ���˴ϴ�.</param>
    public void SetMovement(eMovementType _moveType, Vector2 _moveVector)
    {
        currentMovementType = _moveType;

        if (haveParents) // �θ� �ִٸ�
        {
            // //���� ���͸�
            _moveVector = parentsObject.myRigidbody.velocity + _moveVector;
        }

        switch (_moveType)
        {
            case eMovementType.SetVelocity:
                myRigidbody.velocity = _moveVector;
                break;

            //case eMovementType.AddVelocity:

            //    myRigidbody.velocity += _moveVector;
            //    break;

            //case eMovementType.MovePosition:

            //    myRigidbody.MovePosition(_moveVector);
            //    break;

            case eMovementType.AddForce:
                myRigidbody.AddForce(_moveVector, ForceMode2D.Impulse);
                break;

            case eMovementType.SetVelocityDesiredPosition:
                myRigidbody.velocity = CalcDesiredVelocity(_moveVector);
                break;

            default:
                break;
        }


        CalcLastVelocity();
    }

    [HideInInspector]
    [Tooltip("���ϴ� ��ġ�� �̵��Ϸ��� ������ �����ִ� velocity���Դϴ�.")]
    public Vector2 desiredVelocity = Vector2.zero;
    private Vector2 directionalVector = Vector2.zero;

    [Tooltip("���� �������Դϴ�.")]
    private Vector2 prevDestination = Vector2.zero;
    private float sqrMag = 0f;
    private float lastSqrMag = 0f;

    /// <summary>
    /// ���� �ӵ��� ���ϴ� ��ġ�� ���ϴ� desiredVelocity���� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="_destination">���ϴ� ��ġ�Դϴ�. �ӵ��� movementSpeeds�� x���� ����մϴ�.</param>
    /// <returns></returns>
    public Vector2 CalcDesiredVelocity(Vector2 _destination)
    {

        sqrMag = (_destination - myRigidbody.position).sqrMagnitude;

        if (sqrMag > lastSqrMag) //���� �̵� ���̶��
        {
            if (prevDestination != _destination) //�ٸ� ������ ����
            {
                prevDestination = _destination; //���� �������� _destination���� ����

                directionalVector = (_destination - myRigidbody.position).normalized * movementSpeeds.x; // ������ �׳� float speed�� ���߾���.

                desiredVelocity = directionalVector;
            }
            lastSqrMag = Mathf.Infinity;
        }
        else
        {
            desiredVelocity = Vector2.zero;
            lastSqrMag = sqrMag;
        }

        return desiredVelocity;
    }
    /// <summary>
    ///  ������ �������� �����ϰ�, ������ ���ν�Ƽ�� ����մϴ�.
    /// </summary>
    public void CalcLastVelocity()
    {
        lastVelocity = (myRigidbody.position - lastPosition) * 1f / Time.deltaTime;
        lastPosition = myRigidbody.position;

    }


    public void Log(string _string)
    {
        Debug.Log("[" + gameObject.name + "] : " + _string);
    }


}
