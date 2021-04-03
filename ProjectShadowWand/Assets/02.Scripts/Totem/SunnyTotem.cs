using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyTotem : Totem
{
    public GameObject rainEffect;
    private void CheckingInput()
    {
        if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isPlayerIn == true)
        {
            Debug.Log("is Change");

            if (WeatherManager.Instance.GetMainWeather() != eMainWeatherType.SUNNY)
            {
                WeatherManager.Instance.SetMainWeather(eMainWeatherType.SUNNY);
            }

        }
    }

    private void Update()
    {
        CheckingInput();
        Execute();

    }

    public override void Execute()
    {
        if (WeatherManager.Instance.GetMainWeather() == eMainWeatherType.SUNNY)
        {
            if (rainEffect.activeSelf == true)
            {

                rainEffect.SetActive(false);
                isOn = true;
            }

        }
        else
        {
            if (rainEffect.activeSelf == false)
            {
                rainEffect.SetActive(true);
                isOn = false;
            }

        }

        ColorChange();
    }

}