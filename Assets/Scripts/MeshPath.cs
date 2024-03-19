using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MeshPath : MonoBehaviour
{
    public List<BezierPoint> Points = new();

    public bool Closed = false;

    public List<Vector3> Vertices = new();
    public List<int> Triangles = new();
    public List<Vector3> Normals = new();
    public List<Vector2> UVs = new();

    [Range(10, 100)] public int Segments;

    public Transform Mover;
    private int curCurve = 0;
    private Vector3 bezier;
    private float T = 0;

    void Start()
    {
        _GenerateMesh();
    }

    void OnValidate()
    {
        _GenerateMesh();
    }

    void Update()
    {
        OrientedPoint point;
        if (curCurve != Points.Count - 1)
            point = OrientedPoint.CalculateOrientedPoint(T, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1),
                        Points[curCurve + 1].GetAnchorPos(), Points[curCurve + 1].GetControlPos(0));
        else
            point = OrientedPoint.CalculateOrientedPoint(T, Points[Points.Count - 1].GetAnchorPos(), Points[Points.Count - 1].GetControlPos(1),
                    Points[0].GetAnchorPos(), Points[0].GetControlPos(0));

        Mover.SetPositionAndRotation(point.pos, point.rot);
        T += Time.deltaTime * 0.5f;

        if (T >= 1)
        {
            T = 0;
            curCurve++;
            if (curCurve == Points.Count)
                curCurve = 0;
        }
    }

    //void OnDrawGizmos()
    //{
    //    _DrawBezier();

    //    for (int i = 0; i < Vertices.Count; ++i)
    //    {
    //        Gizmos.DrawSphere(Vertices[i], 0.1f);
    //    }
    //}

    //private void _DrawBezier()
    //{
    //    for (int i = 1; i < Points.Count; i++)
    //    {
    //        Handles.DrawBezier(Points[i - 1].GetAnchorPos(), Points[i].GetAnchorPos(),
    //            Points[i - 1].GetControlPos(1), Points[i].GetControlPos(0), Color.green, Texture2D.whiteTexture, 2f);
    //    }
    //    if (Closed)
    //        Handles.DrawBezier(Points[Points.Count - 1].GetAnchorPos(), Points[0].GetAnchorPos(),
    //            Points[Points.Count - 1].GetControlPos(1), Points[0].GetControlPos(0), Color.green, Texture2D.whiteTexture, 2f);
    //}

    private void _GenerateMesh()
    {
        Vertices.Clear();
        Triangles.Clear();
        Normals.Clear();
        UVs.Clear();

        _Road();

        Mesh mesh = new Mesh();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetNormals(Normals);
        mesh.SetUVs(0, UVs);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private void _Road()
    {
        int segsPerCurve = Segments / Points.Count;
        int maxSegs = segsPerCurve * Points.Count;

        // Vertices, normals, UVs
        for (int i = 0; i < Points.Count - 1; i++)
        {
            float t = 0;

            for (int j = 0; j < segsPerCurve; j++)
            {
                _AddRoadPointVertices(i, t);

                t += 1 / ((float)segsPerCurve - 1);
            }
        }
        if (Closed)
        {
            float t = 0;

            for (int j = 0; j < segsPerCurve; j++)
            {
                _AddRoadPointVertices(0, t, true);

                t += 1 / ((float)segsPerCurve - 1);
            }
        }

        // Triangles
        for (int i = 0; i < maxSegs - segsPerCurve - 1; i++)
            _AddRoadPointTriangles(i * 16);

        if (Closed)
        {
            for (int i = maxSegs - segsPerCurve - 1; i < maxSegs - 1; i++)
                _AddRoadPointTriangles(i * 16);

            _AddRoadPointTriangles((maxSegs - 1) * 16, true);
        }
    }

    private void _AddRoadPointVertices(int i, float t, bool closed = false)
    {
        OrientedPoint point;
        if (!closed)
            point = OrientedPoint.CalculateOrientedPoint(t, Points[i].GetAnchorPos(), Points[i].GetControlPos(1),
                        Points[i + 1].GetAnchorPos(), Points[i + 1].GetControlPos(0));
        else
            point = OrientedPoint.CalculateOrientedPoint(t, Points[Points.Count - 1].GetAnchorPos(), Points[Points.Count - 1].GetControlPos(1),
                    Points[0].GetAnchorPos(), Points[0].GetControlPos(0));

        Vertices.Add(point.LocalToWorld(Vector3.right * 3));
        Vertices.Add(point.LocalToWorld(Vector3.right * 3));
        Vertices.Add(point.LocalToWorld((Vector3.right * 4) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 4) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.down));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.down));

        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.down));
        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.down));
        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * -4) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * -4) + Vector3.up));
        Vertices.Add(point.LocalToWorld(Vector3.right * -3));
        Vertices.Add(point.LocalToWorld(Vector3.right * -3));

        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));
        Normals.Add(point.LocalToWorld(new Vector3(-0.7071f, 0.7071f)));
        Normals.Add(point.LocalToWorld(new Vector3(-0.7071f, 0.7071f)));
        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));
        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));
        Normals.Add(point.LocalToWorld(new Vector3(1, 0)));
        Normals.Add(point.LocalToWorld(new Vector3(1, 0)));
        Normals.Add(point.LocalToWorld(new Vector3(0, -1)));

        Normals.Add(point.LocalToWorld(new Vector3(0, -1)));
        Normals.Add(point.LocalToWorld(new Vector3(-1, 0)));
        Normals.Add(point.LocalToWorld(new Vector3(-1, 0)));
        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));
        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));
        Normals.Add(point.LocalToWorld(new Vector3(0.7071f, -0.7071f)));
        Normals.Add(point.LocalToWorld(new Vector3(0.7071f, -0.7071f)));
        Normals.Add(point.LocalToWorld(new Vector3(0, 1)));

        UVs.Add(new Vector2(0.2f, t));
        UVs.Add(new Vector2(0.2f, t));
        UVs.Add(new Vector2(0.1f, t));
        UVs.Add(new Vector2(0.1f, t));
        UVs.Add(new Vector2(0.05f, t));
        UVs.Add(new Vector2(0.05f, t));
        UVs.Add(new Vector2(0, t));
        UVs.Add(new Vector2(1, t));

        UVs.Add(new Vector2(1f, t));
        UVs.Add(new Vector2(1f, t));
        UVs.Add(new Vector2(1f, t));
        UVs.Add(new Vector2(1f, t));
        UVs.Add(new Vector2(0.9f, t));
        UVs.Add(new Vector2(0.9f, t));
        UVs.Add(new Vector2(0.8f, t));
        UVs.Add(new Vector2(0.8f, t));
    }

    private void _AddRoadPointTriangles(int zero, bool lastSegment = false)
    {
        if (!lastSegment)
        {
            for (int i = 0; i < 14; i += 2)
            {
                Triangles.Add(zero + 1 + i);
                Triangles.Add(zero + 17 + i);
                Triangles.Add(zero + 2 + i);

                Triangles.Add(zero + 2 + i);
                Triangles.Add(zero + 17 + i);
                Triangles.Add(zero + 18 + i);
            }

            Triangles.Add(zero + 15);
            Triangles.Add(zero + 31);
            Triangles.Add(zero + 0);

            Triangles.Add(zero + 0);
            Triangles.Add(zero + 31);
            Triangles.Add(zero + 16);
        }
        else
        {
            for (int i = 0; i < 14; i += 2)
            {
                Triangles.Add(zero + 1 + i);
                Triangles.Add(2 + i);
                Triangles.Add(zero + 2 + i);

                Triangles.Add(zero + 1 + i);
                Triangles.Add(1 + i);
                Triangles.Add(2 + i);
            }

            Triangles.Add(zero + 15);
            Triangles.Add(15);
            Triangles.Add(zero + 0);

            Triangles.Add(zero + 0);
            Triangles.Add(15);
            Triangles.Add(0);
        }
    }
}

public struct OrientedPoint
{
    public Vector3 pos;
    public Quaternion rot;

    public OrientedPoint(Vector3 _pos, Quaternion _rot)
    {
        pos = _pos;
        rot = _rot;
    }

    public Vector3 LocalToWorld(Vector3 _local)
    {
        return pos + rot * _local;
    }

    public static OrientedPoint CalculateOrientedPoint(float t, Vector3 first_a, Vector3 first_c, Vector3 second_a, Vector3 second_c)
    {
        Vector3 X = Vector3.Lerp(Vector3.Lerp(first_a, first_c, t), Vector3.Lerp(first_c, second_c, t), t);
        Vector3 Y = Vector3.Lerp(Vector3.Lerp(first_c, second_c, t), Vector3.Lerp(second_c, second_a, t), t);

        return new(Vector3.Lerp(X, Y, t), Quaternion.LookRotation(Y - X));
    }
}