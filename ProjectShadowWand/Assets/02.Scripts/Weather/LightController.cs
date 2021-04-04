using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Serializable]
    public struct LightColorSetting
    {
        public eMainWeatherType weatherName;
        public Gradient color;
        public AnimationCurve lightIntensity;
        public AnimationCurve shadowIntensity;
    };
    [SerializeField] public LightColorSetting[] lightSettings;

    public struct LightColorData
    {
        public Color color;
        public float lightIntensity;
        public float shadowIntensity;
    }

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
        LightColorData curLc = new LightColorData();
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

            curLc.color = Color.Lerp(lightSettings[prevMainWeather].color.Evaluate(nowProgress), lightSettings[nowMainWeather].color.Evaluate(nowProgress), changingRatio);
            curLc.lightIntensity = Mathf.Lerp(lightSettings[prevMainWeather].lightIntensity.Evaluate(nowProgress), lightSettings[nowMainWeather].lightIntensity.Evaluate(nowProgress), changingRatio);
            curLc.shadowIntensity = Mathf.Lerp(lightSettings[prevMainWeather].shadowIntensity.Evaluate(nowProgress), lightSettings[nowMainWeather].shadowIntensity.Evaluate(nowProgress), changingRatio);
        }
        else
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();

            curLc.color = lightSettings[nowMainWeather].color.Evaluate(nowProgress);
            curLc.lightIntensity = lightSettings[nowMainWeather].lightIntensity.Evaluate(nowProgress);
            curLc.shadowIntensity =lightSettings[nowMainWeather].shadowIntensity.Evaluate(nowProgress);
        }

        for (int i =0; i<lightSetters.Length; ++i)
        {
            lightSetters[i].setLightProperty(curLc);
        }
    }
}
