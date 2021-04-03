using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyTotem : Totem
{
    public GameObject rainEffect;
    private void CheckingInput()
    {
        if (InputManager.Instance.buttonCatch.wasPressedThisFrame
            && isPlayerIn == true)
        {
            Debug.Log("is Change");

            isOn = !isOn;
            if (WeatherManager.Instance.SetMainWeather(eMainWeatherType.SUNNY) == true)
            {
                isOn = true;

            }



        }
    }
    private void Update()
    {
        CheckingInput();


        if (isOn)
        {
            if (rainEffect.activeSelf == true)
            {
                sr.color = Color.blue;
                rainEffect.SetActive(false);
            }

        }
        else
        {
            if (rainEffect.activeSelf == false)
            {

                sr.color = Color.red;
                rainEffect.SetActive(true);
            }


        }

        
    }
}
