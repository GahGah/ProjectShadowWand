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

    private GameObject plantObject_Clone;
    private CatchableObject catchableObject;

    //private SpriteRenderer spriteRenderer;

    int animatorGrowBool;


    [Header("�ִϸ�����")]
    public Animator animator;

    private string growString;
    private void Start()
    {
        Init();
        growString = "Grow";
        animatorGrowBool = Animator.StringToHash(growString);
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
        //  spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public override void OnWater()
    {
        base.OnWater();
        StartGrow();
    }

    public override void StartGrow()
    {
        //   spriteRenderer.enabled = false;
        catchableObject.canCatched = false;
        base.StartGrow();

    }

    private void FlipPlantObjectClone()
    {
        Transform tempTr = plantObject_Clone.transform;
        if (catchableObject.isRight)
        {
            tempTr.transform.localScale = new Vector3(Mathf.Abs(tempTr.localScale.x), tempTr.localScale.y, tempTr.localScale.z);

        }
        else
        {
            tempTr.transform.localScale = new Vector3(Mathf.Abs(tempTr.localScale.x) * -1f, tempTr.localScale.y, tempTr.localScale.z);

        }
    }
    public override void EndGrow()
    {
        Debug.Log("EndGrow!");
        isFinishedGrow = true;
        //base.EndGrow();
        plantObject_Clone = Instantiate(plantObject, plantTransform.position, Quaternion.identity, null);
        FlipPlantObjectClone();

        gameObject.SetActive(false);

    }
    private IEnumerator ProcessGrow()
    {
        animator.SetBool(animatorGrowBool, true);


        //while (animator.GetCurrentAnimatorStateInfo(0).IsName("Grow")==false)
        //{
        //    yield return YieldInstructionCache.WaitForEndOfFrame;
        //}

        while (!(animator.GetCurrentAnimatorStateInfo(0).IsName(growString) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f))
        {

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        //yield return new WaitUntil(() => 
        //animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        //&& animator.GetCurrentAnimatorStateInfo(0).IsName("Grow"));

        //currentGrowTime = 0f;
        //animator.SetBool(animatorGrowBool, true);
        //while (currentPer < 1f)
        //{
        //    currentGrowTime += Time.fixedDeltaTime;
        //    Debug.Log(currentGrowTime);

        //    currentPer = currentGrowTime / growTime; 
        //    //Mathf.Lerp(0f, 1f, currentGrowTime);

        //    //timeTakenDuringLerp += Time.deltaTime;

        //    yield return YieldInstructionCache.WaitForFixedUpdate;
        //}
        yield return YieldInstructionCache.WaitForEndOfFrame;

        EndGrow();
        yield break;
    }
}
