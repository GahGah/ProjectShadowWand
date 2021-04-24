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

    [Tooltip("�ҿ��� ������Ʈ�� ���- ���Ⱑ ���� ���ΰ�?")]
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

            if (BurningCoroutine != null) //���ư��� �ִ� ���¶��
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
