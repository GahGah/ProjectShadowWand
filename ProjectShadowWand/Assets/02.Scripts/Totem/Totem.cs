using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 날씨를 변경하는 특수 오브젝트
/// </summary>
public class Totem : MonoBehaviour
{
    [Header("상호작용 시 해당 날씨로 변경됨 ")]
    [Tooltip("아...뭐라쓰지?")]
    public eMainWeatherType mainWeatherType;
    public eSubWeatherType subWeatherType;


    public bool canUse;
    public bool isOn;

    public void ChangeWeather()
    {

        if (canUse)
        {
            //뭔가 날씨 바꾸는거
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
