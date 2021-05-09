using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class StageDoor : MonoBehaviour
{

    private Light2D[] lights;
    public bool isOpen;
    public bool isLoop;
    private void Start()
    {
        lights = GetComponentsInChildren<Light2D>();
        isOpen = false;
        Debug.Log("AllLight is Done");

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = Color.red;
        }
    }
    public IEnumerator ChangeOpenDoor()
    {
        isOpen = false;
        float timer = 0f;
        float progress = 0f;
        float _goingTime = 2f;

        while (progress < 1f)
        {
            timer += Time.unscaledDeltaTime;
            progress = timer / _goingTime;

            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].color = Color32.Lerp(Color.red, Color.white, progress);
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        isOpen = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //ÀR·¹ÀÌ¾î°¡ ´ê¾Ò´Ù¸é
        {
            if (isOpen)
            {
                StageManager.Instance.UpdateStageName();
                if (isLoop == false)
                {

                    StartCoroutine(SceneChanger.Instance.LoadThisScene(StageManager.Instance.nextStageName));
                }
                else
                {
                    StartCoroutine(SceneChanger.Instance.LoadThisScene(StageManager.Instance.nowStageName));
                }

            }
        }
    }
}
