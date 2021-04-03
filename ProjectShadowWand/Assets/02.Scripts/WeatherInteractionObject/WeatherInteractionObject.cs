using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
/// <summary>
/// 날씨의 영향을 받는 오브젝트들.
/// </summary>
public class WeatherInteractionObject : MonoBehaviour
{
    [Tooltip("현재 이 오브젝트가 영향을 받고있는 날씨를 뜻합니다.")]
    public eMainWeatherType affectedWeather;
    public delegate void WeatherDelegate();
    WeatherDelegate weatherDelegate;

    private void Awake()
    {
        
    }

    public void Init()
    {
        affectedWeather = eMainWeatherType.SUNNY;
        weatherDelegate = ProcessSunny;
    }

    public virtual void Exectue()
    {
        ChangeState();
    }
    public virtual void ChangeState()
    {
        eMainWeatherType nowMainType = WeatherManager.Instance.GetMainWeather();
        if (nowMainType!=affectedWeather)
        {
            Debug.Log("Weather is Different");
            switch (WeatherManager.Instance.GetMainWeather())
            {
                case eMainWeatherType.SUNNY:
                    weatherDelegate = ProcessSunny;
                break;

                case eMainWeatherType.RAINY:
                    weatherDelegate = ProcessRainy;
                    break;

                default:
                    weatherDelegate = ProcessSunny;
                    break;
            }
            affectedWeather = nowMainType;
        }
        else
        {

        }


    }

    public virtual void ProcessRainy()
    {

    }
    public virtual void ProcessSunny()
    {

    }
}


