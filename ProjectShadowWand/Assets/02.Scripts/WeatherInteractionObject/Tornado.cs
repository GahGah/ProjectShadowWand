using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tornado : WeatherInteractionObject
{
    public PlayerController player;

    [Header("좌,우로 움직이는 힘")]
    public float movePower;

    public LayerMask areaMask;

    private Rigidbody2D rb;

    public AreaEffector2D upWindAreaEffector;

    [Header("이 토네이도가 영향을 받는 아리아 이펙터")]
    public AreaEffector2D affectedAreaEffector;

    [HideInInspector] public bool isPlayerIn;


    [HideInInspector] public bool shouldMove;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        rb = GetComponent<Rigidbody2D>();
        upWindAreaEffector.gameObject.SetActive(false);
        shouldMove = true;

        if (affectedAreaEffector == null)
        {
            Debug.LogError("아리아이펙터 증발");
        }
    }
    private void Update()
    {
        ChangeState();
        Execute();

    }

    private void FixedUpdate()
    {
        if (shouldMove)
        {
            rb.velocity = new Vector2(movePower * Time.deltaTime, 0f);
        }


    }

    public override void Execute()
    {
        base.Execute();
        if (WeatherManager.Instance.GetMainWeather() != eMainWeatherType.SUNNY)
        {
            if (affectedAreaEffector.forceMagnitude > 0f)
            {

                shouldMove = false;
                if (rb.bodyType != RigidbodyType2D.Dynamic)
                {
                    rb.velocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
            else
            {
                shouldMove = true;
                if (rb.bodyType != RigidbodyType2D.Kinematic)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }
        else
        {
            shouldMove = false;
        }

    }

    public override void ChangeState()
    {
        base.ChangeState();
    }

    public override void EnterSunny()
    {
        StartCoroutine(GoSunny());
    }

    public override void EnterRainy()
    {
        StartCoroutine(GoRainy());
    }

    /// <summary>
    /// EnterSunny때 실행
    /// </summary>
    public IEnumerator GoSunny()
    {
        Debug.Log("GoSUnny");

        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("Default");

        shouldMove = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        var startScale = rb.transform.localScale;
        var smallTime = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            rb.transform.localScale = Vector2.Lerp(startScale, Vector2.one / 0.5f, t);
            yield return null;
        }
        upWindAreaEffector.gameObject.transform.position = transform.position;
        upWindAreaEffector.gameObject.SetActive(true);
        rb.velocity = Vector2.zero;
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public IEnumerator GoRainy()
    {
        Debug.Log("GoRainy");

        gameObject.tag = "Tornado";
        gameObject.layer = LayerMask.NameToLayer("Tornado");

        shouldMove = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        var startScale = rb.transform.localScale;
        var smallTime = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            rb.transform.localScale = Vector2.Lerp(startScale, new Vector2(4f,3f), t);
            yield return null;
        }
        upWindAreaEffector.gameObject.transform.position = transform.position;
        upWindAreaEffector.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;

    }
    public override void ProcessRainy()
    {

    }
    public override void ProcessSunny()
    {
        upWindAreaEffector.transform.position = transform.position;
    }




}
