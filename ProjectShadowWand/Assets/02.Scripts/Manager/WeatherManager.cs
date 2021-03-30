using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct WeatherColor
{
    Gradient SkyGradient;

    float globalLightIntensity;
    Color globalLightColor;

    float mainLightIntensity;
    Color mainLightColor;

    float mainLightShadowIntensity;
    //Color shadowColor;

};

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;
    
    private eWeatherType prevWeather = eWeatherType.SUNNY;
    [SerializeField] private eWeatherType nowWeather = eWeatherType.SUNNY;

    [SerializeField] private Dictionary<eWeatherType, WeatherColor> weatherSettings = new Dictionary<eWeatherType, WeatherColor>();

    [SerializeField] private WeatherColor curWeatherColor = new WeatherColor();

    // �̱��� ����
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WeatherManager.DontDestroyOnLoad(this.gameObject); // �� �ε��� �� ��(�Űܴٴ� ��) ���������� 
        }
    }

    public int Init()
    {
        return 0;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void WeatherChaneged()
    {
        //curWeather
    }

    public void SetWeather(eWeatherType weatherType)
    {
        prevWeather = nowWeather;
        nowWeather = weatherType;
    }

    public void AddWeather(eWeatherType weatherType)
    {
        prevWeather = nowWeather;
        nowWeather |= weatherType;
    }

    public void RemoveWeather(eWeatherType weatherType)
    {
        prevWeather = nowWeather;
        nowWeather &= ~weatherType;
    }

    public eWeatherType GetNowWeather()
    {
        return nowWeather;
    }
}
