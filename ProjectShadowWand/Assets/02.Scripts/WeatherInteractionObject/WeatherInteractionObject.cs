using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// 날씨의 영향을 받는 오브젝트들.
/// </summary>
public class WeatherInteractionObject : MonoBehaviour
{
    [Header("이 오브젝트가 받고 있는 날씨"), Tooltip("현재 이 오브젝트가 영향을 받고있는 날씨를 뜻합니다.")]
    public eMainWeatherType affectedWeather;

    public delegate void WeatherDelegate();
    WeatherDelegate weatherDelegate;

    /// <summary>
    /// 기본적으로 SUNNY로 초기화합니다.
    /// </summary>
    public virtual void Init()
    {
        affectedWeather = WeatherManager.Instance.GetMainWeather();
        ChangeDelegate(affectedWeather);
    }

    /// <summary>
    /// 아직은 weatherDelegate를 돌리는 것 밖에 안합니다.
    /// </summary>
    public virtual void Execute()
    {
        weatherDelegate();
    }

    /// <summary>
    /// 현재 날씨에 따라서(웨더매니저.겟메인웨더), 어떤 함수를 호출할 지를 바꿉니다.
    /// </summary>
    public virtual void ChangeState()
    {
        eMainWeatherType nowMainType = WeatherManager.Instance.GetMainWeather();
        if (nowMainType!=affectedWeather)
        {
            Debug.Log("Weather is Different");
            ChangeDelegate(nowMainType);
        }
        else
        {

        }


    }
    private void ChangeDelegate(eMainWeatherType _wt)
    {
        Debug.Log("ChangeDelegate");
        switch (_wt)
        {
            case eMainWeatherType.SUNNY:
                EnterSunny();
                weatherDelegate = ProcessSunny;
                break;

            case eMainWeatherType.RAINY:
                EnterRainy();
                weatherDelegate = ProcessRainy;
                break;

            default:
                weatherDelegate = ProcessSunny;
                break;
        }
        affectedWeather = _wt;
    }

    public virtual void EnterRainy() { }
    public virtual void EnterSunny() { }
    public virtual void ProcessRainy()
    {

    }
    public virtual void ProcessSunny()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Debug.Log(gameObject.name + " : 뭔가 닿았음! : " + collision.gameObject.name);


        if (collision.gameObject.CompareTag("Tornado"))
        {
            ProcessDestroy();
        }
    }

    public virtual void ProcessDestroy()
    {
        Destroy(gameObject);
    }
}


