using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSceneChanger : MonoBehaviour
{
    public string moveSceneName;

    public Image goBlackImage;

    private void Start()
    {
        goBlackImage.gameObject.SetActive(false);
    }
    public void ButtonGoPlayerScene()
    {
        SceneManager.LoadScene(moveSceneName);
    }


    public void PlayerDie_SceneReload()
    {
        StartCoroutine(ProcessDie());
    }

    public void Test()
    {
        Screen.fullScreen = false;
    }

    IEnumerator ProcessDie()
    {
        var timer = 0f;
        var maxTime = 2f;
        var progress = 0f;
        var twoMyeong = new Color32(0, 0, 0, 0);
        var black = new Color32(0, 0, 0, 255);

        yield return new WaitForSeconds(2f);

        goBlackImage.gameObject.SetActive(true);
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / maxTime;
            Debug.Log("Progress : "+ progress);

            goBlackImage.color = Color32.Lerp(twoMyeong, black, progress);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(moveSceneName);

    }
}
