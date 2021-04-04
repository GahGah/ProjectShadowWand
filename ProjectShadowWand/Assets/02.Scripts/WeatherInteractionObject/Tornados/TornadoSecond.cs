using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : WeatherInteractionObject
{

    [Header("�����̴� ���ӵ�")]
    public float moveVelocity;

    [Header("����޴� ����� ������")]
    public AreaEffector2D affectedAreaEffector;

    [Header("�� �Ʒ��� �ǵ��� �ǵ帮�� ����")]

    [Tooltip("�÷��̾�� ��Ÿ ������Ʈ�� �浹���� �ʴ� ������ �ٵ�. (�߿�)")]
    public Rigidbody2D tornadoRigidBody;

    public Collider2D tornadoTrigger;

    public GameObject tornadoObject_Collider;
    public GameObject tornadoObject_Trigger;

    public AreaEffector2D upWindAreaEffector;

    [Tooltip("���� �����̴� ���������� ���մϴ�.")]
    public bool moveSelf;

    public Vector3 originalScale;

    private LayerMask playerLayer;
    private LayerMask Tornado_ColliderLayer;

    private void Awake()
    {
        Init();
    }
    public override void Init()
    {

        base.Init();
        //�÷��̾�� ����̵� �ݶ��̴����� �浹�� ����.
        playerLayer = LayerMask.NameToLayer("Player");
        Tornado_ColliderLayer = LayerMask.NameToLayer("Tornado_Collider");

        Physics2D.IgnoreLayerCollision(Tornado_ColliderLayer, playerLayer, true);
        //Physics2D.IgnoreLayerCollision(layer1, layer3, true);

        ////���� �����ϴٰ� ������������ ����� ��찡 �־ zero�� �ʱ�ȭ
        //tornadoObject_Collider.transform.localPosition = Vector2.zero;
        //tornadoObject_Trigger.transform.localPosition = Vector2.zero;
        originalScale = transform.localScale;

    }

    private void Update()
    {
        ChangeState();
        Execute();
    }

    private void FixedUpdate()
    {
        if (moveSelf)
        {
            tornadoRigidBody.velocity = new Vector2(moveVelocity * Time.deltaTime, 0f);
        }
    }

    public override void Execute()
    {
        base.Execute();

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


    private void ProcessMoveSelf()
    {
        if (affectedWeather != eMainWeatherType.SUNNY)
        {
            ////����޴� AreaEffector�� forceMagnitude�� 0���� ũ��...�Ƹ��� �����Ϳ� �������...
            //if (tornadoRigidBody.IsTouchingLayers(LayerMask.NameToLayer("AreaEffector")))
            //{
                if (affectedAreaEffector.forceMagnitude !=0f)
                {
                    //���� �������� �ʰ�
                    moveSelf = false;

                    //�̹� ���̳����̸� �������� �ʰ�
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Dynamic)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
                        tornadoRigidBody.velocity = Vector2.zero;
                    }

                }
            else
            {
                //�ƴ϶�� ���� �����̴� ���·�
                moveSelf = true;

                //�̹� Ű�׸�ƽ�̸� �������� �ʰ�
                if (tornadoRigidBody.bodyType != RigidbodyType2D.Kinematic)
                {
                    tornadoRigidBody.bodyType = RigidbodyType2D.Kinematic;
                    tornadoRigidBody.velocity = Vector2.zero;
                }
            }

            //}
            //else
            //{
            //    Debug.Log("�Ƹ��� �����Ϳ� �ȴ�� ����");
            //    //�ƴ϶�� ���� �����̴� ���·�
            //    moveSelf = true;

            //    //�̹� Ű�׸�ƽ�̸� �������� �ʰ�
            //    if (tornadoRigidBody.bodyType != RigidbodyType2D.Kinematic)
            //    {
            //        tornadoRigidBody.bodyType = RigidbodyType2D.Kinematic;
            //        tornadoRigidBody.velocity = Vector2.zero;
            //    }

            //}
        }
        else // ����϶�
        {
            //if (tornadoRigidBody.IsTouchingLayers(LayerMask.NameToLayer("AreaEffector")))
            //{

                if (affectedAreaEffector.forceMagnitude !=0f)
                {
                    //���� �������� �ʰ�
                    moveSelf = false;

                    //�̹� ���̳����̸� �������� �ʰ�
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Dynamic)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
                        tornadoRigidBody.velocity = Vector2.zero;
                    }
                }
                else
                {
                    Debug.Log("��� : �Ƹ��� �����Ϳ� �ȴ�� ����");
                    //�ƴ϶�� ���� �����̴� ���·�
                    moveSelf = false;

                    //�̹� ���̳����̸� �������� �ʰ�
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Static)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Static;
                    }

                }
            //}

        }

    }
    /// <summary>
    /// EnterSunny�� ����
    /// </summary>
    public IEnumerator GoSunny()
    {
        Debug.Log("GoSunny");

        gameObject.tag = "Untagged";
        moveSelf = false;

        //�����Ϳ� ������ �޾ƾ� �ϱ� ������ ���̳������� ����
        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
        tornadoRigidBody.velocity = Vector2.zero;
        var startScale = transform.localScale;

        //1�ʵ��� ��ȭ
        var smallTime = 1f;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            tornadoRigidBody.transform.localScale = Vector2.Lerp(startScale, originalScale * 0.5f, t);
            yield return null;
        }

        //upWindAreaEffector.gameObject.transform.position = transform.position;


        //������ �ѱ�
        upWindAreaEffector.gameObject.SetActive(true);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), Tornado_ColliderLayer, true);

    }

    public IEnumerator GoRainy()
    {
        Debug.Log("GoRainy");

        gameObject.tag = "Tornado";

        //���� ����������
        moveSelf = true;

        tornadoRigidBody.bodyType = RigidbodyType2D.Kinematic;
        tornadoRigidBody.velocity = Vector2.zero;
        var startScale = tornadoRigidBody.transform.localScale;
        var bigTime = 1f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / bigTime;
            transform.localScale = Vector2.Lerp(startScale, originalScale, t);
            yield return null;
        }
        //upWindAreaEffector.gameObject.transform.position = transform.position;
        upWindAreaEffector.gameObject.SetActive(false);


        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), Tornado_ColliderLayer, false);

    }


    public override void ProcessRainy()
    {
        ProcessMoveSelf();
    }

    public override void ProcessSunny()
    {
        ProcessMoveSelf();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (affectedWeather != eMainWeatherType.SUNNY)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("�÷��̾ ��Ҵ�.");
                PlayerController.Instance.ProcessDie();
            }
        }

    }
}
