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

    void Awake()
    {
        mainCameraTransform = Camera.main.transform;
        //Init();  
    }

    //private void Init()
    //{

    //}
    void LateUpdate()
    {
        transform.position = Vector3.Scale(mainCameraTransform.position, movementScale);
    }

}
