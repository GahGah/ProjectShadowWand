using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ������ �Ҿ�� ������Ʈ�Դϴ�.
/// </summary>
public class WaterObject : WeatherInteractionObject
{
    private PolygonCollider2D waterCollider = null;

    private Vector2 originalPosition = Vector2.zero;
    private Vector2 originalScale = Vector2.zero;

    [Header("���� �������� �ִ�ġ"), Tooltip("��� �������� �󸶸�ŭ �������� ���մϴ�.")]
    public Vector2 maxExtent;
    [Header("���� �������� �ӵ�(��)"), Tooltip("�ش� �� �ȿ� �ִ�ġ(increaseRange)�� �����մϴ�.")]
    public float changeTime;

    [Header("���� �������� �������� �߾��ΰ�"), Tooltip("true��� �߾��� �������� ũ�Ⱑ �þ�ϴ�. \n �⺻�� : false ")]
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
        Debug.Log("����");
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
        ChangeExtentCoroutine = null; // null�� ����
    }

    /// <summary>
    /// �����ŵ�ϴ�.
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
        if (ChangeExtentCoroutine != null) //�̹� ����Ǿ��ִ� ���¿��ٸ�
        {
            StopCoroutine(ChangeExtentCoroutine); //���߱�
        }
        else
        {
            ChangeExtentCoroutine = DoChangeExtent();
        }
        StartCoroutine(ChangeExtentCoroutine);
    }

    public override void EnterSunny()
    {
        if (ChangeExtentCoroutine != null) //�̹� ����Ǿ��ִ� ���¿��ٸ�
        {
            StopCoroutine(ChangeExtentCoroutine); //���߱�
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
        if (collision.CompareTag("Player")) //�÷��̾ ���Դٸ�
        {
            PlayerController.Instance.SetIsWater(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //�÷��̾ �����ٸ�
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
