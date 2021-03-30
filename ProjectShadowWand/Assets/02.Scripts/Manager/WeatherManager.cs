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



    // 싱글톤 패턴
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            WeatherManager.DontDestroyOnLoad(this.gameObject); // 씬 로딩을 할 때(옮겨다닐 때) 지우지마라 
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
