using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderTotem : Totem
{
    //[Header("비 이펙트 ")]
    //public GameObject rainEffect;

    [Tooltip("번개의 위치를 지정할 수 있는 돌~")]
    public ThunderStone thunderStone;
    public ThunderObject thunderObject;

    private void Awake()
    {
        Init();
        SetDefaultWeatherType();
    }

    protected override void Init()
    {
        base.Init();
        mainWeatherType = eMainWeatherType.RAINY;
    }

    private void Update()
    {
        ChangeCanUse();
        CheckingInput();
        Execute();
    }

    public override void Execute()
    {
        if (canUse)
        {
            if (WeatherManager.Instance.GetMainWeather() == mainWeatherType)
            {
                //if (rainEffect.activeSelf == false)
                //{
                //rainEffect.SetActive(true);
                isOn = true;
                //}

            }
            else
            {
                //if (rainEffect.activeSelf == true)
                //{
                //rainEffect.SetActive(false);
                isOn = false;

                //}

            }

        }

        ColorChange();
    }

    public override void ChangeCanUse()
    {
        //base.ChangeCanUse();
        if (WeatherManager.Instance.GetMainWeather()==eMainWeatherType.RAINY)
        {
            canUse = true;
        }
        else
        {
            canUse = false;
        }
    }
    protected override void CheckingInput()
    {
        if (isInteractable)
        {
            if (canUse)
            {

                if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isPlayerIn == true)
                {
                    if (WeatherManager.Instance.GetMainWeather() == mainWeatherType) // == eMainWeatherType.RAINY;
                    {
                       //썬더
                    }
                    else
                    {
                    }
                }
            }
        }
    }
}
