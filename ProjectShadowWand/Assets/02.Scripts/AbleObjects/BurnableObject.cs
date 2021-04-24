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

    [Tooltip("불연소 오브젝트의 경우- 연기가 나는 중인가?")]
    public bool isSmoking = false;


    IEnumerator BurningCoroutine;
    private void Start()
    {
        originalLayer = gameObject.layer;
        BurningCoroutine = null;
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

            if (BurningCoroutine != null) //돌아가고 있는 상태라면
            {
                StopCoroutine(BurningCoroutine);
                BurningCoroutine = null;
            }
        }
        else
        {
            if (BurningCoroutine == null)
            {
                BurningCoroutine = ProcessBurning();
                StartCoroutine(BurningCoroutine);
            }

        }



    }

    public IEnumerator ProcessBurning()
    {
        while (true)
        {
            yield return null;
        }
    }
}
