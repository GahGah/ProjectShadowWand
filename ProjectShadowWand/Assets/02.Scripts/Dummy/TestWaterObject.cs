using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaterObject : WeatherInteractionObject
{


    private PolygonCollider2D waterCollider;

    private Vector2 originalPosition = Vector2.zero;
    private Vector2 originalScale = Vector2.zero;

    [Header("물이 차오르는 최대치"), Tooltip("어느 방향으로 얼마만큼 차오를지 정합니다.")]
    public Vector2 increaseRange;
    [Header("물이 차오르는 속도(초)"), Tooltip("해당 초 안에 최대치(increaseRange)에 도달합니다.")]
    public float increaseTime;

    [SerializeField]
    private float currentTime;
    [SerializeField]
    private float currentPer;

    private IEnumerator IncreaseWater;
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
        IncreaseWater = GoChangeScale_FixedLerp_TimeMode();
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncreaseWater);
        // IncreaseScale(increaseRange);
    }

    // Update is called once per frame
    void Update()
    {
        //Execute();
        //ChangeState();
    }
    public override void Execute()
    {
        //base.Execute();

    }
    public override void ChangeState()
    {
        //base.ChangeState();
    }

    /// <summary>
    /// 특정 방향으로 얼마만큼 늘릴 것인지를 정합니다.
    /// </summary>
    /// <param name="_range"></param>
    public void IncreaseScale(Vector2 _range)
    {
        transform.localScale = new Vector2(originalScale.x + _range.x, originalScale.y + _range.y);
        transform.position = new Vector2(originalPosition.x + (_range.x / 2), (originalPosition.y + (_range.y / 2)));
    }
    private IEnumerator GoChangeScale()
    {
        float temp = 0f;

        while (temp < 0.98f)
        {
            temp = Mathf.Lerp(temp, 1f, Time.deltaTime * 2f);
            IncreaseScale(increaseRange * temp);
            yield return null;
        }
    }

    private IEnumerator GoChangeScale_FixedLerp_TimeMode()
    {
        Debug.Log("시작");
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime / increaseTime;

            currentPer = Mathf.Lerp(0f, 1f, currentTime);

            IncreaseScale(increaseRange * currentPer);

            //timeTakenDuringLerp += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        var test = GetComponent<SpriteRenderer>();
        test.color = Color.red;
    }


    public void ButtonGoTest(int _i)
    {
        if (_i == 0)
        {
            if (IncreaseWater == null)
            {
                IncreaseWater = GoChangeScale_FixedLerp_TimeMode();
            }

            StopCoroutine(IncreaseWater);
            IncreaseWater = null;
        }
        else if (_i == 1)
        {
            if (IncreaseWater == null)
            {
                IncreaseWater = GoChangeScale_FixedLerp_TimeMode();
                StartCoroutine(IncreaseWater);
            }


        }
        else
        {
            if (IncreaseWater == null)
            {
                IncreaseWater = GoChangeScale_FixedLerp_TimeMode();
            }
            StopCoroutine(IncreaseWater);
            IncreaseWater = null;

            currentTime = 0f;
            currentPer = 0f;

            transform.position = originalPosition;
            transform.localScale = originalScale;
            GetComponent<SpriteRenderer>().color = Color.cyan;
            IncreaseWater = GoChangeScale_FixedLerp_TimeMode();

            StartCoroutine(IncreaseWater);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (originalScale != Vector2.zero)
        {
            Gizmos.DrawWireCube(new Vector3(originalPosition.x + (increaseRange.x / 2), originalPosition.y + (increaseRange.y / 2)),
                    new Vector3(originalScale.x + increaseRange.x, originalScale.y + increaseRange.y));

        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(transform.position.x + (increaseRange.x / 2), transform.position.y + (increaseRange.y / 2)),
                    new Vector3(transform.localScale.x + increaseRange.x, transform.localScale.y + increaseRange.y));
        }
    }

}
