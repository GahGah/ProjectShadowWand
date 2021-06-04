using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 걍 원경은 느리게 움직이고 근경은 빠르게 움직이고 그거임
/// </summary>
public class ParallaxMoveObject : MonoBehaviour
{

    [Header("추가 움직임값")]
    public Vector2 movementScale = Vector2.zero;

    [Header("x축만 움직임")]
    public bool onlyMoveXpos;

    [Header("플레이어를 따라가는가")]
    public bool isFollowPlayer;

    private float originalZ;

    private Transform myTransform;
    private Vector2 originalPos;


    private Transform cameraTransform;
    void Awake()
    {

        Init();
    }



    private Transform targetTransform;
    private Vector2 startTargetPos;
    private Vector2 startPos;
    public void Init()
    {
        cameraTransform = Camera.main.transform;

        myTransform = GetComponent<Transform>();

        startPos = gameObject.transform.position;

        originalZ = transform.position.z;
    }
    private void Start()
    {
        if (isFollowPlayer)
            targetTransform = PlayerController.Instance.transform;
        else
            targetTransform = cameraTransform;


        startTargetPos = targetTransform.position;
    }

    private Vector2 tempVector2;
    void FixedUpdate()
    {
        tempVector2 = startPos;
        tempVector2 += Vector2.Scale(targetTransform.position - (Vector3)startTargetPos, movementScale);


        //if (onlyMoveXpos)
        //{
        //    tempVector2.x += movementScale.x * (targetTransform.position.x - startTargetPos.x);
        //}
        //else
        //{
        //    tempVector2 += movementScale * (targetTransform.position - (Vector3)startTargetPos);
        //}

        //if ((Vector3)tempVector2 == myTransform.position)
        //{
        //    return;
        //}
        myTransform.position = tempVector2;
    }

}
