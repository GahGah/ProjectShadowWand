using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ELayerCategory
{
    Default_0 = 1 << 0,
    TransparentFX_1 = 1 << 1,
    IgnoreRaycast_2 = 1 << 2,
    _3 = 1 << 3,
    Water_4 = 1 << 4,
    UI_5 = 1 << 5,
    _6 = 1 << 6,
    _7 = 1 << 7,
    Monster_8 = 1 << 8,
    Ground_9 = 1 << 9,
    _10 = 1 << 10,
};


public class ShadowCaster2D_Manager : MonoBehaviour
{
    public static ShadowCaster2D_Manager Instance;

    public ELayerCategory includeLayer = ELayerCategory.Default_0;
    [SerializeField] private GameObject[] includeObjects = null;
    [SerializeField] private PolygonCollider2D[] includeObjectsPolygonCol2D = null;
    [SerializeField] private CompositeCollider2D[] includeObjectCompositeCol2D = null;

    private List<ShadowCaster2D_Custom> shadowLightsSC2D = new List<ShadowCaster2D_Custom>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject); // 씬 로딩을 할 때(옮겨다닐 때) 지우지마라 
        }
    }

    private void Start()
    {
        SetIncludeObjects();
    }

    private void Update()
    {
        ClearAllIntersectSegmentPoint();
        SetAllIntersectSegmentPoint();
    }

    /*-----------------------------------------------------*/

    // 선택한 레이어에 포함된 게임 오브젝트와, 갖고 있는 PolygonCollider2D를 배열로 저장. [ O(1)의 접근을 위함. ]
    public void SetIncludeObjects()
    {
        if (includeObjects != null)
            Array.Clear(includeObjects, 0, includeObjects.Length);
        if (includeObjectsPolygonCol2D != null)
            Array.Clear(includeObjectsPolygonCol2D, 0, includeObjectsPolygonCol2D.Length);
        if (includeObjectCompositeCol2D != null)
            Array.Clear(includeObjectCompositeCol2D, 0, includeObjectCompositeCol2D.Length);

        GameObject[] temp_allGameObejcts = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];    // 씬 내의 모든 게임 오브젝트를 가져옴.
        List<GameObject> temp_includeObjectList = new List<GameObject>();
        List<PolygonCollider2D> temp_includeObjectPolygonCol2DList = new List<PolygonCollider2D>();
        List<CompositeCollider2D> temp_includeObjectCompositeCol2DList = new List<CompositeCollider2D>();

        Debug.Log(temp_allGameObejcts);

        foreach(GameObject Obj in temp_allGameObejcts)
        {
            if (includeLayer.HasFlag((ELayerCategory)(1<<Obj.layer)))
            {
                temp_includeObjectList.Add(Obj);

                PolygonCollider2D temp_polygonCol2DComponent = Obj.GetComponent<PolygonCollider2D>();
                if (temp_polygonCol2DComponent != null)
                    temp_includeObjectPolygonCol2DList.Add(temp_polygonCol2DComponent);

                CompositeCollider2D temp_compositeCol2DComponent = Obj.GetComponent<CompositeCollider2D>();
                if (temp_compositeCol2DComponent != null)
                    temp_includeObjectCompositeCol2DList.Add(temp_compositeCol2DComponent);

            }
        }

        includeObjects = new GameObject[temp_includeObjectList.Count];
        includeObjects = temp_includeObjectList.ToArray();

        includeObjectsPolygonCol2D = new PolygonCollider2D[temp_includeObjectPolygonCol2DList.Count];
        includeObjectsPolygonCol2D = temp_includeObjectPolygonCol2DList.ToArray();

        includeObjectCompositeCol2D = new CompositeCollider2D[temp_includeObjectCompositeCol2DList.Count];
        includeObjectCompositeCol2D = temp_includeObjectCompositeCol2DList.ToArray();
    }

    // 그림자를 그리는 빛 리스트에 obj를 추가.
    public void AddShadowLightObj(GameObject obj)
    {
        shadowLightsSC2D.Add(obj.GetComponent<ShadowCaster2D_Custom>());
    }


    public void ClearAllIntersectSegmentPoint()
    {
        for (int i =0; i<shadowLightsSC2D.Count; ++i)
        {
            shadowLightsSC2D[i].ClearIntersectPoint();
        }
    }

    public void SetAllIntersectSegmentPoint()
    {
        // ToDo:
        // 1. 화면(+ a 범위) 내에 있는 오브젝트에 대해서만 for문.
        // 2. 화면(+ a 범위) 내에 있는 vertex에 대해서만 함수 넘겨줌.
        // 을 해주어야 함.

        // Polygon Col 2D를 갖고 있는 오브젝트에 대해.
        for(int i = 0; i<includeObjectsPolygonCol2D.Length; ++i)
        {
            Vector2[] polyCol2DVertex = includeObjectsPolygonCol2D[i].points;

            for (int j = 0; j < polyCol2DVertex.Length; ++j)
            {
                for (int k = 0; k < shadowLightsSC2D.Count; ++k)
                {
                    shadowLightsSC2D[k].AddIntersectSegmentPoint(
                        includeObjectsPolygonCol2D[i].transform.TransformPoint(polyCol2DVertex[j])
                        );
                }
            }
        }

        // Composite Col 2D를 갖고 있는 오브젝트에 대해.
        for (int i = 0; i < includeObjectCompositeCol2D.Length; ++i)
        {
            Vector2[] compositeCol2DVertex = new Vector2[0];

            // Composite Col 2D의 정점을 배열에 넣음.
            for (int j = 0; j < includeObjectCompositeCol2D[i].pathCount; ++j)
            {
                compositeCol2DVertex = new Vector2[includeObjectCompositeCol2D[i].GetPathPointCount(j)];
                includeObjectCompositeCol2D[i].GetPath(j, compositeCol2DVertex);
            }

            for (int j = 0; j < compositeCol2DVertex.Length; ++j)
            {
                for (int k = 0; k < shadowLightsSC2D.Count; ++k)
                {
                    shadowLightsSC2D[k].AddIntersectSegmentPoint(
                        includeObjectCompositeCol2D[i].transform.TransformPoint(compositeCol2DVertex[j])
                        );
                }
            }
        }
    }

    public void CreateMesh()
    {
        // 잠시 고민. 매쉬를 생성하는 것 까지는 좋은데, 블렌딩이 가능할까?
        // uv나 그런것도 직접 펴야하는데.

        for (int i =0; i<shadowLightsSC2D.Count; ++i)
        {
            shadowLightsSC2D[i].SortIntersectPoint();
        }
    }
}
