using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public enum WEATHER
    {
        None = 0,
    }

    public static WeatherManager Instance;

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
}
