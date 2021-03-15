using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance;

    public Camera mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // GameObject.DontDestroyOnLoad(this.gameObject); //상황에 따라서 돈디스트로이를 없앨수도 있음.
        }
        else
        {
            // Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (mainCamera==null)
        {
            mainCamera = Camera.main;

        }   
    }
    /// <summary>
    ///해당 오브젝트가 화면 범위 내에 있는지 검사합니다.
    /// </summary>
    /// <param name="_object"></param>
    /// <returns>화면 안에 있으면 true, 아니면 false를 리턴합니다.</returns>
    public bool CheckThisObjectInScreen(GameObject _object)
    {

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(_object.transform.position);
        bool inScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        return inScreen;
    }

}