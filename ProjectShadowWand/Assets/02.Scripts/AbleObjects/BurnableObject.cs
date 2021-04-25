using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    [Header("������ / �ҿ���")]
    public eBurnableType burnType;

    [Tooltip("Ż �� �ִ� �����ΰ�? �ν������� ������ ������ ��Ĩ�ϴ�.")]
    public bool canBurn = true;

    [Tooltip("Ÿ�� ���ΰ�?")]
    public bool isBurning = false;

    [Tooltip("�ѹ��̶� ź ���� �ִ°�?")]
    public bool isBurned = false;

    [Tooltip("���� ���� ������Ʈ")]
    public FireObject fireObject;

    [Tooltip("�� ������Ʈ�� ���� ���̾� ")]
    private int originalLayer;

    [Header("[�ҿ��� ������Ʈ]")]
    [Tooltip("���Ⱑ ���� ���ΰ�?")]
    public bool isSmoking = false;

    IEnumerator BurningCoroutine;

    [Header("[������ ������Ʈ]")]
    [Tooltip("���� �پ��� ��, �ش� �ʰ� ������ ������Ʈ�� �ı��˴ϴ�.")]
    public float liveTime;
    private float currentTime;
    [Tooltip("1�� �������� ź �̴ϴ�.")]
    private float burnPer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Tooltip("��İ� ���°�?")]
    private bool isBlack;
    //[Tooltip("���� ���� ���� ������ ���� �����ΰ�?")]
    //public bool isWet;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (liveTime == 0f)
        {
            liveTime = 5f;
        }
        originalLayer = gameObject.layer;
        BurningCoroutine = null;
        currentTime = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a);
        isBlack = false;

        if (fireObject == null)
        {
            isBurning = false;
        }
    }

    private void Update()
    {
        if (isBurned)
        {
            canBurn = false;
        }

        if (fireObject == null)
        {
            isBurning = false;
            gameObject.layer = originalLayer;

            if (BurningCoroutine != null) //���ư��� �ִ� ���¶��
            {
                StopCoroutine(BurningCoroutine);
                BurningCoroutine = null;
            }
        }
        else //���� �پ��� ���
        {

            if (burnType == eBurnableType.BURN) //�������̶��
            {
                if (BurningCoroutine == null) //�� �ڷ�ƾ�� ���� ����Ǿ����� �ʾҴٸ�
                {
                    BurningCoroutine = ProcessBurning();
                    StartCoroutine(BurningCoroutine);
                }
            }
            else
            {

            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            if (BurningCoroutine != null)//Ÿ���־��� ��쿡
            {
                StopCoroutine(BurningCoroutine); //Ÿ�°� �����Ű��
                BurningCoroutine = null;

                if (fireObject != null)
                {
                    fireObject.DestroyFireObject();
                    fireObject = null;
                }

            }
        }

        if (isBlack)
        {
            if (BurningCoroutine != null)//Ÿ���־��� ��쿡
            {
                StopCoroutine(BurningCoroutine); //Ÿ�°� �����Ű��
                BurningCoroutine = null;

                fireObject.DestroyFireObject();
                fireObject = null;
            }
            if (fireObject != null) //������ ��Ÿ�� �ִٸ�
            {
                fireObject.DestroyFireObject();
                fireObject = null;
            }

            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("�� ����");
        if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex)//�� ������
        {
            if (BurningCoroutine != null)//Ÿ���־��� ��쿡
            {
                StopCoroutine(BurningCoroutine); //Ÿ�°� �����Ű��
                BurningCoroutine = null;

                fireObject.DestroyFireObject();
                fireObject = null;
            }
        }
        else
        {

        }
    }
    /// <summary>
    ///  isBurning�� true��� �¿� ������(?)�� �ö󰩴ϴ�. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ProcessBurning()
    {
        var shouldDestroy = false;
        while (isBurning)
        {
            currentTime += Time.deltaTime;

            burnPer = currentTime / liveTime;
            if (burnPer < 1f)
            {
                spriteRenderer.color = Color.Lerp(originalColor, Color.black, burnPer);
            }
            else
            {
                spriteRenderer.color = Color.black;
                isBlack = true;
                shouldDestroy = true;
                break;
            }

            yield return null;
        }

        if (shouldDestroy == true) //�ı��ؾ��� ���
        {
            fireObject.DestroyFireObject();
        }
    }
}
