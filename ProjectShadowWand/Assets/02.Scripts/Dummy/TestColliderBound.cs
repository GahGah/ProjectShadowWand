using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColliderBound : MonoBehaviour
{
    public GameObject[] pointObjects;

    Collider2D m_Collider;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max;
    Vector2[] verts;
    void Start()
    {
        verts = new Vector2[4];
        //Fetch the Collider from the GameObject
        m_Collider = GetComponent<Collider2D>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        //Output this data into the console
        OutputData();
    }
    private void Update()
    {
        OutputData();
    }
    void OutputData()
    {
        verts = Test2();
        pointObjects[0].transform.position = verts[0];
        pointObjects[1].transform.position = verts[1];
        pointObjects[2].transform.position = verts[2];
        pointObjects[3].transform.position = verts[3];
    }

    Vector2[] Test2()
    {
        Vector2 relOffset = transform.position;
        var offset = m_Collider.offset;

        var path = new Vector2[] {
            offset + new Vector2(-m_Collider.bounds.extents.x, -m_Collider.bounds.extents.y),
            offset + new Vector2(m_Collider.bounds.extents.x, -m_Collider.bounds.extents.y),
            offset + new Vector2(m_Collider.bounds.extents.x, m_Collider.bounds.extents.y) ,
            offset + new Vector2(-m_Collider.bounds.extents.x, m_Collider.bounds.extents.y)
};
        return path;
    }

    void OnDrawGizmosSelected()
    {
        //BoxCollider2D b = GetComponent<BoxCollider2D>();

        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(transform.TransformPoint(b.offset + new Vector2(b.size.x, -b.size.y) * 0.5f), 1f);
        //Gizmos.DrawSphere(transform.TransformPoint(b.offset + new Vector2(-b.size.x, b.size.y) * 0.5f), 1f);
        //Gizmos.DrawSphere(transform.TransformPoint(b.offset + new Vector2(-b.size.x, -b.size.y) * 0.5f), 1f);
        //Gizmos.DrawSphere(transform.TransformPoint(b.offset + new Vector2(b.size.x, b.size.y) * 0.5f), 1f);
    }
}
