using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ������ �ڶ�� ������Ʈ�Դϴ�.
/// </summary>
public class GrowableObject : MonoBehaviour
{
    [Tooltip("���� �� ���̶� ���� ���� �ִٸ� true�� �˴ϴ�.")]
    public bool isWetted = false;

    [Tooltip("�Ĺ��� �� �ڶ� ���¶�� true�� �˴ϴ�.")]
    public bool isFinishedGrow = false;


    [Tooltip("growTime�� ���� �ڶ��ϴ�.")]
    public float growTime;

    [Tooltip("���� ���ۺ��� ���ݱ��� �󸶸�ŭ�� �ð��� �����°�?")]
    protected float currentGrowTime;


    [Tooltip("���嵵�� 0~1�� ��Ÿ���ϴ�.")]
    protected float currentPer;

    [Tooltip("�ڶ�� �� ��ü�� ���̴� �ڷ�ƾ�Դϴ�.")]
    protected IEnumerator GrowCoroutine;

    /// <summary>
    /// �ܺο��� GrowCoroutine�� ȣ���մϴ�. �Ĺ��� �ڶ�� �����մϴ�.
    /// </summary>
    public virtual void StartGrow()
    {
        StartCoroutine(GrowCoroutine);
    }
    /// <summary>
    /// ���� ����� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// </summary>
    public virtual void OnWater()
    {
        if (isWetted == false)
        {
            isWetted = true;
        }
    }
    private IEnumerator TestProcessGrow()
    {

        Debug.Log("����");

        currentGrowTime = 0f;

        while (currentGrowTime < 1f)
        {
            currentGrowTime += Time.deltaTime / growTime;

            currentPer = Mathf.Lerp(0f, 1f, currentGrowTime);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //var test = GetComponent<SpriteRenderer>();
        //test.color = Color.red;
        isFinishedGrow = true;
    }

}
