using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

namespace Util
{
    // https://forum.unity.com/threads/lerp-from-one-gradient-to-another.342561/
    // �׶���Ʈ Lerp. �� �ּҿ��� ������ �״�� ���. ���� ������ �Ǹ� �ڽ��� �ڵ�� �� �ۼ� �ʿ�.
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

    // �̱��� ����
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WeatherManager.DontDestroyOnLoad(this.gameObject); // �� �ε��� �� ��(�Űܴٴ� ��) ���������� 
        }
    }


    /// <summary> WeatherManager�� �ʱ�ȭ�մϴ�. ���������� �ʱ�ȭ�� �Ϸ����� �� eErrorType.NONE�� ��ȯ�մϴ�. ��� ���������� ��, eErrorType.MANAGER_INIT_ERROR�� ��ȯ�մϴ�. </summary>
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

    /// <summary> ���� ������ �����մϴ�. ������ �ٲ�� �ִ� �����̶�� false�� ��ȯ�ϸ�, ������ ������� �ʽ��ϴ�. ���������� ������ �� true�� ��ȯ�մϴ�. </summary>
    public bool SetMainWeather(eMainWeatherType mWeatherType)
    {
        //������ �ٲ�� �ִ� ���̸� SetWeather�� ������� ����.
        if(isMainWeatherChanging == true) { return false; }

        prevMainWeather = nowMainWeather;
        nowMainWeather = mWeatherType;

        StartCoroutine(runMainWeatherTimer());

        return true;
    }

    /// <summary> [Legacy] ���� ������ �����մϴ�. ������ �ٲ�� �ִ� �����̶�� false�� ��ȯ�ϸ�, ������ ������� �ʽ��ϴ�. ���������� �������� �� true�� ��ȯ�մϴ�. </summary>
    public bool SetSubWeather(eSubWeatherType sWeatherType)
    {
        if(isSubWeatherChanging == true) { return false; }

        prevSubWeather = nowSubWeather;
        nowSubWeather = sWeatherType;

        return true;
    }

    /// <summary> ������ ���� ������ ��ȯ�մϴ�. </summary>
    public eMainWeatherType GetMainWeather()
    {
        return nowMainWeather;
    }

    /// <summary> [Legacy] ������ ���� ������ ��ȯ�մϴ�. </summary>
    public eSubWeatherType GetSubWeather()
    {
        return nowSubWeather;
    }

    /// <summary> ������ ���� ������ ��ȯ�մϴ�. </summary>
    public eMainWeatherType GetPrevMainWeather()
    {
        return prevMainWeather;
    }

    /// <summary> [Legacy] ������ ���� ������ ��ȯ�մϴ�. </summary>
    public eSubWeatherType GetPrevSubWeather()
    {
        return prevSubWeather;
    }
}
