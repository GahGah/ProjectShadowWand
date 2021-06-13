using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� �ִ� ������Ʈ�� Ŭ�����Դϴ�.
/// </summary>
public class MovableObject : MonoBehaviour
{

    //[Header("�̵� ����")]

    //[Tooltip("�����̴� �ӵ�")]
    //public float movementSpeed;

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
    /// �̵� �Լ��� ȣ���մϴ�( FixedUpdate ����...)
    /// </summary>
    public void SetMovement(eMovementType _moveType, Vector2 _moveVector)
    {

        if (haveParents)
        {
            switch (_moveType)
            {
                case eMovementType.SetVelocity:
                    myRigidbody.velocity = _moveVector;
                    break;

                case eMovementType.AddVelocity:

                    myRigidbody.velocity += _moveVector;
                    break;

                case eMovementType.MovePosition:

                    myRigidbody.MovePosition(_moveVector);
                    break;

                case eMovementType.AddForce:
                    myRigidbody.AddForce(_moveVector, ForceMode2D.Impulse);
                    break;

                default:
                    break;
            }


        }
        else
        {
            switch (_moveType)
            {
                case eMovementType.SetVelocity:
                    myRigidbody.velocity = _moveVector;
                    break;

                case eMovementType.AddVelocity:

                    myRigidbody.velocity += _moveVector;
                    break;

                case eMovementType.MovePosition:

                    myRigidbody.MovePosition(_moveVector);
                    break;

                case eMovementType.AddForce:
                    myRigidbody.AddForce(_moveVector, ForceMode2D.Impulse);
                    break;

                default:
                    break;
            }
        }


        CalcLastVelocity();
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
