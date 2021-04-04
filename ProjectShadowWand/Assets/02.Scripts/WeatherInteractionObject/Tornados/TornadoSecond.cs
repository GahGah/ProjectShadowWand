using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSecond : WeatherInteractionObject
{

    [Header("움직이는 가속도")]
    public float moveVelocity;

    [Header("영향받는 에어리어 이펙터")]
    public AreaEffector2D affectedAreaEffector;

    [Header("이 아래는 되도록 건드리지 말기")]

    [Tooltip("플레이어와 기타 오브젝트와 충돌하지 않는 리지드 바디. (중요)")]
    public Rigidbody2D tornadoRigidBody;

    public Collider2D tornadoTrigger;

    public GameObject tornadoObject_Collider;
    public GameObject tornadoObject_Trigger;

    public AreaEffector2D upWindAreaEffector;

    [Tooltip("직접 움직이는 상태인지를 뜻합니다.")]
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
        //플레이어와 토네이도 콜라이더간의 충돌을 막음.
        playerLayer = LayerMask.NameToLayer("Player");
        Tornado_ColliderLayer = LayerMask.NameToLayer("Tornado_Collider");

        Physics2D.IgnoreLayerCollision(Tornado_ColliderLayer, playerLayer, true);
        //Physics2D.IgnoreLayerCollision(layer1, layer3, true);

        ////가끔 수정하다가 로컬포지션이 요상한 경우가 있어서 zero로 초기화
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
            ////영향받는 AreaEffector의 forceMagnitude가 0보다 크고...아리아 이펙터에 닿았을때...
            //if (tornadoRigidBody.IsTouchingLayers(LayerMask.NameToLayer("AreaEffector")))
            //{
                if (affectedAreaEffector.forceMagnitude !=0f)
                {
                    //직접 움직이지 않게
                    moveSelf = false;

                    //이미 다이나믹이면 변경하지 않게
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Dynamic)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
                        tornadoRigidBody.velocity = Vector2.zero;
                    }

                }
            else
            {
                //아니라면 직접 움직이는 상태로
                moveSelf = true;

                //이미 키네마틱이면 변경하지 않게
                if (tornadoRigidBody.bodyType != RigidbodyType2D.Kinematic)
                {
                    tornadoRigidBody.bodyType = RigidbodyType2D.Kinematic;
                    tornadoRigidBody.velocity = Vector2.zero;
                }
            }

            //}
            //else
            //{
            //    Debug.Log("아리아 이펙터에 안닿고 있음");
            //    //아니라면 직접 움직이는 상태로
            //    moveSelf = true;

            //    //이미 키네마틱이면 변경하지 않게
            //    if (tornadoRigidBody.bodyType != RigidbodyType2D.Kinematic)
            //    {
            //        tornadoRigidBody.bodyType = RigidbodyType2D.Kinematic;
            //        tornadoRigidBody.velocity = Vector2.zero;
            //    }

            //}
        }
        else // 써니일때
        {
            //if (tornadoRigidBody.IsTouchingLayers(LayerMask.NameToLayer("AreaEffector")))
            //{

                if (affectedAreaEffector.forceMagnitude !=0f)
                {
                    //직접 움직이지 않게
                    moveSelf = false;

                    //이미 다이나믹이면 변경하지 않게
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Dynamic)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
                        tornadoRigidBody.velocity = Vector2.zero;
                    }
                }
                else
                {
                    Debug.Log("써니 : 아리아 이펙터에 안닿고 있음");
                    //아니라면 직접 움직이는 상태로
                    moveSelf = false;

                    //이미 다이나믹이면 변경하지 않게
                    if (tornadoRigidBody.bodyType != RigidbodyType2D.Static)
                    {
                        tornadoRigidBody.bodyType = RigidbodyType2D.Static;
                    }

                }
            //}

        }

    }
    /// <summary>
    /// EnterSunny때 실행
    /// </summary>
    public IEnumerator GoSunny()
    {
        Debug.Log("GoSunny");

        gameObject.tag = "Untagged";
        moveSelf = false;

        //이펙터에 영향을 받아야 하기 때문에 다이나믹으로 변경
        tornadoRigidBody.bodyType = RigidbodyType2D.Dynamic;
        tornadoRigidBody.velocity = Vector2.zero;
        var startScale = transform.localScale;

        //1초동안 변화
        var smallTime = 1f;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / smallTime;
            tornadoRigidBody.transform.localScale = Vector2.Lerp(startScale, originalScale * 0.5f, t);
            yield return null;
        }

        //upWindAreaEffector.gameObject.transform.position = transform.position;


        //이펙터 켜기
        upWindAreaEffector.gameObject.SetActive(true);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), Tornado_ColliderLayer, true);

    }

    public IEnumerator GoRainy()
    {
        Debug.Log("GoRainy");

        gameObject.tag = "Tornado";

        //직접 움직여야함
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
                Debug.Log("플레이어가 닿았다.");
                PlayerController.Instance.ProcessDie();
            }
        }

    }
}
