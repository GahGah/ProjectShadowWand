using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : WeatherInteractionObject
{
    /// <summary>
    /// 플레이어와 기타 오브젝트와 충돌하지 않는 리지드 바디. (중요)
    /// </summary>
    public Rigidbody2D tornadoRigidBody;

    public Collider2D tornadoTrigger;

    public GameObject tornadoObject_Collider;
    public GameObject tornadoObject_Trigger;

    public AreaEffector2D upWindAreaEffector;

    [Tooltip("직접 움직이는 상태인지를 뜻합니다.")]
    public bool moveSelf;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        //플레이어와 토네이도 콜라이더간의 충돌을 막음.
        LayerMask layer1 = LayerMask.NameToLayer("Tornado_Collider");
        LayerMask layer2 = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(layer1, layer2, true);

        //가끔 수정하다가 로컬포지션이 요상한 경우가 있어서 zero로 초기화
        tornadoObject_Collider.transform.localPosition = Vector2.zero;
        tornadoObject_Trigger.transform.localPosition = Vector2.zero;

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
        Debug.Log("GoSunny");

        gameObject.tag = "Untagged";

        moveSelf = false;
        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
        var startScale = tornadoRigidBody.transform.localScale;
        var smallTime = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            tornadoRigidBody.transform.localScale = Vector2.Lerp(startScale, Vector2.one / 0.5f, t);
            yield return null;
        }
        upWindAreaEffector.gameObject.transform.position = transform.position;
        upWindAreaEffector.gameObject.SetActive(true);
        tornadoRigidBody.velocity = Vector2.zero;

        //rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public IEnumerator GoRainy()
    {
        Debug.Log("GoRainy");

        gameObject.tag = "Tornado";

        shouldMove = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        var startScale = rb.transform.localScale;
        var smallTime = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            rb.transform.localScale = Vector2.Lerp(startScale, new Vector2(1.8104f, 1.3578f), t);
            yield return null;
        }
        upWindAreaEffector.gameObject.transform.position = transform.position;
        upWindAreaEffector.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
        }
    }
}
