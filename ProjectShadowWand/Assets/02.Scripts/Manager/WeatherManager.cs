using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    struct WeatherColor
    {

    };

    public static WeatherManager Instance;
    
    private eWeatherType prevWeather = eWeatherType.SUNNY;
    [SerializeField] private eWeatherType nowWeather = eWeatherType.SUNNY;



    // �̱��� ����
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WeatherManager.DontDestroyOnLoad(this.gameObject); // �� �ε��� �� ��(�Űܴٴ� ��) ���������� 
        }
    }

    public int Init()
    {
        return 0;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void WeatherChaneged()
    {
        //curWeather
    }

    public void SetWeather()
    {
        
    }

    public void GetWeather()
    {

    }

}
