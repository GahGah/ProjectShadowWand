using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
[ExecuteInEditMode]
public class FlickerLight : MonoBehaviour
{
    private Light2D L2D;
    //[SerializeField] private Vector2 intensityRange = new Vector2(1, 1);

    private void Start()
    {
        L2D = this.GetComponent<Light2D>();
    }

    void Update()
    {
        L2D.intensity = Random.Range(0.8f, 1.2f);
    }
}
