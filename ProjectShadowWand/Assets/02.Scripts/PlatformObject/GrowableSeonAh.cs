using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ڶ�� ��ٸ�ȭ �Ǵ� ������Ʈ�Դϴ�.
/// </summary>

public class GrowableSeonAh : MonoBehaviour
{
    GrowableObject growObject;
    Ladder ladder;

    bool isFinishedGrow;

    [Tooltip("�� �ʰ� ������ �� �ڶ����?!")]
    public float growTime;
    private float currentGrowTime;

    [Tooltip("�ڶ�� �ִ� ũ��")]
    public Vector2 maxExtent;


    private float currentPer;
    private IEnumerator GrowCoroutine;

    private Vector2 originalScale;
    private Vector2 originalPosition;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (growObject == null)
        {
            growObject = gameObject.AddComponent<GrowableObject>();
        }
        if (ladder == null)
        {

            ladder = gameObject.AddComponent<Ladder>();
        }

        ladder.canLadder = false;
        isFinishedGrow = false;
        currentGrowTime = 0f;


        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        currentPer = 0f;
    }


    private void FixedUpdate()
    {
        if (growObject.isWetted == true)
        {
            if (GrowCoroutine == null)
            {
                GrowCoroutine = ProcessGrow();
                StartCoroutine(GrowCoroutine);
            }

        }
    }

    private IEnumerator ProcessGrow()
    {
        Debug.Log("����");

        currentGrowTime = 0f;

        while (currentGrowTime < 1f)
        {
            currentGrowTime += Time.deltaTime / growTime;

            currentPer = Mathf.Lerp(0f, 1f, currentGrowTime);

            ApplyExtent(maxExtent * currentPer);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //var test = GetComponent<SpriteRenderer>();
        //test.color = Color.red;
        isFinishedGrow = true;
        ladder.canLadder = true;
    }
    /// <summary>
    /// ũ��� ��ġ�� �����ŵ�ϴ�.
    /// </summary>
    /// <param name="_extent"></param>
    private void ApplyExtent(Vector2 _extent)
    {
        transform.localScale = new Vector2(originalScale.x + _extent.x, originalScale.y + _extent.y);

        transform.position = new Vector2(originalPosition.x + (_extent.x / 2), (originalPosition.y + (_extent.y / 2)));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (originalScale != Vector2.zero)
        {
            Gizmos.DrawWireCube(new Vector3(originalPosition.x + (maxExtent.x / 2), originalPosition.y + (maxExtent.y / 2)),
                    new Vector3(originalScale.x + maxExtent.x, originalScale.y + maxExtent.y));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (maxExtent.x / 2), transform.position.y + (maxExtent.y / 2)),
                    new Vector3(transform.localScale.x + maxExtent.x, transform.localScale.y + maxExtent.y));
        }
    }
}
