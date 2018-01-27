using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickup : MonoBehaviour {

    public float lifeTime;
    public float diameter;
    float timeAlive;
    float startY;
    void Start()
    {
        startY = transform.position.y;
        Vector3[] vertices = new Vector3[0];
        int[] triangles = new int[0];
        ActualPrimitiveGenerator.getIcoSphereMeshData(out triangles, out vertices, diameter, 1);
        Vector2[] UVs = new Vector2[vertices.Length];
        for (int i = 0; i < UVs.Length; i++)
        {
            UVs[i] = new Vector2(0.65f, 0.9f);
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UVs;
        mesh.RecalculateNormals();

        timeAlive = 0.0f;
    }

    private void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y = startY+Mathf.Sin(Time.timeSinceLevelLoad*3)*0.3f;
        transform.position = newPos;
        transform.Rotate(Vector3.up * 45*Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag =="Player")
        {

        }
    }

}
