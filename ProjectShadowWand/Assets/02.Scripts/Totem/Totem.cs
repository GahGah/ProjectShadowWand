using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 날씨를 변경하는 특수 오브젝트
/// </summary>
public class Totem : MonoBehaviour
{
    [Header("상호작용 시 해당 날씨로 변경됨 ")]
    [Tooltip("아...뭐라쓰지?")]
    public eMainWeatherType mainWeatherType;
    public eSubWeatherType subWeatherType;

    public SpriteRenderer sr;

    public bool canUse;
    public bool isOn;

    protected bool isPlayerIn = false;

    public void ChangeWeather()
    {
        if (canUse)
        {
            //뭔가 날씨 바꾸는거
        }

    }
    public bool ChangeMainWeather()
    {
        return WeatherManager.Instance.SetMainWeather(mainWeatherType);
    }

    public bool ChangeSubWeather()
    {
        return WeatherManager.Instance.SetSubWeather(subWeatherType);
    }

    public virtual void Execute()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
        }

    }

    protected void ColorChange()
    {
        if (isOn)
        {
            sr.color = Color.blue;
        }
        else
        {
            sr.color = Color.red;
        }
    }
}
