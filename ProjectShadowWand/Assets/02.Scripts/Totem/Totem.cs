using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// 날씨를 변경하는 특수 오브젝트
/// </summary>
public class Totem : MonoBehaviour
{
    [Header("상호작용 시 해당 날씨로 변경됨 ")]
    [Tooltip("아...뭐라쓰지?")]
    public eMainWeatherType mainWeatherType;
    public eSubWeatherType subWeatherType;

    public SpriteRenderer sr;

    public Light2D totemLight;

    [HideInInspector] public bool canUse = true;
    [HideInInspector] public bool isOn = false;

    protected bool isPlayerIn = false;

    [HideInInspector] public bool isInteractable = true;
    protected eMainWeatherType defaultWeatherType;

    protected virtual void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        totemLight = GetComponentInChildren<Light2D>();
        canUse = true;
        isOn = false;
        isPlayerIn = false;
        isInteractable = true;

        sr.color = Color.white;
        totemLight.gameObject.SetActive(false);
    }

    protected virtual void CheckingInput()
    {
        if (isInteractable)
        {
            if (canUse)
            {

                if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isPlayerIn == true)
                {
                    Debug.Log("is Change");

                    if (WeatherManager.Instance.GetMainWeather() != mainWeatherType)
                    {
                        WeatherManager.Instance.SetMainWeather(mainWeatherType);
                    }
                    else
                    {
                    }
                }
            }
        }


    }
    /// <summary>
    /// 이게 그냥 StageWeatherType이라는 게 될 수 있음. 주의하기.
    /// 그렇다면, 그냥 ChangeCanUse에서 디폴트웨더타입 대신 스테이지 웨더 ㅌ타입을 사용하면 됨.
    /// </summary>
    protected void SetDefaultWeatherType()
    {
        defaultWeatherType = WeatherManager.Instance.GetMainWeather();
    }
    /// <summary>
    /// 만약 토템이 담당하는 날씨와 현재 날씨가 같다면, 토템을 종료시키고 사용할 수 없는 상태로 만듭니다.
    /// </summary>
    public virtual void ChangeCanUse()
    {

        //메인 웨더 타입이 스테이지 기본 타입과 현재 타입이랑 같아야  사용 불가.

        //하지만 메인 웨더 타입이 현재 타입이랑 같다 : 그냥 활성화된 상태.
        //하지만 메인 웨더 타입이 기본 타입이랑 같다 : 쓸 수 없는 상태.

        //하지만 여기서 문제가 되는건, 다른 토템에 의해서 현재 타입이 바뀌었을 때.
        //그렇다면, 메인웨더 타입과 현재 타입이 달라짐. 즉, false를 반환함.
        if (mainWeatherType == defaultWeatherType
            && mainWeatherType == WeatherManager.Instance.GetMainWeather())

        {
            canUse = false;
        }
        else
        {
            canUse = true;
        }
    }

    public void ChangeWeather()
    {
        if (canUse)
        {
            ChangeMainWeather();
        }
        else
        {

        }

    }
    public bool ChangeMainWeather()
    {
        return WeatherManager.Instance.SetMainWeather(mainWeatherType);
    }

    public bool ChangeSubWeather()
    {
        return WeatherManager.Instance.SetSubWeather(subWeatherType);
    }

    public virtual void Execute()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
        }

    }



    protected void ColorChange()
    {
        if (canUse == false)
        {
            sr.color = Color.black;
        }
        else
        {
            if (isOn)
            {
                sr.color = Color.white;
                totemLight.gameObject.SetActive(true);
            }
            else
            {
                sr.color = Color.white;
                totemLight.gameObject.SetActive(false);

            }
        }

    }
}
