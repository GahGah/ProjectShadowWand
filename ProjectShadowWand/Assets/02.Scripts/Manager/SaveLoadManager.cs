using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터를 세이브하고 로드할 때 쓰이는 매니저...
/// Json파일을 저장 & 출력하는데에 사용합니다...
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("이미 instance가 존재합니다." + this);
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Json파일로 저장하고, 해당 키값을 반환합니다.
    /// </summary>
    /// <param name="obj">저장할 object</param>
    public string SaveToJson(object obj)
    {
        string data = JsonUtility.ToJson(obj);
        return data;
    }

    public T LoadToJson<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }


    public IEnumerator SaveThis()
    {
        yield return null;
    }
}
