using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������ ������ ���Ʒ��� �����̴� ������
/// </summary>
public class ElectricableSuRim : MonoBehaviour
{

    public ElectricableObject electricableObject;
    public MovePlatform movePlatform;

    public bool isMoving;


    private void Start()
    {
        Init();
    }
    public void Init()
    {
        movePlatform.Init();
        movePlatform.moveSelf = false;
        movePlatform.isLoop = true;
        movePlatform.canMoving = false;
        isMoving = false;
    }

    private void Update()
    {
        if (electricableObject.isShocked)
        {

            movePlatform.canMoving = true;
            isMoving = true;
            movePlatform.ProcessMove();
        }
    }

}
