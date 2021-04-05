using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyTotem : Totem
{
    [Header("비 이펙트 ")]
    public GameObject rainEffect;

    [Header("토템의 활성화 시간")]
    public float activeTime;

    [Tooltip("활성화 되어있는 시간...")]
    private float currentTime;

    private bool isTimer;

    private void Awake()
    {
        Init();
        SetDefaultWeatherType();
    }

    protected override void Init()
    {
        base.Init();
        mainWeatherType = eMainWeatherType.SUNNY;
        currentTime = 0f;
        isTimer = false;
    }

    private void Update()
    {
        ChangeCanUse();
        CheckingInput();
        Execute();
    }

    protected override void CheckingInput()
    {
        if (isInteractable)
        {
            if (canUse && isTimer == false)
            {
                if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isPlayerIn == true)
                {
                    Debug.Log("is Change");

                    if (WeatherManager.Instance.GetMainWeather() != mainWeatherType)
                    {
                        WeatherManager.Instance.SetMainWeather(mainWeatherType);
                    }
                    else
                    {

                    }
                }
            }

        }
    }

    public override void Execute()
    {
        if (canUse)
        {
            if (WeatherManager.Instance.GetMainWeather() == mainWeatherType)
            {
                //if (rainEffect.activeSelf == true)
                //{
                rainEffect.SetActive(false);

                if (isOn == false)
                {
                    isOn = true;
                    StartCoroutine(ProcessOnSunny());
                }
                //}

            }
            else
            {
                //if (rainEffect.activeSelf == false)
                //{
                rainEffect.SetActive(true);
                if (isOn == true)
                {
                    isOn = false;
                }

                //}

            }


        }

        ColorChange();
    }

    public override void ChangeCanUse()
    {
        base.ChangeCanUse();
    }

    public IEnumerator ProcessOnSunny()
    {
        currentTime = 0f;

        isTimer = true;

        while (currentTime < activeTime) //타이머가 액티브 타임보다 적을 때 까지
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        
        var test = WeatherManager.Instance.SetMainWeather(defaultWeatherType);
        Debug.Log("메인 웨더 체인지 시도: " + test);
        isTimer = false;
    }
}