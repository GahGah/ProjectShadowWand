using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerToBeContinue : MonoBehaviour
{

    private IEnumerator ActiveToBeContinueCoroutine;

    public CanvasGroup canvasGroup;
    public GameObject blackPanel;
    public GameObject tbcText;
    public GameObject pressAnyKeyText;

    public bool isPlayerIn;

    private void Start()
    {
        ActiveToBeContinueCoroutine = ActiveToBeContinue();
        isPlayerIn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ActiveToBeContinueCoroutine);
            isPlayerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerIn = false;
        }

    }

    private IEnumerator ActiveToBeContinue()
    {
        ActiveToBeContinueCoroutine = null;

        float timer = 0f;
        float time = 1.5f;

        PlayerController.Instance.canMove = false; //이동 불가능

        while (timer < time)
        {
            timer += Time.unscaledDeltaTime;

            if (isPlayerIn == false)
            {
                break;
            }
        }

        if (isPlayerIn == false)
        {
            PlayerController.Instance.isDie = true;
            yield break;
        }

        Time.timeScale = 0f;

        UIManager.Instance.canPause = false; //일시정지 불가능

        canvasGroup.alpha = 0f;
        blackPanel.SetActive(true); //검은 판넬 띄우기

        yield return StartCoroutine(OnBlackScreen());

        yield return new WaitForSecondsRealtime(1f);
        tbcText.SetActive(true); //투 비 컨티뉴 텍스트 띄우기

        yield return new WaitForSecondsRealtime(1f);
        //플레이어가 제대로 내려갔다면
        pressAnyKeyText.SetActive(true); //아무 키 입력하라는 텍스트 띄우기

        while (true)
        {
            if (InputManager.Instance.buttonAnyKey.wasPressedThisFrame)
            {
                break; //아무 키나 누를 때 까지 대기
            }

            yield return null;
        }

        SceneChanger.Instance.LoadThisSceneName("Stage_Main", false);
    }

    private IEnumerator OnBlackScreen()
    {
        float progress = 0f;
        float timer = 0f;
        float limit = 3f;

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / limit;
            canvasGroup.alpha = progress;
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }
}
