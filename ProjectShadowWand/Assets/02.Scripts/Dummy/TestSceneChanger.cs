using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{

    public void ButtonGoPlayerScene()
    {
        SceneManager.LoadScene("SecondPlayerControllerTestScene");
    }
}
