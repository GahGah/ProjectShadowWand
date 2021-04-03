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

    public void Init()
    {
        affectedWeather = eMainWeatherType.SUNNY;
        weatherDelegate = ProcessSunny;
    }

    /// <summary>
    /// 아직은  ChangeState()를 돌리는 것 밖에 안합니다.
    /// </summary>
    public virtual void Exectue()
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


