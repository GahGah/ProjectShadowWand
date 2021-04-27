using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 걍 원경은 느리게 움직이고 근경은 빠르게 움직이고 그거임
/// </summary>
public class ParallaxMoveObject : MonoBehaviour
{
    public Vector3 movementScale = Vector3.one;
    private Transform mainCameraTransform;

    private float originalZ;

    void Awake()
    {
        mainCameraTransform = Camera.main.transform;
        originalZ = transform.position.z;
        //Init();  
    }

    //private void Init()
    //{

    //}
    void LateUpdate()
    {
        var tempVector3 = Vector2.Scale(mainCameraTransform.position, movementScale);
        transform.position = new Vector3(tempVector3.x, tempVector3.y, originalZ);
    }

}
