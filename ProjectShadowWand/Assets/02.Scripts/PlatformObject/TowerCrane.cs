using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCrane : MonoBehaviour
{
    public MovePlatform towerArm;

   // public ElectricableObject electricableObject;

    public bool isMoving;

    private DistanceJoint2D distanceJoint;

    public LineRenderer lineRederer;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        towerArm.Init();
        towerArm.moveSelf = false;
        //towerArm.isLoop = false;
        towerArm.canMoving = false;
        isMoving = false;
    }

    private void Update()
    {
        //if (electricableObject.isShocked)
        //{
        //    towerArm.canMoving = true;
        //}
    }
    private void FixedUpdate()
    {
        //if (electricableObject.isShocked)
        //{
        //    if (towerArm.canMoving && !towerArm.isGoal)
        //    {
        //        towerArm.ProcessMove();
        //    }
        //    else if (towerArm.isGoal) //�����ϸ�
        //    {
        //        electricableObject.isShocked = false;
        //        towerArm.ToggleDestination();
        //        //�ٽ� ���߸��� ������� �������� ����
        //    }
        //}



    }

    IEnumerator ProcessMoveDistance()
    {
        var currentTime = 0f;
        var changeTime = towerArm.moveSpeed;
        var disJo = towerArm.GetComponent<DistanceJoint2D>();

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime / changeTime;

            //currentPer = Mathf.Lerp(0f, 1f, currentTime);

            //ApplyExtent(maxExtent * currentPer);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

    }
}
