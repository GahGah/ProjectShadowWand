using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum ELight2DCategory
{
    Directional, //직선형
    Radial //방사형
};

public delegate void Delegate_AddIntersectSegmentPoint(Vector3 point);

public class ShadowCaster2D_Custom : MonoBehaviour
{
    public class SortComparer_Angle : IComparer<Vector2>
    {
        public int Compare(Vector2 first, Vector2 second)
        {
            float firstAngle = (float)((Mathf.Atan2(first.x, first.y) / Math.PI) * 180f);
            float secondAngle = (float)((Mathf.Atan2(second.x, second.y) / Math.PI) * 180f);

            firstAngle = firstAngle < 0 ? firstAngle + 360f : firstAngle;
            secondAngle = secondAngle < 0 ? secondAngle + 360f : secondAngle;

            if (firstAngle > secondAngle)
                return -1;
            else if (firstAngle == secondAngle)
                return 0;
            else
                return -1;
        }
    }

    private ShadowCaster2D_Manager SC2D_Manager;

    [SerializeField] private ELight2DCategory lightCategory = ELight2DCategory.Radial;
    [SerializeField] private int rayCount = 50;

    public Delegate_AddIntersectSegmentPoint AddIntersectSegmentPoint;              // 함수 대리자
    public List<Vector2> intersectPoint = new List<Vector2>();

    private Light2D light2d = null;
    private float unitRot;
    private float distance = 0.0f;

    private void Awake()
    {
        light2d = this.GetComponent<Light2D>();
        
        if (light2d == null)
            Debug.Log("!!! light2D is null !!!");

    }

    void Start()
    {
        SC2D_Manager = ShadowCaster2D_Manager.Instance;
        if (SC2D_Manager == null)
            Debug.Log("!!! SC2D_Manager is null !!!");

        SC2D_Manager.AddShadowLightObj(this.gameObject);

        switch (lightCategory)
        {
            case ELight2DCategory.Directional:
                Init_Directinal();
                break;

            case ELight2DCategory.Radial:
                Init_Redial();
                break;
        }
    }

    /*-------------------------------------------------*/

    void Init_Directinal()
    {
        //pass
    }

    void Init_Redial()
    {
        AddIntersectSegmentPoint = new Delegate_AddIntersectSegmentPoint(AddIntersectSegmentPoint_Radial);

        distance = light2d.pointLightOuterRadius;
        unitRot = 360 / (float)rayCount;

        Debug.Log("unitRot: " + unitRot);
        Debug.Log("distance: " + distance);
    }

    public void SortIntersectPoint()
    {
        var convPosLocal = new Converter<Vector2, Vector2>(x => x - (Vector2)this.transform.position);
        var convPosWorld = new Converter<Vector2, Vector2>(x => x + (Vector2)this.transform.position);

        intersectPoint.ConvertAll<Vector2>(convPosLocal);
        intersectPoint.Sort(new SortComparer_Angle());
        intersectPoint.ConvertAll<Vector2>(convPosWorld);
    }

    public void ClearIntersectPoint()
    {
        intersectPoint.Clear();
    }

    //
    public void AddIntersectSegmentPoint_Radial(Vector3 point)
    {
        Vector2 pointV2 = point;
        Vector2 direction = (pointV2 - new Vector2(this.transform.position.x, this.transform.position.y)).normalized;   // this에서 point로 향하는 벡터 + 정규화.
        distance = light2d.pointLightOuterRadius;

        // 1차 hit.
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, distance, (int)SC2D_Manager.includeLayer);
        if (hit)
        {
            Debug.DrawLine(this.transform.position, hit.point, Color.blue);
            intersectPoint.Add(hit.point);

            // 2차 hit (확장)
            // hit한 곳이 모서리일 경우 확장시켜줌.
            if(hit.point == pointV2)
            {
                RaycastHit2D extendedRayhit = Physics2D.Raycast(hit.point + direction*0.1f, direction, Vector2.Distance(hit.point, (Vector2)this.transform.position + (direction * distance)), (int)SC2D_Manager.includeLayer);
                if (extendedRayhit)
                {
                    Debug.DrawLine(hit.point + direction * 0.1f, extendedRayhit.point, Color.cyan);
                    intersectPoint.Add(extendedRayhit.point);
                }
                else
                {
                    Debug.DrawLine(hit.point, (Vector2)this.transform.position + (direction * distance), Color.magenta);
                }
            }
        }
        else
        {
            // 히트하지 않았는데 둘 사이의 거리가 범위 안에 있다 -> raycast의 오차이므로 보정해줌.
            if (Vector2.Distance((Vector2)this.transform.position, pointV2) <= distance)
            {
                Debug.DrawLine(this.transform.position, pointV2, Color.blue);
                intersectPoint.Add(pointV2);

                RaycastHit2D extendedRayhit = Physics2D.Raycast(pointV2 + direction * 0.1f, direction, Vector2.Distance(pointV2, (Vector2)this.transform.position + (direction * distance)), (int)SC2D_Manager.includeLayer);
                if (extendedRayhit)
                {
                    Debug.DrawLine(pointV2 + direction * 0.1f, extendedRayhit.point, Color.cyan);
                    intersectPoint.Add(extendedRayhit.point);
                }
                else
                {
                    Debug.DrawLine(pointV2, (Vector2)this.transform.position + (direction * distance), Color.magenta);
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, direction * distance, Color.red);
            }
        }
    }

    /*
    void FindIntersectSegment_Directional()
    {

    }

    void FindIntersectSegment_Radial()
    {
        Vector2 rayDirection = Vector2.zero;

        for (int i = 0; i<rayCount; ++i)
        {
            //rayDirection = unitQRot * rayDirection;
            //Debug.Log("rayDirection: " + rayDirection);

            rayDirection.x = Mathf.Cos(unitRot*i);
            rayDirection.y = Mathf.Sin(unitRot*i);

            distance = light2d.pointLightOuterRadius;

            Debug.Log("angle: " + unitRot * i);
            Debug.DrawRay(this.transform.position, rayDirection*distance, Color.red);

            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(this.transform.position, rayDirection, distance, (int)SC2D_Manager.includeLayer))
            {
                Debug.DrawLine(hit.point, hit.point + new Vector2(rayDirection.x, rayDirection.y), Color.blue);
            }
        }
    }
    */
}