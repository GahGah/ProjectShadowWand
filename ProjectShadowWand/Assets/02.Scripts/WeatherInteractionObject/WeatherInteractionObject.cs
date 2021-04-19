using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// ������ ������ �޴� ������Ʈ��.
/// </summary>
public class WeatherInteractionObject : MonoBehaviour
{
    [Header("�� ������Ʈ�� �ް� �ִ� ����"), Tooltip("���� �� ������Ʈ�� ������ �ް��ִ� ������ ���մϴ�.")]
    public eMainWeatherType affectedWeather;

    public delegate void WeatherDelegate();
    WeatherDelegate weatherDelegate;

    [Tooltip("ChangeState�� GetMainWeatherType�� ��ȯ���� ������ �ֽ��ϴ�.")]
    protected eMainWeatherType currentMainWeatherType;

    /// <summary>
    /// �⺻������ SUNNY�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public virtual void Init()
    {
        currentMainWeatherType = WeatherManager.Instance.GetMainWeather();
        affectedWeather = WeatherManager.Instance.GetMainWeather();
        ChangeDelegate(affectedWeather);
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
        currentMainWeatherType= WeatherManager.Instance.GetMainWeather();
        if (currentMainWeatherType != affectedWeather)
        {
            Debug.Log("Weather is Different");
            ChangeDelegate(currentMainWeatherType);
        }
        else
        {

        }
    }

    protected void ChangeDelegate(eMainWeatherType _wt)
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
        // Debug.Log(gameObject.name + " : ���� �����! : " + collision.gameObject.name);


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


