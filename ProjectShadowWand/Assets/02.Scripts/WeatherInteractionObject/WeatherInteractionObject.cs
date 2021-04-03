using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
/// <summary>
/// ������ ������ �޴� ������Ʈ��.
/// </summary>
public class WeatherInteractionObject : MonoBehaviour
{
    [Tooltip("���� �� ������Ʈ�� ������ �ް��ִ� ������ ���մϴ�.")]
    public eMainWeatherType affectedWeather;
    public delegate void WeatherDelegate();
    WeatherDelegate weatherDelegate;

    public void Init()
    {
        affectedWeather = eMainWeatherType.SUNNY;
        weatherDelegate = ProcessSunny;
    }

    /// <summary>
    /// ������  ChangeState()�� ������ �� �ۿ� ���մϴ�.
    /// </summary>
    public virtual void Exectue()
    {
        weatherDelegate();
    }

    /// <summary>
    /// ���� ������ ����(�����Ŵ���.�ٸ��ο���), � �Լ��� ȣ���� ���� �ٲߴϴ�.
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


