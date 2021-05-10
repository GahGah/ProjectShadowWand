using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSortingLayer : MonoBehaviour
{
    public string sortingLayerName;
    public int sortingOrder;

    private MeshRenderer meshRenderer;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = sortingOrder;
        meshRenderer.sortingLayerName = sortingLayerName;
    }

    //public void Update()
    //{
    //    if (meshRenderer.sortingLayerName != sortingLayerName)
    //        meshRenderer.sortingLayerName = sortingLayerName;
    //    if (meshRenderer.sortingOrder != sortingOrder)
    //        meshRenderer.sortingOrder = sortingOrder;
    //}

    public void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = sortingOrder;
    }
}
