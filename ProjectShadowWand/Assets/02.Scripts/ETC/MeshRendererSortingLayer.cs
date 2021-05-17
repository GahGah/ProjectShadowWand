using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererSortingLayer : MonoBehaviour
{
    public eSortingLayer sortingLayer;
    public int sortingOrder;

    public MeshRenderer meshRenderer;
    void Awake()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        meshRenderer.sortingLayerName = sortingLayer.ToString();
        meshRenderer.sortingOrder = sortingOrder;
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
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        meshRenderer.sortingLayerName = sortingLayer.ToString();
        meshRenderer.sortingOrder = sortingOrder;
    }
}
