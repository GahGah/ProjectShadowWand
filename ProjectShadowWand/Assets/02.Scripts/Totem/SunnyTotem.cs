using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyTotem : Totem
{
    public GameObject rainEffect;




    private void Awake()
    {
        mainWeatherType = eMainWeatherType.SUNNY;
    }
    private void OnEnable()
    {
        SetDefaultWeatherType();
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
                //if (rainEffect.activeSelf == true)
                //{
                    rainEffect.SetActive(false);
                    isOn = true;
                //}

            }
            else
            {
                //if (rainEffect.activeSelf == false)
                //{
                    rainEffect.SetActive(true);
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
}