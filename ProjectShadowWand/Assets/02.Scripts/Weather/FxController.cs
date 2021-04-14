using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class FxController : MonoBehaviour
{
    [Serializable]
    public struct FxSetting
    {
        public eMainWeatherType weatherName;
        public GameObject fxParticleObject;
    };
    [SerializeField] public FxSetting[] fxSettings;

    private GameObject nowFxObject = null;
    private GameObject curFxObject = null;

    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    /*-------------*/
    
    private void Init()
    {
        /*
        string nowFxObjName = "nowFxObj";
        string prevFxObjName = "prevFxObj";

        int tempChildCount = this.transform.childCount;

        if (tempChildCount <= 2)
        {
            for (int i = 0; i < 2; ++i)
            {
                if (i < tempChildCount)
                {
                    switch (i)
                    {
                        case 0:
                            nowFxObject = this.transform.GetChild(i).gameObject;
                            nowFxObject.name = nowFxObjName;
                            break;
                        case 1:
                            curFxObject = this.transform.GetChild(i).gameObject;
                            curFxObject.name = prevFxObjName;
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            nowFxObject = new GameObject(nowFxObjName);
                            nowFxObject.transform.SetParent(this.transform);
                            break;
                        case 1:
                            curFxObject = new GameObject(prevFxObjName);
                            curFxObject.transform.SetParent(this.transform);
                            break;
                    }
                }
            }
        }
        else
        {
            for (int i = 2; i < tempChildCount; ++i)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }
        */
    }

    private void fxControl()
    {
#if UNITY_EDITOR
        WeatherManager wmInstance = FindObjectOfType<WeatherManager>();
        GameManager gmInstance = FindObjectOfType<GameManager>();
#else
        WeatherManager wmInstance = WeatherManager.Instance;
        GameManager gmInstance = GameManager.Instance;
#endif

        if (wmInstance.isMainWeatherChanging == true)
        {
            int nowMainWeather = (int)wmInstance.GetMainWeather();
            int prevMainWeather = (int)wmInstance.GetPrevMainWeather();
            float changingRatio = wmInstance.changingMainWeatherRatio;

            
        }

    }

    /* 그... cur 날씨의 Object와 now 날씨의 Object가 생성된 것을 확인하고, 계속 교체해 주어야 함.
     * 
     */
}
