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
        [GradientUsageAttribute(true)]
        public Gradient SkyColor;
        [GradientUsageAttribute(true)]
        public Gradient SkyGradientColor;
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
        GameManager gmInstance = FindObjectOfType<GameManager>();
#else
        WeatherManager wmInstance = WeatherManager.Instance;
        GameManager gmInstance = GameManager.Instance;
#endif

        float nowProgress = gmInstance.gameProgress;

        if (wmInstance.isMainWeatherChanging == true)
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();
            int prevMainWeather = (int)wmInstance.GetPrevMainWeather();
            float changingRatio = wmInstance.changingMainWeatherRatio;

            curSkyCol = Color.Lerp(skySettings[prevMainWeather].SkyColor.Evaluate(nowProgress), skySettings[nowMainWeather].SkyColor.Evaluate(nowProgress), changingRatio);
            curSkyGradientCol = Color.Lerp(skySettings[prevMainWeather].SkyGradientColor.Evaluate(nowProgress), skySettings[nowMainWeather].SkyGradientColor.Evaluate(nowProgress), changingRatio);
        }
        else
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();
            curSkyCol = skySettings[nowMainWeather].SkyColor.Evaluate(nowProgress);
            curSkyGradientCol = skySettings[nowMainWeather].SkyGradientColor.Evaluate(nowProgress);
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
