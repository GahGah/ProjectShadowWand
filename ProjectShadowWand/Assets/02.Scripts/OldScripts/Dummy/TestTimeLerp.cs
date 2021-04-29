using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeLerp : MonoBehaviour
{
    float moveTime;
    Vector3 startPosition;
    public Transform target;
    void Start()
    {
        moveTime = 5f;
        startPosition = transform.position;
        StartCoroutine(MoveToPosition(transform, target.position, moveTime));
    }

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        Debug.LogError("Go");
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            Debug.LogError("Move");
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}
