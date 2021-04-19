using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 비에 맞으면 불어나는 오브젝트입니다.
/// </summary>
public class WaterObject : WeatherInteractionObject
{
    private PolygonCollider2D waterCollider = null;

    private Vector2 originalPosition = Vector2.zero;
    private Vector2 originalScale = Vector2.zero;

    [Header("물이 차오르는 최대치"), Tooltip("어느 방향으로 얼마만큼 차오를지 정합니다.")]
    public Vector2 maxExtent;
    [Header("물이 차오르는 속도(초)"), Tooltip("해당 초 안에 최대치(increaseRange)에 도달합니다.")]
    public float changeTime;

    [Header("물이 차오르는 기준점이 중앙인가"), Tooltip("true라면 중앙을 기준으로 크기가 늘어납니다. \n 기본값 : false ")]
    public bool isCenter = false;

    private float currentTime;
    private float currentPer;

    private IEnumerator ChangeExtentCoroutine;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        //base.Init();
        waterCollider = GetComponent<PolygonCollider2D>();

        originalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        currentTime = 0f;
        currentPer = 0f;

        ChangeExtentCoroutine = DoChangeExtent();
    }
    private void Start()
    {
        //StartCoroutine(ChangeExtentCoroutine);
        ChangeDelegate(affectedWeather);
    }
    void Update()
    {
        Execute();
        ChangeState();
    }
    public override void Execute()
    {
        base.Execute();

    }
    public override void ChangeState()
    {
        base.ChangeState();

    }
    private IEnumerator DoChangeExtent()
    {
        Debug.Log("시작");
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime / changeTime;

            currentPer = Mathf.Lerp(0f, 1f, currentTime);

            ApplyExtent(maxExtent * currentPer);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        //var test = GetComponent<SpriteRenderer>();
        //test.color = Color.red;
        ChangeExtentCoroutine = null; // null로 변경
    }

    /// <summary>
    /// 적용시킵니다.
    /// </summary>
    /// <param name="_extent"></param>
    private void ApplyExtent(Vector2 _extent)
    {
        transform.localScale = new Vector2(originalScale.x + _extent.x, originalScale.y + _extent.y);

        if (isCenter == false)
        {
            transform.position = new Vector2(originalPosition.x + (_extent.x / 2), (originalPosition.y + (_extent.y / 2)));
        }

    }

    public override void EnterRainy()
    {
        Debug.Log("EnterRainy");
        if (ChangeExtentCoroutine != null) //이미 실행되어있는 상태였다면
        {
            StopCoroutine(ChangeExtentCoroutine); //멈추기
        }
        else
        {
            ChangeExtentCoroutine = DoChangeExtent();
        }
        StartCoroutine(ChangeExtentCoroutine);
    }

    public override void EnterSunny()
    {
        if (ChangeExtentCoroutine != null) //이미 실행되어있는 상태였다면
        {
            StopCoroutine(ChangeExtentCoroutine); //멈추기
        }
        else
        {
            ChangeExtentCoroutine = DoChangeExtent();
            StopCoroutine(ChangeExtentCoroutine);
        }
    }


    public void ButtonGoTest(int _i)
    {
        if (_i == 0)
        {
            if (ChangeExtentCoroutine == null)
            {
                ChangeExtentCoroutine = DoChangeExtent();
            }

            StopCoroutine(ChangeExtentCoroutine);
            ChangeExtentCoroutine = null;
        }
        else if (_i == 1)
        {
            if (ChangeExtentCoroutine == null)
            {
                ChangeExtentCoroutine = DoChangeExtent();
                StartCoroutine(ChangeExtentCoroutine);
            }
        }
        else
        {
            if (ChangeExtentCoroutine == null)
            {
                ChangeExtentCoroutine = DoChangeExtent();
            }

            StopCoroutine(ChangeExtentCoroutine);
            ChangeExtentCoroutine = null;

            currentTime = 0f;
            currentPer = 0f;

            transform.position = originalPosition;
            transform.localScale = originalScale;

            ChangeExtentCoroutine = DoChangeExtent();
            StartCoroutine(ChangeExtentCoroutine);

            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //플래이어가 들어왔다면
        {
            PlayerController.Instance.SetIsWater(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //플래이어가 나갔다면
        {
            PlayerController.Instance.SetIsWater(false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
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
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;
    //    if (originalScale != Vector2.zero)
    //    {
    //        Gizmos.DrawWireCube(new Vector3(originalPosition.x + (maxExtent.x / 2), originalPosition.y + (maxExtent.y / 2)),
    //                new Vector3(originalScale.x + maxExtent.x, originalScale.y + maxExtent.y));

    //    }
    //    else
    //    {
    //        Gizmos.DrawWireCube(new Vector3(transform.position.x + (maxExtent.x / 2), transform.position.y + (maxExtent.y / 2)),
    //                new Vector3(transform.localScale.x + maxExtent.x, transform.localScale.y + maxExtent.y));
    //    }
    //}


}
