using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{
    public List<Vector3> Vertices = new();
    public List<int> Triangles = new();
    public List<Vector3> Normals = new();
    public List<Vector2> UVs = new();

    [Range(3, 100)] public int Segments;
    [Range(0.1f, 10f)] public float Radius1;
    [Range(0.1f, 10f)] public float Radius2;

    void Start()
    {
        GenerateMesh();
    }

    void OnValidate()
    {
        GenerateMesh();
    }

    void OnDrawGizmos()
    {
        //for (int i = 0; i < Vertices.Count; i++)
        //{
        //    Gizmos.DrawSphere(Vertices[i], 0.05f);
        //}
    }

    private void GenerateMesh()
    {
        Vertices.Clear();
        Triangles.Clear();
        Normals.Clear();
        UVs.Clear();

        Donut();

        Mesh mesh = new Mesh();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetNormals(Normals);
        mesh.SetUVs(0, UVs);

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
        {
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * Radius1, Mathf.Sin(angle * i) * Radius1));
            Vertices.Add(new Vector3(Mathf.Cos(angle * i) * Radius2, Mathf.Sin(angle * i) * Radius2));

            Normals.Add(Vector3.forward);
            Normals.Add(Vector3.forward);

            float t = i / (float)Segments;
            UVs.Add(new Vector2(t, 0));
            UVs.Add(new Vector2(t, 1));
        }

        for (int i = 0; i < Segments; i++)
        {
            int iRoot = i * 2;
            int iInnerRoot = iRoot + 1;
            int iInnerNext = (iRoot + 2) % Vertices.Count;
            int iOuterNext = (iRoot + 3) % Vertices.Count;

            Triangles.Add(iRoot);
            Triangles.Add(iInnerNext);
            Triangles.Add(iOuterNext);

            Triangles.Add(iRoot);
            Triangles.Add(iOuterNext);
            Triangles.Add(iInnerRoot);
        }
    }
}
