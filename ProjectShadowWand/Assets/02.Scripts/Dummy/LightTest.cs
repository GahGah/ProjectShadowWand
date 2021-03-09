using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빛을 내뿜는다고 가정하는 오브젝트입니다.
/// </summary>
public class LightTest : MonoBehaviour
{

    public bool DiMode;
    private Vector3 nowPosition;
    public GameObject[] objectList;

    public float offset = 0.01f;

    public GameObject lightRays; //.

    private Mesh mesh;

    public struct angledVerts
    {
        public Vector3 vert;
        public float angle;
        public Vector2 uv;
    }
    void Start()
    {
        mesh = lightRays.GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        if (DiMode)
            Test3D();
        else
            Test2D();
    }

    void Test2D()
    {
        nowPosition = this.transform.position;

        for (int i = 0; i < objectList.Length; i++)
        {

            Vector3[] meshs = SpriteToMesh(objectList[i].GetComponent<SpriteRenderer>().sprite).vertices;

            for (int j = 0; j < meshs.Length; j++)
            {
                Vector3 vertLoc = objectList[i].transform.localToWorldMatrix.MultiplyPoint3x4(meshs[j]);

                RaycastHit2D hit1 = Physics2D.Raycast(nowPosition, new Vector2(vertLoc.x - nowPosition.x - offset, vertLoc.y - nowPosition.y - offset), 100f);
                RaycastHit2D hit2 = Physics2D.Raycast(nowPosition, new Vector2(vertLoc.x - nowPosition.x + offset, vertLoc.y - nowPosition.y + offset), 100f);

                Debug.DrawLine(nowPosition, hit1.point, Color.red);
                Debug.DrawLine(nowPosition, hit2.point, Color.green);
            }
        }
    }

    public static int[] AddItemsToArray(int[] original, int itemToAdd1, int itemToAdd2, int itemToAdd3)
    {
        int[] finalArray = new int[original.Length + 3];
        for (int i = 0; i < original.Length; i++)
        {
            finalArray[i] = original[i];
        }

        finalArray[original.Length] = itemToAdd1;
        finalArray[original.Length + 1] = itemToAdd2;
        finalArray[original.Length + 2] = itemToAdd3;

        return finalArray;
    }

    public static Vector3[] ConcatArrays(Vector3[] first, Vector3[] second)
    {
        Vector3[] concatted = new Vector3[first.Length + second.Length];

        Array.Copy(first, concatted, first.Length);
        Array.Copy(second, 0, concatted, first.Length, second.Length);

        return concatted;
    }

    void Test3D()
    {
        mesh.Clear();

        Vector3[] objverts = objectList[0].GetComponent<MeshFilter>().mesh.vertices;

        for (int i = 0; i < objectList.Length; i++)
        {
            objverts = ConcatArrays(objverts, objectList[i].GetComponent<MeshFilter>().mesh.vertices);
        }

        angledVerts[] angledVerts = new angledVerts[(objverts.Length * 2)];
        Vector3[] verts = new Vector3[(objverts.Length * 2) + 1];
        Vector2[] uvs = new Vector2[(objverts.Length * 2) + 1];

        verts[0] = lightRays.transform.worldToLocalMatrix.MultiplyPoint3x4(this.transform.position);
        uvs[0] = new Vector2(verts[0].x, verts[0].y);

        int h = 0;

        nowPosition = this.transform.position;

        for (int i = 0; i < objectList.Length; i++)
        {
            Vector3[] meshs = objectList[i].GetComponent<MeshFilter>().mesh.vertices;
            for (int j = 0; j < meshs.Length; j++)
            {
                Vector3 vertLoc = objectList[i].transform.localToWorldMatrix.MultiplyPoint3x4(meshs[j]);

                RaycastHit hit1;
                RaycastHit hit2;

                float angle1 = Mathf.Atan2((vertLoc.y - nowPosition.y - offset), (vertLoc.x - nowPosition.x - offset));
                float angle2 = Mathf.Atan2((vertLoc.y - nowPosition.y + offset), (vertLoc.x - nowPosition.x + offset));

                Physics.Raycast(nowPosition, new Vector2(vertLoc.x - nowPosition.x - offset, vertLoc.y - nowPosition.y - offset), out hit1, 100f);
                Physics.Raycast(nowPosition, new Vector2(vertLoc.x - nowPosition.x + offset, vertLoc.y - nowPosition.y + offset), out hit2, 100f);
                Debug.DrawLine(nowPosition, hit1.point, Color.blue);
                Debug.DrawLine(nowPosition, hit2.point, Color.yellow);

                angledVerts[(h * 2)].vert = lightRays.transform.worldToLocalMatrix.MultiplyPoint3x4(hit1.point);
                angledVerts[(h * 2)].angle = angle1;
                angledVerts[(h * 2)].uv = new Vector2(angledVerts[(h * 2)].vert.x, angledVerts[(h * 2)].vert.y);

                angledVerts[(h * 2) + 1].vert = lightRays.transform.worldToLocalMatrix.MultiplyPoint3x4(hit2.point);
                angledVerts[(h * 2) + 1].angle = angle2;
                angledVerts[(h * 2) + 1].uv = new Vector2(angledVerts[(h * 2) + 1].vert.x, angledVerts[(h * 2) + 1].vert.y);

                h++;
            }
        }

        Array.Sort(angledVerts, delegate (angledVerts one, angledVerts two)
        {
            return one.angle.CompareTo(two.angle);
        });

        for (int i = 0; i < angledVerts.Length; i++)
        {
            verts[i + 1] = angledVerts[i].vert;
            uvs[i + 1] = angledVerts[i].uv;
        }

        mesh.vertices = verts;

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(uvs[i].x + .5f, uvs[i].y + .5f);
        }
        mesh.uv = uvs;

        int[] triangles = { 0, 1, verts.Length - 1 };
        for (int i = verts.Length - 1; i > 0; i--)
        {
            triangles = AddItemsToArray(triangles, 0, i, i - 1);
        }
        mesh.triangles = triangles;
    }
    Mesh SpriteToMesh(Sprite sprite)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
        mesh.uv = sprite.uv;
        mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

        return mesh;
    }
}
