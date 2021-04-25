using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    [Header("가연성 / 불연성")]
    public eBurnableType burnType;

    [Tooltip("탈 수 있는 상태인가? 인스펙터의 수정이 영향을 끼칩니다.")]
    public bool canBurn = true;

    [Tooltip("타는 중인가?")]
    public bool isBurning = false;

    [Tooltip("한번이라도 탄 적이 있는가?")]
    public bool isBurned = false;

    [Tooltip("붙은 불의 오브젝트")]
    public FireObject fireObject;

    [Tooltip("이 오브젝트의 원래 레이어 ")]
    private int originalLayer;

    [Header("[불연성 오브젝트]")]
    [Tooltip("연기가 나는 중인가?")]
    public bool isSmoking = false;

    IEnumerator BurningCoroutine;

    [Header("[가연성 오브젝트]")]
    [Tooltip("불이 붙었을 때, 해당 초가 지나면 오브젝트가 파괴됩니다.")]
    public float liveTime;
    private float currentTime;
    [Tooltip("1에 가까울수록 탄 겁니다.")]
    private float burnPer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Tooltip("까맣게 탔는가?")]
    private bool isBlack;
    //[Tooltip("물로 인해 불이 꺼져서 젖은 상태인가?")]
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

            if (BurningCoroutine != null) //돌아가고 있는 상태라면
            {
                StopCoroutine(BurningCoroutine);
                BurningCoroutine = null;
            }
        }
        else //불이 붙었을 경우
        {

            if (burnType == eBurnableType.BURN) //가연성이라면
            {
                if (BurningCoroutine == null) //불 코루틴이 아직 실행되어지지 않았다면
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
            if (BurningCoroutine != null)//타고있었을 경우에
            {
                StopCoroutine(BurningCoroutine); //타는걸 종료시키고
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
            if (BurningCoroutine != null)//타고있었을 경우에
            {
                StopCoroutine(BurningCoroutine); //타는걸 종료시키고
                BurningCoroutine = null;

                fireObject.DestroyFireObject();
                fireObject = null;
            }
            if (fireObject != null) //아직도 불타고 있다면
            {
                fireObject.DestroyFireObject();
                fireObject = null;
            }

            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("비에 닿음");
        if (other.layer == (int)eLayer.WeatherFx_withOpaqueTex)//비에 닿으면
        {
            if (BurningCoroutine != null)//타고있었을 경우에
            {
                StopCoroutine(BurningCoroutine); //타는걸 종료시키고
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
    ///  isBurning이 true라면 태움 게이지(?)가 올라갑니다. 
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

        if (shouldDestroy == true) //파괴해야할 경우
        {
            fireObject.DestroyFireObject();
        }
    }
}
