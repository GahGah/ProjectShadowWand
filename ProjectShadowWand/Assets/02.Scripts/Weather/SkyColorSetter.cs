using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SkyColorSetter : MonoBehaviour
{
    private Material mat;

    private void Start()
    {
        mat = this.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void setColorProperty(Color col)
    {
        mat.SetColor("_Color", col);
    }
}
