using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����ϴ� Ư�� ������Ʈ
/// </summary>
public class Totem : MonoBehaviour
{
    [Header("��ȣ�ۿ� �� �ش� ������ ����� ")]
    [Tooltip("��...������?")]
    public eMainWeatherType mainWeatherType;
    public eSubWeatherType subWeatherType;


    public bool canUse;
    public bool isOn;

    public void ChangeWeather()
    {

        if (canUse)
        {
            //���� ���� �ٲٴ°�
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
}
