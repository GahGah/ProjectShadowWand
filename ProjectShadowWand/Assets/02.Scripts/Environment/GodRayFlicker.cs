using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class GodRayFlicker : MonoBehaviour
{
    private Light2D[] L2D;
    private List<float> defaultIntensity = new List<float>();

    [SerializeField] private Vector2 multiplyRange = new Vector2(1, 1);
    [SerializeField] private float speed = 1f;

    private float timer = 0;
    
    private void Start()
    {
        L2D = this.GetComponentsInChildren<Light2D>();

        for(int i=0; i < L2D.Length; ++i)
        {
            defaultIntensity.Add(L2D[i].intensity);
        }

    }

    void Update()
    {
        timer += Time.deltaTime * speed;

        for(int i =0; i< L2D.Length; ++i)
        {
            L2D[i].intensity = defaultIntensity[i] * Mathf.Lerp(multiplyRange.x, multiplyRange.y, (Mathf.Sin(timer)+1) * 0.5f);
        }

        if(timer > 10000) { timer = 0; }
    }
}
