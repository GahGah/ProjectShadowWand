using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ������ ������ �����̰� �ٰ��� ������ �����̰� �װ���
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
