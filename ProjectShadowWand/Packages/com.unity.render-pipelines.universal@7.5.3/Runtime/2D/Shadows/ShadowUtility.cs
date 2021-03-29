using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal.LibTessDotNet;


namespace UnityEngine.Experimental.Rendering.Universal
{
    internal class ShadowUtility
    {
        internal struct Edge : IComparable<Edge>
        {
            public int vertexIndex0;
            public int vertexIndex1;
            public Vector4 tangent;
            private bool compareReversed; // This is done so that edge AB can equal edge BA

            public Vector3 vertex0;
            public Vector3 vertex1;

            public void AssignVertexIndices(int vi0, int vi1)
            {
                vertexIndex0 = vi0;
                vertexIndex1 = vi1;
                compareReversed = vi0 > vi1;
            }

            public int Compare(Edge a, Edge b)
            {
                int adjustedVertexIndex0A = a.compareReversed ? a.vertexIndex1 : a.vertexIndex0;
                int adjustedVertexIndex1A = a.compareReversed ? a.vertexIndex0 : a.vertexIndex1;
                int adjustedVertexIndex0B = b.compareReversed ? b.vertexIndex1 : b.vertexIndex0;
                int adjustedVertexIndex1B = b.compareReversed ? b.vertexIndex0 : b.vertexIndex1;

                // Sort first by VI0 then by VI1
                int deltaVI0 = adjustedVertexIndex0A - adjustedVertexIndex0B;
                int deltaVI1 = adjustedVertexIndex1A - adjustedVertexIndex1B;

                if (deltaVI0 == 0)
                    return deltaVI1;
                else
                    return deltaVI0;
            }

            public int CompareTo(Edge edgeToCompare)
            {
                return Compare(this, edgeToCompare);
            }
        }

        static Edge CreateEdge(int triangleIndexA, int triangleIndexB, List<Vector3> vertices, List<int> triangles, List<Vector2> uv2s, List<Vector2> uv3s)
        {
            Edge retEdge = new Edge();

            retEdge.AssignVertexIndices(triangles[triangleIndexA], triangles[triangleIndexB]);
            
            retEdge.vertex0 = vertices[retEdge.vertexIndex0];
            retEdge.vertex0.z = 0;
            retEdge.vertex1 = vertices[retEdge.vertexIndex1];
            retEdge.vertex1.z = 0;

            Vector3 edgeDir = Vector3.Normalize(retEdge.vertex1 - retEdge.vertex0);
            retEdge.tangent = Vector3.Cross(-Vector3.forward, edgeDir);

            //retEdge.tangent = new Vector4(vertex0.x, vertex0.y, vertex1.x, vertex1.y);
            return retEdge;
        }

        static void PopulateEdgeArray(List<Vector3> vertices, List<int> triangles, List<Edge> edges, List<Vector2> uv2s, List<Vector2> uv3s)
        {
            for(int triangleIndex=0;triangleIndex<triangles.Count;triangleIndex+=3)
            {
                edges.Add(CreateEdge(triangleIndex, triangleIndex + 1, vertices, triangles, uv2s, uv3s));
                edges.Add(CreateEdge(triangleIndex+1, triangleIndex + 2, vertices, triangles, uv2s, uv3s));
                edges.Add(CreateEdge(triangleIndex+2, triangleIndex, vertices, triangles, uv2s, uv3s));
            }
        }

        static bool IsOutsideEdge(int edgeIndex, List<Edge> edgesToProcess)
        {
            int previousIndex = edgeIndex - 1;
            int nextIndex = edgeIndex + 1;
            int numberOfEdges = edgesToProcess.Count;
            Edge currentEdge = edgesToProcess[edgeIndex];

            return (previousIndex < 0 || (currentEdge.CompareTo(edgesToProcess[edgeIndex - 1]) != 0)) && (nextIndex >= numberOfEdges || (currentEdge.CompareTo(edgesToProcess[edgeIndex + 1]) != 0));
        }

        static void SortEdges(List<Edge> edgesToProcess)
        {
            edgesToProcess.Sort();
        }

        static void CreateShadowTriangles(List<Vector3> vertices, List<Color> colors, List<int> triangles, List<Vector4> tangents, List<Edge> edges, List<Vector2> uv2s, List<Vector2> uv3s)
        {
            for (int edgeIndex = 0; edgeIndex < edges.Count; edgeIndex++)
            {
                if (IsOutsideEdge(edgeIndex, edges))
                {
                    Edge edge = edges[edgeIndex];
                    tangents[edge.vertexIndex1] = -edge.tangent;

                    int newVertexIndex = vertices.Count;
                    vertices.Add(vertices[edge.vertexIndex0]);
                    colors.Add(colors[edge.vertexIndex0]);

                    tangents.Add(-edge.tangent);

                    triangles.Add(edge.vertexIndex0);
                    triangles.Add(newVertexIndex);
                    triangles.Add(edge.vertexIndex1);

                    uv2s[edge.vertexIndex0] = new Vector2(edge.vertex0.x, edge.vertex0.y);
                    uv3s[edge.vertexIndex1] = new Vector2(edge.vertex1.x, edge.vertex1.y);

                    uv2s.Add(new Vector2(edge.vertex0.x, edge.vertex0.y));
                    uv3s.Add(new Vector2(edge.vertex1.x, edge.vertex1.y));
                }
            }
        }

        static object InterpCustomVertexData(Vec3 position, object[] data, float[] weights)
        {
            return data[0];
        }

        static void InitializeTangents(int tangentsToAdd, List<Vector4> tangents)
        {
            for (int i = 0; i < tangentsToAdd; i++)
                tangents.Add(Vector4.zero);
        }

        /*----------------- ORIGINAL CODE -------------------*/
        //public static void GenerateShadowMesh(Mesh mesh, Vector3[] shapePath)
        //{
        //    List<Vector3> vertices = new List<Vector3>();
        //    List<int> triangles = new List<int>();
        //    List<Vector4> tangents = new List<Vector4>();
        //    List<Color> extrusion = new List<Color>();

        //    // Create interior geometry
        //    int pointCount = shapePath.Length;
        //    var inputs = new ContourVertex[2 * pointCount];
        //    for (int i = 0; i < pointCount; i++)
        //    {
        //        Color extrusionData = new Color(shapePath[i].x, shapePath[i].y, shapePath[i].x, shapePath[i].y);
        //        int nextPoint = (i + 1) % pointCount;
        //        inputs[2*i] = new ContourVertex() { Position = new Vec3() { X = shapePath[i].x, Y = shapePath[i].y, Z=0 }, Data = extrusionData };

        //        extrusionData = new Color(shapePath[i].x, shapePath[i].y, shapePath[nextPoint].x, shapePath[nextPoint].y);
        //        Vector2 midPoint = 0.5f * (shapePath[i] + shapePath[nextPoint]);
        //        inputs[2*i+1] = new ContourVertex() { Position = new Vec3() { X = midPoint.x, Y = midPoint.y, Z = 0}, Data = extrusionData };
        //    }

        //    Tess tessI = new Tess();
        //    tessI.AddContour(inputs, ContourOrientation.Original);
        //    tessI.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3, InterpCustomVertexData);

        //    var indicesI = tessI.Elements.Select(i => i).ToArray();
        //    var verticesI = tessI.Vertices.Select(v => new Vector3(v.Position.X, v.Position.Y, 0)).ToArray();
        //    var extrusionI = tessI.Vertices.Select(v => new Color(((Color)v.Data).r, ((Color)v.Data).g, ((Color)v.Data).b, ((Color)v.Data).a)).ToArray();

        //    vertices.AddRange(verticesI);
        //    triangles.AddRange(indicesI);
        //    extrusion.AddRange(extrusionI);

        //    InitializeTangents(vertices.Count, tangents);

        //    List<Edge> edges = new List<Edge>();
        //    PopulateEdgeArray(vertices, triangles, edges);
        //    SortEdges(edges);
        //    CreateShadowTriangles(vertices, extrusion, triangles, tangents, edges);

        //    Color[] finalExtrusion = extrusion.ToArray();
        //    Vector3[] finalVertices = vertices.ToArray();
        //    int[] finalTriangles = triangles.ToArray();
        //    Vector4[] finalTangents = tangents.ToArray();

        //    mesh.Clear();
        //    mesh.vertices = finalVertices;
        //    mesh.triangles = finalTriangles;
        //    mesh.tangents = finalTangents;
        //    mesh.colors = finalExtrusion;
        //}

        public static void GenerateShadowMesh(Mesh mesh, Vector3[] shapePath)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector4> tangents = new List<Vector4>();
            List<Color> extrusion = new List<Color>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector2> uv2s = new List<Vector2>(); //uv2채널을 통해 선분의 시작점을 담음.
            List<Vector2> uv3s = new List<Vector2>(); //uv3채널을 통해 선분의 끝점을 담음. 


            // Create interior geometry
            int pointCount = shapePath.Length;
            var inputs = new ContourVertex[2 * pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                Color extrusionData = new Color(shapePath[i].x, shapePath[i].y, shapePath[i].x, shapePath[i].y);
                int nextPoint = (i + 1) % pointCount;
                inputs[2 * i] = new ContourVertex() { Position = new Vec3() { X = shapePath[i].x, Y = shapePath[i].y, Z = 0 }, Data = extrusionData };

                extrusionData = new Color(shapePath[i].x, shapePath[i].y, shapePath[nextPoint].x, shapePath[nextPoint].y);
                Vector2 midPoint = 0.5f * (shapePath[i] + shapePath[nextPoint]);
                inputs[2 * i + 1] = new ContourVertex() { Position = new Vec3() { X = midPoint.x, Y = midPoint.y, Z = 0 }, Data = extrusionData };
            }

            Tess tessI = new Tess();
            tessI.AddContour(inputs, ContourOrientation.Original);
            tessI.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3, InterpCustomVertexData);

            var indicesI = tessI.Elements.Select(i => i).ToArray();
            var verticesI = tessI.Vertices.Select(v => new Vector3(v.Position.X, v.Position.Y, 0)).ToArray();
            var extrusionI = tessI.Vertices.Select(v => new Color(((Color)v.Data).r, ((Color)v.Data).g, ((Color)v.Data).b, ((Color)v.Data).a)).ToArray();

            vertices.AddRange(verticesI);
            triangles.AddRange(indicesI);
            extrusion.AddRange(extrusionI);
            
            for(int i =0; i<vertices.Count; ++i)
            {
                uvs.Add(Vector2.zero);
                uv2s.Add(Vector2.zero);
                uv3s.Add(Vector2.zero);
            }

            InitializeTangents(vertices.Count, tangents);

            List<Edge> edges = new List<Edge>();
            PopulateEdgeArray(vertices, triangles, edges, uv2s, uv3s);
            SortEdges(edges);
            CreateShadowTriangles(vertices, extrusion, triangles, tangents, edges, uv2s, uv3s);

            Color[] finalExtrusion = extrusion.ToArray();
            Vector3[] finalVertices = vertices.ToArray();
            int[] finalTriangles = triangles.ToArray();
            Vector4[] finalTangents = tangents.ToArray();

            Vector2[] finalUv2s = uv2s.ToArray();
            Vector2[] finalUv3s = uv3s.ToArray();

            mesh.Clear();
            mesh.vertices = finalVertices;
            mesh.triangles = finalTriangles;
            mesh.tangents = finalTangents;
            mesh.colors = finalExtrusion;
            //mesh.uv = finalUvs;
            mesh.uv2 = finalUv2s;
            mesh.uv3 = finalUv3s;
        }
    }
}
