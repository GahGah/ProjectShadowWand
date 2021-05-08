using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̱��������� ����Ǿ��ִ� �Ŵ��� Ŭ���� 
/// </summary>
/// <typeparam name="T">�̱��� ������ �����Ϸ��� Ŭ����</typeparam>
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
                Debug.LogError(typeof(T).Name + "�� �������� �ʽ��ϴ�. �߰���!");
               // instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }

            return instance;
        }
    }

    /// <summary>
    /// ���� Instance�� null�ϰ��, instance�� gameObject.GetComponent(T)�� �մϴ�.
    /// </summary>
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            instance = gameObject.GetComponent<T>();
        }
    }

}
