using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunnyTotem : Totem
{
    [Header("�� ����Ʈ ")]
    public GameObject rainEffect;

    [Header("������ Ȱ��ȭ �ð�")]
    public float activeTime;

    [Tooltip("Ȱ��ȭ �Ǿ��ִ� �ð�...")]
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

        while (currentTime < activeTime) //Ÿ�̸Ӱ� ��Ƽ�� Ÿ�Ӻ��� ���� �� ����
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        
        var test = WeatherManager.Instance.SetMainWeather(defaultWeatherType);
        Debug.Log("���� ���� ü���� �õ�: " + test);
        isTimer = false;
    }
}