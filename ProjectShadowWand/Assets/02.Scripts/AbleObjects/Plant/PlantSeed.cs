using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSeed : GrowableObject
{
    [Header("커서 뭐가 되는거니?")]
    [Tooltip("다 자랐을때 생성되는 식물오브젝트입니다.")]
    public GameObject plantObject;

    private CatchableObject catchableObject;

    private SpriteRenderer spriteRenderer;

    [Header("애니메이터")]
    public Animator animator;
    private void Start()
    {
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
        Instantiate(plantObject, gameObject.transform.position, Quaternion.identity, null);
        gameObject.SetActive(false);

    }
    private IEnumerator ProcessGrow()
    {
        currentGrowTime = 0f;

        while (currentGrowTime < 1f)
        {
            currentGrowTime += Time.fixedDeltaTime / growTime;

            currentPer = Mathf.Lerp(0f, 1f, currentGrowTime);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        EndGrow();
        yield break;
    }
}
