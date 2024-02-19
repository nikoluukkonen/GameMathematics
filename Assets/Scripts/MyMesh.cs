using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{
    public List<Vector3> Vertices = new();
    public List<int> Triangles = new();
    public List<Vector2> UVs = new();

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
        for (int i = 0; i < Vertices.Count; i++)
        {
            Gizmos.DrawSphere(Vertices[i], 0.05f);
        }
    }

    private void GenerateMesh()
    {
        //Salmiakki();
        //Disc(32, 2);
        Donut(8, 1, 2);

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

    private void Disc(int segments, float radius)
    {
        Vertices.Add(Vector3.zero);

        float angle = Mathf.Deg2Rad * 360 / segments;

        for (int i = 0; i < segments; i++)
        {
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * radius, Mathf.Sin(angle * i) * radius));
        }
        for (int i = 1; i < segments; i++)
        {
            Triangles.Add(0);
            Triangles.Add(i + 1);
            Triangles.Add(i);
        }
        Triangles.Add(0);
        Triangles.Add(1);
        Triangles.Add(segments);
    }

    private void Donut(int segments, float radiusInner, float radiusOuter)
    {
        float angle = Mathf.Deg2Rad * 360 / (float)segments;

        for (int i = 0; i < segments; i++)
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * radiusInner, Mathf.Sin(angle * i) * radiusInner));
        for (int i = 0; i < segments; i++)
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * radiusOuter, Mathf.Sin(angle * i) * radiusOuter));

        for (int i = 0; i < segments -1; i++)
        {
                Triangles.Add(i);
                Triangles.Add(i + segments + 1);
                Triangles.Add(i + segments);
            
                Triangles.Add(i);
                Triangles.Add(i + 1);
                Triangles.Add(i + segments +1);
        }
    }
}
