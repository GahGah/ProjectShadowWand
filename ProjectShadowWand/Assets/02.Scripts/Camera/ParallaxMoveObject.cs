using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 걍 원경은 느리게 움직이고 근경은 빠르게 움직이고 그거임
/// </summary>
public class ParallaxMoveObject : MonoBehaviour
{

   [Header("x축만 움직임")]
    public bool onlyMoveXpos;
    public Vector2 movementScale = Vector2.one;
    private Transform mainCameraTransform;

    private float originalZ;

    private Transform myTransform;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        mainCameraTransform = Camera.main.transform;
        originalZ = transform.position.z;
        //Init();  
    }

    //private void Init()
    //{

    //}

    private Vector2 tempVector2;
    void LateUpdate()
    {
        if (onlyMoveXpos)
        {
            tempVector2 = Vector2.Scale(mainCameraTransform.position, movementScale);
            myTransform.position = new Vector3(tempVector2.x, myTransform.position.y, originalZ);
        }
        else
        {
            tempVector2 = Vector2.Scale(mainCameraTransform.position, movementScale);
            myTransform.position = new Vector3(tempVector2.x, tempVector2.y, originalZ);

        }

    }

}
