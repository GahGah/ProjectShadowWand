using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

namespace Util
{
    // https://forum.unity.com/threads/lerp-from-one-gradient-to-another.342561/
    // 그라디언트 Lerp. 위 주소에서 복제해 그대로 사용. 이후 문제가 되면 자신의 코드로 재 작성 필요.
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

//[ExecuteInEditMode]
public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    private eMainWeatherType prevMainWeather = eMainWeatherType.SUNNY;
    [SerializeField] private eMainWeatherType nowMainWeather = eMainWeatherType.SUNNY;

    private eSubWeatherType prevSubWeather = eSubWeatherType.NONE;
    [SerializeField] private eSubWeatherType nowSubWeather = eSubWeatherType.NONE;

    public bool isMainWeatherChanging = false;
    public bool isSubWeatherChanging = false;

    [SerializeField] private float mainWeatherChangeSmoothTime = 5.0f;
    [SerializeField] private float subWeatherChangeSmoothTime = 2.5f;

    public float changingMainWeatherRatio = 0;

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
        return eErrorType.NONE;
    }

    IEnumerator runMainWeatherTimer()
    {
        float mainWeatherTimer = 0;
        isMainWeatherChanging = true;
        
        while (mainWeatherTimer <= mainWeatherChangeSmoothTime)
        {
            mainWeatherTimer += Time.deltaTime;
            changingMainWeatherRatio = mainWeatherTimer / mainWeatherChangeSmoothTime;

            yield return null;
        }

        isMainWeatherChanging = false;
    }

    /// <summary> 메인 날씨를 변경합니다. 날씨가 바뀌고 있는 도중이라면 false를 반환하며, 날씨가 변경되지 않습니다. 정상적으로 변경할 시 true를 반환합니다. </summary>
    public bool SetMainWeather(eMainWeatherType mWeatherType)
    {
        //날씨가 바뀌고 있는 중이면 SetWeather이 적용되지 않음.
        if(isMainWeatherChanging == true) { return false; }

        prevMainWeather = nowMainWeather;
        nowMainWeather = mWeatherType;

        StartCoroutine(runMainWeatherTimer());

        return true;
    }

    /// <summary> [Legacy] 서브 날씨를 변경합니다. 날씨가 바뀌고 있는 도중이라면 false를 반환하며, 날씨가 변경되지 않습니다. 정상적으로 변경했을 시 true를 반환합니다. </summary>
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

    /// <summary> [Legacy] 현재의 서브 날씨를 반환합니다. </summary>
    public eSubWeatherType GetSubWeather()
    {
        return nowSubWeather;
    }

    /// <summary> 이전의 메인 날씨를 반환합니다. </summary>
    public eMainWeatherType GetPrevMainWeather()
    {
        return prevMainWeather;
    }

    /// <summary> [Legacy] 이전의 서브 날씨를 반환합니다. </summary>
    public eSubWeatherType GetPrevSubWeather()
    {
        return prevSubWeather;
    }
}
