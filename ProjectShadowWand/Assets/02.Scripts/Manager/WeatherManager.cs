using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;


// https://forum.unity.com/threads/lerp-from-one-gradient-to-another.342561/
namespace Util
{
    public static class Gradient
    {
        public static UnityEngine.Gradient Lerp(UnityEngine.Gradient a, UnityEngine.Gradient b, float t)
        {
            return Lerp(a, b, t, false, false);
        }

        public static UnityEngine.Gradient LerpNoAlpha(UnityEngine.Gradient a, UnityEngine.Gradient b, float t)
        {
            return Lerp(a, b, t, true, false);
        }

        public static UnityEngine.Gradient LerpNoColor(UnityEngine.Gradient a, UnityEngine.Gradient b, float t)
        {
            return Lerp(a, b, t, false, true);
        }

        static UnityEngine.Gradient Lerp(UnityEngine.Gradient a, UnityEngine.Gradient b, float t, bool noAlpha, bool noColor)
        {
            //list of all the unique key times
            var keysTimes = new List<float>();
            if (!noColor)
            {
                for (int i = 0; i < a.colorKeys.Length; i++)
                {
                    float k = a.colorKeys[i].time;
                    if (!keysTimes.Contains(k))
                        keysTimes.Add(k);
                }

                for (int i = 0; i < b.colorKeys.Length; i++)
                {
                    float k = b.colorKeys[i].time;
                    if (!keysTimes.Contains(k))
                        keysTimes.Add(k);
                }
            }

            if (!noAlpha)
            {
                for (int i = 0; i < a.alphaKeys.Length; i++)
                {
                    float k = a.alphaKeys[i].time;
                    if (!keysTimes.Contains(k))
                        keysTimes.Add(k);
                }

                for (int i = 0; i < b.alphaKeys.Length; i++)
                {
                    float k = b.alphaKeys[i].time;
                    if (!keysTimes.Contains(k))
                        keysTimes.Add(k);
                }
            }

            GradientColorKey[] clrs = new GradientColorKey[keysTimes.Count];
            GradientAlphaKey[] alphas = new GradientAlphaKey[keysTimes.Count];

            //Pick colors of both gradients at key times and lerp them
            for (int i = 0; i < keysTimes.Count; i++)
            {
                float key = keysTimes[i];
                var clr = Color.Lerp(a.Evaluate(key), b.Evaluate(key), t);
                clrs[i] = new GradientColorKey(clr, key);
                alphas[i] = new GradientAlphaKey(clr.a, key);
            }

            var g = new UnityEngine.Gradient();
            g.SetKeys(clrs, alphas);

            return g;
        }
    }
}

[Serializable]
public struct MainWeatherColor
{
    public Gradient SkyGradient;

    public Color globalLightColor;
    public float globalLightIntensity;

    public Color mainLightColor;
    public float mainLightIntensity;

    public float mainLightShadowIntensity;
    //public Color shadowColor;

    public ParticleSystem[] fxParticles;

    //public Color SubWeatherColor;
};

[Serializable]
public struct SubWeatherColor
{
    public float lerpIntensity;

    public Gradient SkyGradient;

    public Color globalLightColor;
    public float globalLightIntensity;

    public Color mainLightColor;
    public float mainLightIntensity;

    public float mainLightShadowIntensity;

    public ParticleSystem[] fxParticles;
}


public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    [SerializeField] private Light2D _GlobalLight2DObject;
    [SerializeField] private List<Light2D> weatherLight;

    private eMainWeatherType prevMainWeather = eMainWeatherType.SUNNY;
    [SerializeField] private eMainWeatherType nowMainWeather = eMainWeatherType.SUNNY;

    private eSubWeatherType prevSubWeather = eSubWeatherType.NONE;
    [SerializeField] private eSubWeatherType nowSubWeather = eSubWeatherType.NONE;

    [SerializeField] private MainWeatherColor[] mainWeatherColorSettings;
    [SerializeField] private SubWeatherColor[] subWeatherColorSettings;

    [SerializeField] private MainWeatherColor curMainWeatherColor = new MainWeatherColor();
    [SerializeField] private SubWeatherColor curSubWeatherColor = new SubWeatherColor();

    private bool isMainWeatherChanging = false;
    private bool isSubWeatherChanging = false;

    [SerializeField] float mainWeatherChangeSmoothTime = 5.0f;
    [SerializeField] float subWeatherChangeSmoothTime = 2.5f;

    

    //아래부터는 SerializeField? 제대로 들어오는지 직접 보기 위해서. 이후 삭제함.


    // 싱글톤 패턴
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WeatherManager.DontDestroyOnLoad(this.gameObject); // 씬 로딩을 할 때(옮겨다닐 때) 지우지마라 
        }
    }

    /// <summary> WeatherManager를 초기화합니다. 정상적으로 초기화를 완료했을 시 eErrorType.NONE을 반환합니다. 어딘가 비정상적일 시, eErrorType.MANAGER_INIT_ERROR를 반환합니다. </summary>
    public eErrorType Init()
    {
        // Light2D 중, WeatherLight 레이어에 있는 것들을 제외하고 전부 제외.
        // _GlobalLight 도 제외.
        weatherLight.AddRange(FindObjectsOfType(typeof(Light2D)) as Light2D[]);
        var toRemove = new HashSet<Light2D>();
        for(int i = 0; i<weatherLight.Count; ++i)
        {
            if(weatherLight[i].gameObject.layer != (int)eLayer.WeatherLight)
            {
                toRemove.Add(weatherLight[i]);
            }
        }
        weatherLight.RemoveAll(toRemove.Contains);
        weatherLight.Remove(_GlobalLight2DObject);

        // 현재 날씨의 초기값 설정.
        


        // 정상/비정상 반환
        if (_GlobalLight2DObject == null){ return eErrorType.MANAGER_INIT_ERROR; }
        if (weatherLight.Count == 0) { return eErrorType.MANAGER_INIT_ERROR; }

        return eErrorType.NONE;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void applyWeatherColor()
    {

    }

    IEnumerator applyMainWeather()
    {
        float mainWeatherTimer = 0;
        isMainWeatherChanging = true;
        
        while (mainWeatherTimer <= mainWeatherChangeSmoothTime)
        {
            mainWeatherTimer += Time.deltaTime;

            yield return null;
        }

        curMainWeatherColor = mainWeatherColorSettings[(int)nowMainWeather];
        isMainWeatherChanging = false;
    }

    /// <summary> 메인 날씨를 변경합니다. 날씨가 바뀌고 있는 도중이라면 false를 반환하며, 날씨가 변경되지 않습니다. 정상적으로 변경할 시 true를 반환합니다. </summary>
    public bool SetMainWeather(eMainWeatherType mWeatherType)
    {
        //날씨가 바뀌고 있는 중이면 SetWeather이 적용되지 않음.
        if(isMainWeatherChanging == true) { return false; }

        prevMainWeather = nowMainWeather;
        nowMainWeather = mWeatherType;

        StartCoroutine(applyMainWeather());

        return true;
    }

    /// <summary> 서브 날씨를 변경합니다. 날씨가 바뀌고 있는 도중이라면 false를 반환하며, 날씨가 변경되지 않습니다. 정상적으로 변경했을 시 true를 반환합니다. </summary>
    public bool SetSubWeather(eSubWeatherType sWeatherType)
    {
        if(isSubWeatherChanging == true) { return false; }

        prevSubWeather = nowSubWeather;
        nowSubWeather = sWeatherType;

        return true;
    }

    /// <summary> 현재의 메인 날씨를 반환합니다. </summary>
    public eMainWeatherType GetMainWeather()
    {
        return nowMainWeather;
    }

    public eSubWeatherType GetSubWeather()
    {
        return nowSubWeather;
    }
}
