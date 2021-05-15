using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : GrowableObject
{
    [Header("Ŀ�� ���� �Ǵ°Ŵ�?")]
    [Tooltip("�� �ڶ����� �����Ǵ� �Ĺ�������Ʈ�Դϴ�.")]
    public GameObject plantObject;

    [Header("�Ĺ��� ������ ��ġ")]
    [Tooltip("������ �� �ڶ�� �Ĺ��� �ش� ��ġ�� �����˴ϴ�.")]
    public Transform plantTransform;

    private CatchableObject catchableObject;
    private SpriteRenderer spriteRenderer;


    [Header("�ִϸ�����")]
    public Animator animator;
    private void Start()
    {
        Init();
        catchableObject.canCatched = true;
        GrowCoroutine = ProcessGrow();
    }
    public void Init()
    {
        if (growTime <= 0f)
        {
            growTime = 5f;
        }

        if (currentGrowTime >= 0f)
        {
            currentGrowTime = 0f;
        }

        GrowCoroutine = ProcessGrow();
        catchableObject = GetComponent<CatchableObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void OnWater()
    {
        base.OnWater();
        StartGrow();
    }

    public override void StartGrow()
    {
        spriteRenderer.enabled = false;
        catchableObject.canCatched = false;
        base.StartGrow();

    }
    public override void EndGrow()
    {
        isFinishedGrow = true;
        //base.EndGrow();
        Instantiate(plantObject, plantTransform.position, Quaternion.identity, null);
        gameObject.SetActive(false);

    }
    private IEnumerator ProcessGrow()
    {
        currentGrowTime = 0f;

        while (currentPer < 1f)
        {
            currentGrowTime += Time.fixedDeltaTime;
            Debug.Log(currentGrowTime);

            currentPer = currentGrowTime / growTime; 
            //Mathf.Lerp(0f, 1f, currentGrowTime);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        EndGrow();
        yield break;
    }
}
