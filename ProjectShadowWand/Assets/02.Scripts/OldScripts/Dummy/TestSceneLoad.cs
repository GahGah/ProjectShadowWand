using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TestSceneLoad : MonoBehaviour
{

    // called zero
    void Awake()
    {
        Debug.Log("Awake");
    }

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }

    // called third
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.name + "'.");
        Debug.Log("DataPath : " + Application.dataPath);
        string path = Application.dataPath;
        string[] splitPath = path.Split(new string[] { "Assets" }, System.StringSplitOptions.RemoveEmptyEntries);
        path = splitPath[0];
        Debug.Log("Split Data Path : " + path);
    }
    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
