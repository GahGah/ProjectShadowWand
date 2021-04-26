using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{
    public string moveSceneName;
    public void ButtonGoPlayerScene()
    {
        SceneManager.LoadScene(moveSceneName);
    }

    public void Test()
    {
        Screen.fullScreen = false;
    }
}
