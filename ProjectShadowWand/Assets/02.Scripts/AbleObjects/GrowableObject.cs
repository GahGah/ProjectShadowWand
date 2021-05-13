using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 물에 닿으면 자라는 오브젝트입니다.
/// </summary>
public class GrowableObject : MonoBehaviour
{
    [Tooltip("물에 한 번이라도 젖은 적이 있다면 true가 됩니다.")]
    public bool isWetted = false;

    [Tooltip("식물이 다 자란 상태라면 true가 됩니다.")]
    public bool isFinishedGrow = false;


    [Tooltip("growTime초 동안 자랍니다.")]
    public float growTime;

    [Tooltip("성장 시작부터 지금까지 얼마만큼의 시간이 지났는가?")]
    protected float currentGrowTime;


    [Tooltip("성장도를 0~1로 나타냅니다.")]
    protected float currentPer;

    [Tooltip("자라는 것 자체에 쓰이는 코루틴입니다.")]
    protected IEnumerator GrowCoroutine;

    /// <summary>
    /// 외부에서 GrowCoroutine을 호출합니다. 식물이 자라기 시작합니다.
    /// </summary>
    public virtual void StartGrow()
    {
        StartCoroutine(GrowCoroutine);
    }
    /// <summary>
    /// 물에 닿았을 때 호출되는 함수입니다.
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

        Debug.Log("시작");

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
