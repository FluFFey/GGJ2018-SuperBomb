using System;
using System.Collections.Generic;
using UnityEngine;

public static class ActualPrimitiveGenerator
{
    private static Dictionary<Int64, int> middlePointIndexCache = new Dictionary<long, int>(); //TODO: move these to treetop...
    private static List<Vector3> verticesList = new List<Vector3>();  //TODO: move these to treetop...
    private static int indexCount = 0;

    public static void getIcoSphereMeshData(out int[] triangles, out Vector3[] vertices, float size =1.0f, int subdivisions=0)
    {
        float topSize = UnityEngine.Random.Range(1.0f, 2.0f);
        verticesList.Clear();
        middlePointIndexCache.Clear();
        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;
        verticesList.Add(new Vector3(-1, t, 0).normalized);
        verticesList.Add(new Vector3(1, t, 0).normalized);
        verticesList.Add(new Vector3(-1, -t, 0).normalized);
        verticesList.Add(new Vector3(1, -t, 0).normalized);
        verticesList.Add(new Vector3(0, -1, t).normalized);
        verticesList.Add(new Vector3(0, 1, t).normalized);
        verticesList.Add(new Vector3(0, -1, -t).normalized);
        verticesList.Add(new Vector3(0, 1, -t).normalized);
        verticesList.Add(new Vector3(t, 0, -1).normalized);
        verticesList.Add(new Vector3(t, 0, 1).normalized);
        verticesList.Add(new Vector3(-t, 0, -1).normalized);
        verticesList.Add(new Vector3(-t, 0, 1).normalized);

        vertices = new Vector3[verticesList.Count];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = verticesList[i];
        }
        //if (subdivisions ==0)
        {
            triangles = new[]{
                0,11,5,
                0,5,1,
                0,1,7,
                0,7,10,
                0,10,11,

                1,5,9,
                5,11,4,
                11,10,2,
                10,7,6,
                7,1,8,

                3,9,4,
                3,4,2,
                3,2,6,
                3,6,8,
                3,8,9,

                4,9,5,
                2,4,11,
                6,2,10,
                8,6,7,
                9,8,1
            };
        }
        //else
        {
            //triangles = new int[0];
            for (int o = 0; o < subdivisions; o++)
            {
                int[] newTriangles = new int[triangles.Length * 4];
                indexCount = verticesList.Count - 1;
                for (int i = 0; i < (triangles.Length / 3); i++)
                {
                    int indexA = getIndexOfMiddle(triangles[i * 3 + 0], triangles[i * 3 + 1]);
                    int indexB = getIndexOfMiddle(triangles[i * 3 + 1], triangles[i * 3 + 2]);
                    int indexC = getIndexOfMiddle(triangles[i * 3 + 2], triangles[i * 3 + 0]);

                    newTriangles[i * 12 + 0] = triangles[i * 3 + 0];
                    newTriangles[i * 12 + 1] = indexA;
                    newTriangles[i * 12 + 2] = indexC;

                    newTriangles[i * 12 + 3] = triangles[i * 3 + 1];
                    newTriangles[i * 12 + 4] = indexB;
                    newTriangles[i * 12 + 5] = indexA;

                    newTriangles[i * 12 + 6] = triangles[i * 3 + 2];
                    newTriangles[i * 12 + 7] = indexC;
                    newTriangles[i * 12 + 8] = indexB;

                    newTriangles[i * 12 + 9] = indexA;
                    newTriangles[i * 12 + 10] = indexB;
                    newTriangles[i * 12 + 11] = indexC;
                }

                vertices = new Vector3[verticesList.Count];
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = verticesList[i] * size;
                }
                triangles = newTriangles;
            }
        }        
    }

    private static int getIndexOfMiddle(int v1, int v2)
    {

        // first check if we have it already
        bool firstIsSmaller = v1 < v2;
        Int64 smallerIndex = firstIsSmaller ? v1 : v2;
        Int64 greaterIndex = firstIsSmaller ? v2 : v1;
        Int64 key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (middlePointIndexCache.TryGetValue(key, out ret))
        {
            return ret;
        }

        Vector3 point1 = verticesList[v1];
        Vector3 point2 = verticesList[v2];
        Vector3 middle = new Vector3(
            (point1.x + point2.x) / 2.0f,
            (point1.y + point2.y) / 2.0f,
            (point1.z + point2.z) / 2.0f);

        // add vertex makes sure point is on unit sphere
        verticesList.Add(middle.normalized);
        middlePointIndexCache.Add(key, ++indexCount);
        return indexCount;
    }

    // add vertex to mesh, fix position to be on unit sphere, return index
    //private int addVertex(Vector3 p)
    //{
    //    float length = Mathf.Sqrt(p.x * p.x + p.y * p.y + p.z * p.z);
    //    vertices.Add(new Vector3(p.x / length, p.y / length, p.z / length));
    //    return index++;
    //}
   
}
