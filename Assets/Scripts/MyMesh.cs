using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{
    public List<Vector3> Vertices = new();
    public List<int> Triangles = new();
    public List<Vector2> UVs = new();

    [Range(3, 100)] public int Segments;
    [Range(0.1f, 10f)] public float Radius1;
    [Range(0.1f, 10f)] public float Radius2;

    void Start()
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        GenerateMesh();
    }

    void OnValidate()
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        GenerateMesh();
    }

    private void OnDrawGizmos()
    {
        //for (int i = 0; i < Vertices.Count; i++)
        //{
        //    Gizmos.DrawSphere(Vertices[i], 0.05f);
        //}
    }

    private void GenerateMesh()
    {
        //Salmiakki();
        //Disc();
        Donut();

        Mesh mesh = new Mesh();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void Salmiakki()
    {
        Vertices.Add(new Vector3(0, 0, 0));
        Vertices.Add(new Vector3(1, 0, 0));
        Vertices.Add(new Vector3(0.5f, 1, 0));
        Vertices.Add(new Vector3(1.5f, 1, 0));

        Triangles.Add(2);
        Triangles.Add(1);
        Triangles.Add(0);
        Triangles.Add(2);
        Triangles.Add(3);
        Triangles.Add(1);
    }

    private void Disc()
    {
        Vertices.Add(Vector3.zero);

        float angle = Mathf.Deg2Rad * 360 / Segments;

        for (int i = 0; i < Segments; i++)
        {
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * Radius1, Mathf.Sin(angle * i) * Radius1));
        }
        for (int i = 1; i < Segments; i++)
        {
            Triangles.Add(0);
            Triangles.Add(i + 1);
            Triangles.Add(i);
        }
        Triangles.Add(0);
        Triangles.Add(1);
        Triangles.Add(Segments);
    }

    private void Donut()
    {
        if (Radius2 < Radius1)
        {
            Debug.LogError("Radius2 cannot be smaller than Radius1 for the donut to work!");
            return;
        }

        float angle = Mathf.Deg2Rad * 360 / (float)Segments;

        for (int i = 0; i < Segments; i++)
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * Radius1, Mathf.Sin(angle * i) * Radius1));
        for (int i = 0; i < Segments; i++)
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * Radius2, Mathf.Sin(angle * i) * Radius2));

        for (int i = 0; i < Segments - 1; i++)
        {
            Triangles.Add(i);
            Triangles.Add(i + Segments + 1);
            Triangles.Add(i + Segments);

            Triangles.Add(i);
            Triangles.Add(i + 1);
            Triangles.Add(i + Segments + 1);
        }

        Triangles.Add(Segments - 1);
        Triangles.Add(0);
        Triangles.Add(Segments);

        Triangles.Add(Segments-1);
        Triangles.Add(Segments);
        Triangles.Add(Segments * 2 - 1);
    }
}
