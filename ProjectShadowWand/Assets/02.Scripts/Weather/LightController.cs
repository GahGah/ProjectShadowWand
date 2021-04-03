using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Serializable]
    public struct LightColor
    {
        public eMainWeatherType weatherName;
        public Color color;
        public float lightIntensity;
        public float shadowIntensity;
    };
    [SerializeField] public LightColor[] lightSettings;

    private LightSetter[] lightSetters;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        LightControl();
    }

    /*--------------*/

    private void Init()
    {
        lightSetters = GetComponentsInChildren<LightSetter>();
    }

    private void LightControl()
    {
        LightColor curLc = new LightColor();

        if (WeatherManager.Instance.isMainWeatherChanging == true)
        {
            int nowMainWeather = (int)WeatherManager.Instance.GetMainWeather();
            int prevMainWeather = (int)WeatherManager.Instance.GetPrevMainWeather();
            float changingRatio = WeatherManager.Instance.changingMainWeatherRatio;

            curLc.weatherName = lightSettings[nowMainWeather].weatherName;
            curLc.color = Color.Lerp(lightSettings[prevMainWeather].color, lightSettings[nowMainWeather].color, changingRatio);
            curLc.lightIntensity = Mathf.Lerp(lightSettings[prevMainWeather].lightIntensity, lightSettings[nowMainWeather].lightIntensity, changingRatio);
            curLc.shadowIntensity = Mathf.Lerp(lightSettings[prevMainWeather].shadowIntensity, lightSettings[nowMainWeather].shadowIntensity, changingRatio);
        }
        else
        {
            curLc = lightSettings[(int)WeatherManager.Instance.GetMainWeather()];
        }

        for(int i =0; i<lightSetters.Length; ++i)
        {
            lightSetters[i].setLightProperty(curLc);
        }
    }
}
