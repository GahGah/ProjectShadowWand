using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : WeatherInteractionObject
{
    /// <summary>
    /// �÷��̾�� ��Ÿ ������Ʈ�� �浹���� �ʴ� ������ �ٵ�. (�߿�)
    /// </summary>
    public Rigidbody2D tornadoRigidBody;

    public Collider2D tornadoTrigger;

    public GameObject tornadoObject_Collider;
    public GameObject tornadoObject_Trigger;

    public AreaEffector2D upWindAreaEffector;

    [Tooltip("���� �����̴� ���������� ���մϴ�.")]
    public bool moveSelf;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {
        //�÷��̾�� ����̵� �ݶ��̴����� �浹�� ����.
        LayerMask layer1 = LayerMask.NameToLayer("Tornado_Collider");
        LayerMask layer2 = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(layer1, layer2, true);

        //���� �����ϴٰ� ������������ ����� ��찡 �־ zero�� �ʱ�ȭ
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
    /// EnterSunny�� ����
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
