using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private new Light light;
    [SerializeField] private Vector2 intensityRange = new Vector2(1, 1);

    private float defaultIntensity;
    private void Awake()
    {
        light = this.GetComponent<Light>();
        defaultIntensity = light.intensity;
    }

    void Update()
    {
        light.intensity = defaultIntensity * Random.Range(intensityRange.x, intensityRange.y);
    }
}
