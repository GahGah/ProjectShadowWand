using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyTotem : Totem
{
    public GameObject rainEffect;

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
                    rainEffect.SetActive(true);
                    isOn = true;
                //}

            }
            else
            {
                //if (rainEffect.activeSelf == true)
                //{
                    rainEffect.SetActive(false);
                    isOn = false;

                //}

            }

        }

        ColorChange();
    }

    public override void ChangeCanUse()
    {
        base.ChangeCanUse();
    }
    protected override void CheckingInput()
    {
        base.CheckingInput();
    }
}
