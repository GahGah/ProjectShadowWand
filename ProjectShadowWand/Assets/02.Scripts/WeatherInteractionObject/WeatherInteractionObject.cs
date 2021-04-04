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

    /// <summary>
    /// �⺻������ SUNNY�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public virtual void Init()
    {
        affectedWeather = eMainWeatherType.SUNNY;
        weatherDelegate = ProcessSunny;
    }

    /// <summary>
    /// ������ weatherDelegate�� ������ �� �ۿ� ���մϴ�.
    /// </summary>
    public virtual void Execute()
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
            affectedWeather = nowMainType;
        }
        else
        {

        }


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
        Debug.Log("���� �����!");
        Debug.Log(collision.gameObject.name);


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


