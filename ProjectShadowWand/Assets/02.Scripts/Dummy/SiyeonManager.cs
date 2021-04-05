using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SiyeonManager : MonoBehaviour
{
    public GameObject restartCanvas;
    public GameObject beginCanvas;
    public GameObject goalCanvas;

    public Button restartButton;


    static SiyeonManager instance;
    public static SiyeonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SiyeonManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        goalCanvas.SetActive(false);
        restartCanvas.SetActive(false);
        Time.timeScale = 0f;
        beginCanvas.SetActive(true);
    }

    public void SetActiveTrueRestartUI()
    {
        restartCanvas.SetActive(true);
    }

    public void SetActiveTrueGoalUI()
    {
        ProcessGoal();
        goalCanvas.SetActive(true);

    }
    public void ProcessGoal()
    {
        Time.timeScale = 0f;
    }
    public void ButtonGoRestart()
    {
        SceneManager.LoadScene("ProtoSiyeonScene");
        Time.timeScale = 1f;
        restartCanvas.SetActive(false);
    }
    public void ButtonGoCloseBeginCanvas()
    {
        beginCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ButtonGoExit()
    {
        Application.Quit();
    }
}
