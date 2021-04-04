using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class SkyController : MonoBehaviour
{
    [Serializable]
    public struct SkyWeatherColorSetting
    {
        public eMainWeatherType weatherName;
        [ColorUsageAttribute(true, true)]
        public Color SkyColor;
        [ColorUsageAttribute(true, true)]
        public Color SkyGradientColor;
    };
    [SerializeField] public SkyWeatherColorSetting[] skySettings;

    private SkyColorSetter[] skyColorSetters;
    private SkyGradientSetter[] skyGradientSetters;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        skyControl();
    }

    /*---------------*/

    private void Init()
    {
        skyColorSetters = GetComponentsInChildren<SkyColorSetter>();
        skyGradientSetters = GetComponentsInChildren<SkyGradientSetter>();
    }

    private void skyControl()
    {
        Color curSkyCol = new Color();
        Color curSkyGradientCol = new Color();
#if UNITY_EDITOR
        WeatherManager wmInstance = FindObjectOfType<WeatherManager>();
#else
        WeatherManager wmInstance = WeatherManager.Instance;
#endif

        if (wmInstance.isMainWeatherChanging == true)
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();
            int prevMainWeather = (int)wmInstance.GetPrevMainWeather();
            float changingRatio = wmInstance.changingMainWeatherRatio;

            curSkyCol = Color.Lerp(skySettings[prevMainWeather].SkyColor, skySettings[nowMainWeather].SkyColor, changingRatio);
            curSkyGradientCol = Color.Lerp(skySettings[prevMainWeather].SkyGradientColor, skySettings[nowMainWeather].SkyGradientColor, changingRatio);
        }
        else
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();
            curSkyCol = skySettings[nowMainWeather].SkyColor;
            curSkyGradientCol = skySettings[nowMainWeather].SkyGradientColor;
        }

        

        for(int i = 0; i<skyColorSetters.Length; ++i)
        {
            skyColorSetters[i].setColorProperty(curSkyCol);
        }

        for(int i = 0; i<skyGradientSetters.Length; ++i)
        {
            skyGradientSetters[i].setColorProperty(curSkyGradientCol);
        }
    }
}
