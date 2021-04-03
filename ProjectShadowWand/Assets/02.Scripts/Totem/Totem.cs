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

    public bool canUse = true;
    public bool isOn = false;

    protected bool isPlayerIn = false;

    public bool isInteractable = true;
    protected eMainWeatherType defaultWeatherType;

    protected virtual void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        canUse = true;
        isOn = false;
        isPlayerIn = false;
        isInteractable = true;
    }
    protected virtual void CheckingInput()
    {
        if (isInteractable)
        {
            if (canUse)
            {

                if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isPlayerIn == true)
                {
                    Debug.Log("is Change");

                    if (WeatherManager.Instance.GetMainWeather() !=mainWeatherType)
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
    /// <summary>
    /// �̰� �׳� StageWeatherType�̶�� �� �� �� ����. �����ϱ�.
    /// �׷��ٸ�, �׳� ChangeCanUse���� ����Ʈ����Ÿ�� ��� �������� ���� ��Ÿ���� ����ϸ� ��.
    /// </summary>
    protected void SetDefaultWeatherType()
    {
        defaultWeatherType = WeatherManager.Instance.GetMainWeather();
    }
    /// <summary>
    /// ���� ������ ����ϴ� ������ ���� ������ ���ٸ�, ������ �����Ű�� ����� �� ���� ���·� ����ϴ�.
    /// </summary>
    public virtual void ChangeCanUse()
    {

        //���� ���� Ÿ���� �������� �⺻ Ÿ�԰� ���� Ÿ���̶� ���ƾ�  ��� �Ұ�.

        //������ ���� ���� Ÿ���� ���� Ÿ���̶� ���� : �׳� Ȱ��ȭ�� ����.
        //������ ���� ���� Ÿ���� �⺻ Ÿ���̶� ���� : �� �� ���� ����.

        //������ ���⼭ ������ �Ǵ°�, �ٸ� ���ۿ� ���ؼ� ���� Ÿ���� �ٲ���� ��.
        //�׷��ٸ�, ���ο��� Ÿ�԰� ���� Ÿ���� �޶���. ��, false�� ��ȯ��.
        if (mainWeatherType==defaultWeatherType
            &&mainWeatherType==WeatherManager.Instance.GetMainWeather())

        {
            canUse = false;
        }
        else
        {
            canUse = true;
        }
    }

    public void ChangeWeather()
    {
        if (canUse)
        {
            ChangeMainWeather();
        }
        else
        {

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
        if (canUse == false)
        {
            sr.color = Color.gray;
        }
        else
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
}
