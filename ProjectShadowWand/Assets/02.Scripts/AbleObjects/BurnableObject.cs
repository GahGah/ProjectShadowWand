using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{

    [Tooltip("Ż �� �ִ� �����ΰ�?")]
    public bool canBurn = true;

    [Tooltip("Ÿ�� ���ΰ�?")]
    public bool isBurning = false;
    [Tooltip("�ѹ��̶� ź ���� �ִ°�?")]
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
