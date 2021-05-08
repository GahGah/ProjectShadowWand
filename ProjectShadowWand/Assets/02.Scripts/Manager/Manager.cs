using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글턴패턴이 적용되어있는 매니저 클래스 
/// </summary>
/// <typeparam name="T">싱글턴 패턴을 적용하려는 클래스</typeparam>
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
            }

            if (instance == null)
            {
                Debug.LogError(typeof(T).Name + "이 존재하지 않습니다. 추가해!");
               // instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }

            return instance;
        }
    }

    /// <summary>
    /// 만약 Instance가 null일경우, instance에 gameObject.GetComponent(T)를 합니다.
    /// </summary>
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            instance = gameObject.GetComponent<T>();
        }
    }

}
