using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent (typeof(Light2D))]
[ExecuteInEditMode]
public class LightSetter : MonoBehaviour
{
    private Light2D L2D;

    private void Start()
    {
        L2D = this.GetComponent<Light2D>();
    }

    public void setLightProperty(LightController.LightColorSetting lc)
    {
        L2D.color = lc.color;
        L2D.intensity = lc.lightIntensity;
        L2D.shadowIntensity = lc.shadowIntensity;
    }

}