using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 움직일 수 있는 오브젝트의 클래스입니다.
/// </summary>
public class MovableObject : MonoBehaviour
{

    //[Header("이동 관련")]

    //[Tooltip("움직이는 속도")]
    //public float movementSpeed;

    //[Tooltip("점프력")]
    //public float jumpForce;

    [HideInInspector]
    [Tooltip("리지드 바디")]
    public Rigidbody2D myRigidbody;

    [HideInInspector]
    [Tooltip("부모 오브젝트입니다.")]
    public MovableObject parentsObject = null;

    [HideInInspector]
    [Tooltip("부모를 가지고 있는 상태인가?")]
    public bool haveParents = false;


    [Tooltip("이전 프레임의 위치입니다.")]
    [HideInInspector]
    public Vector2 lastPosition = Vector2.zero;

    [HideInInspector]
    [Tooltip("이전 프레임에 계산된 속력값.")]
    public Vector2 lastVelocity = Vector2.zero;

    /// <summary>
    /// 부모를 설정합니다.
    /// </summary>
    public void SetParents(MovableObject _parents)
    {
        parentsObject = _parents;
        haveParents = (_parents != null);

        Log("부모를 설정했습니다 - " + haveParents);
    }


    /// <summary>
    /// 이동 함수를 호출합니다( FixedUpdate 권장...)
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
    ///  마지막 포지션을 설정하고, 마지막 벨로시티를 계산합니다.
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
