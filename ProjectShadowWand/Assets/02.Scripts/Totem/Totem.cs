using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �����ϴ� Ư�� ������Ʈ
/// </summary>
public class Totem : MonoBehaviour
{
    [Header("��ȣ�ۿ� �� �ش� ������ ����� ")]
    [Tooltip("��...������?")]
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
            //���� ���� �ٲٴ°�
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
