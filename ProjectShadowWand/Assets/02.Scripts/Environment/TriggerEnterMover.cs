using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnterMover : MonoBehaviour
{
    [Header("움직일 오브젝트")]
    public Transform moveTransform;

    [Header("스폰 지점")]
    public Transform spawnPoint;

    [Header("끝 지점")]
    public Transform endPoint;

    [Header("해당 초 동안 움직임")]
    [Range(0f, 10f)]
    public float moveSecond = 0f;

    private Vector2 originalPos;
    private Vector2 endPos;

    [Tooltip("진행중인가?")]
    bool isActive;

    private void Awake()
    {
        isActive = false;
        originalPos = spawnPoint.position;
        endPos = endPoint.position;

        moveTransform.gameObject.SetActive(false);
    }
    private IEnumerator ProcessMove()
    {
        isActive = true;
        moveTransform.gameObject.SetActive(true);

        moveTransform.position = originalPos;

        float progress = 0f;
        float timer = 0f;

        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / moveSecond;
            moveTransform.position = Vector2.Lerp(originalPos, endPos, progress);
            yield return null;
        }
        moveTransform.gameObject.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isActive == false)
            {
                StartCoroutine(ProcessMove());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(spawnPoint.position, endPoint.position);
        Gizmos.DrawSphere(spawnPoint.position, 0.2f);
        Gizmos.DrawSphere(endPoint.position, 0.2f);
    }

}


