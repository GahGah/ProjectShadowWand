using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 움직일 수 있는 오브젝트의 클래스입니다. [우선 SetVelocity, AddForce만 사용합니다.]
/// </summary>
public class MovableObject : MonoBehaviour
{

    //[Header("이동 관련")]

    //[Tooltip("움직이는 속도")]
    //public float movementSpeed;

    [Tooltip("움직이는 속도(벡터)")]
    public Vector2 movementSpeeds;
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


    [HideInInspector]
    public eMovementType currentMovementType;
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
    /// _child의 부모를 설정합니다.
    /// </summary>
    /// <param name="_child">설정될 child</param>
    /// <param name="_parents">childe의 부모로 설정될 mo</param>
    public void SetParents(MovableObject _child, MovableObject _parents)
    {
        _child.parentsObject = _parents;
        _child.haveParents = (_parents != null);

        Log("부모를 설정했습니다 - " + _child.haveParents);
    }

    public void UpdateParentsFollowMovement()
    {
        if (haveParents) // 부모가 있다면
        {
            SetMovement(eMovementType.AddVelocity, new Vector2(parentsObject.myRigidbody.velocity.x, 0f));
            //moveVector
            //_moveVector = parentsObject.myRigidbody.velocity + _moveVector; //무브 벡터에 벨로시티 더하기
        }

    }

    //public void UpdateMovement() 아직 사용하지 않음
    //{
    //    if (haveParents) // 부모가 있다면
    //    {
    //        _moveVector = parentsObject.myRigidbody.velocity + _moveVector; //무브 벡터에 벨로시티 더하기
    //    }
    //}

    public void SetMoveVector(Vector2 _mv)
    {
        moveVector = _mv;
    }
    private Vector2 moveVector;
    /// <summary>
    /// _moveVector만큼 Rigidbody를 이동시킵니다( FixedUpdate 권장...).
    /// </summary>
    /// <param name="_moveType">이동 타입입니다.</param>
    /// <param name="_moveVector">방향 * 원하는 속도의 값입니다. 타입 VDP의 경우, 목적지 위치로 사용됩니다.</param>
    public void SetMovement(eMovementType _moveType, Vector2 _moveVector)
    {
        currentMovementType = _moveType;

        //if (haveParents) // 부모가 있다면
        //{
        //    _moveVector = parentsObject.myRigidbody.velocity + _moveVector; //무브 벡터에 벨로시티 더하기
        //}

        switch (_moveType)
        {
            case eMovementType.SetVelocity:
                if (myRigidbody.velocity != _moveVector)
                {
                    myRigidbody.velocity = _moveVector;
                }
                break;

            case eMovementType.AddVelocity:
                myRigidbody.velocity += _moveVector;
                break;

            case eMovementType.SetVelocityDesiredPosition:

                myRigidbody.velocity = CalcDesiredVelocity(_moveVector);
                break;
            case eMovementType.AddForce:

                myRigidbody.AddForce(_moveVector, ForceMode2D.Impulse);
                break;


            #region 사용하지 않음
            //case eMovementType.AddVelocity:

            //    myRigidbody.velocity += _moveVector;
            //    break;

            //case eMovementType.MovePosition:

            //    myRigidbody.MovePosition(_moveVector);
            //    break;
            #endregion
            default:
                break;
        }
        moveVector = _moveVector;
        //  UpdateParentsFollowMovement();
        CalcLastVelocity();
    }

    [HideInInspector]
    [Tooltip("원하는 위치로 이동하려는 방향을 갖고있는 velocity값입니다.")]
    public Vector2 desiredVelocity = Vector2.zero;
    private Vector2 directionalVector = Vector2.zero;

    [Tooltip("이전 목적지입니다.")]
    private Vector2 prevDestination = Vector2.zero;
    private float sqrMag = 0f;
    private float lastSqrMag = 0f;

    /// <summary>
    /// 일정 속도로 원하는 위치를 향하는 desiredVelocity값을 반환합니다.
    /// </summary>
    /// <param name="_destination">원하는 위치입니다. 속도는 movementSpeeds의 x값을 사용합니다.</param>
    /// <returns></returns>
    public Vector2 CalcDesiredVelocity(Vector2 _destination)
    {

        sqrMag = (_destination - myRigidbody.position).sqrMagnitude;

        if (sqrMag != lastSqrMag) //아직 이동 중이라면
        {
            if (prevDestination != _destination) //다를 때에만 실행
            {
                prevDestination = _destination; //이전 목적지를 _destination으로 설정

                directionalVector = (_destination - myRigidbody.position).normalized * movementSpeeds.x; // 원래는 그냥 float speed를 곱했었음.

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
    ///  마지막 포지션을 설정하고, 마지막 벨로시티를 계산합니다.
    /// </summary>
    public void CalcLastVelocity()
    {
        lastVelocity = (myRigidbody.position - lastPosition) * 1f / Time.deltaTime;
        lastPosition = myRigidbody.position;
    }


    public void Log(object _string)
    {
        Debug.Log("[" + gameObject.name + "] : " + _string);
    }


}
