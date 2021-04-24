using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{

    [Tooltip("탈 수 있는 상태인가?")]
    public bool canBurn = true;

    [Tooltip("타는 중인가?")]
    public bool isBurning = false;
    [Tooltip("한번이라도 탄 적이 있는가?")]
    public bool isBurned = false;

    public FireObject fireObject;

    private int originalLayer;
    private void Start()
    {
        originalLayer = gameObject.layer;
    }

    private void Update()
    {
        if (isBurned)
        {
            canBurn = false;
        }
        if (fireObject == null)
        {
            isBurning = false;
            gameObject.layer = originalLayer;
        }
    }
}
