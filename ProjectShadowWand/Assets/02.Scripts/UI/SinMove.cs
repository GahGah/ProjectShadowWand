using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMove : MonoBehaviour
{
    private float onTimer = 0f;
    public Transform moveTransform;

    public float speed;
    public float length;
    // Start is called before the first frame update
    private Vector2 originalPos;
    private Vector2 tempPos;
    void Start()
    {

    }

    [Header("Å×½ºÆ®")]
    public float tempVal = 0f;
    // Update is called once per frame
    void Update()
    {

        onTimer += Time.unscaledDeltaTime;
        tempVal = Mathf.Sin(onTimer * speed) * length;
        tempPos = new Vector2(0f, tempVal);
        moveTransform.position = moveTransform.position + (Vector3)tempPos * 0.3f;

        if (Mathf.Abs(tempVal - 1f) < 0.001f)
        {
            onTimer = 0f;
        }

        /*
         *         onTimer += Time.unscaledDeltaTime;
        tempSpacing = Mathf.Sin(onTimer * 4f) * 30f;
        layoutGroup.spacing = originalSpacing + tempSpacing;
         */

    }
}
