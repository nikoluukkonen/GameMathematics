using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshPath : MonoBehaviour
{
    public List<BezierPoint> Points = new();

    public bool Closed = false;

    public Transform Mover;
    [SerializeField]
    private int curCurve = 0;
    private Vector3 bezier;
    private float T = 0;
    private float speed = 0.01f;

    [SerializeField]
    [Range(0f, 1f)]
    float Tsimulation;

    public List<Vector3> Vertices = new();
    public List<int> Triangles = new();
    public List<Vector3> Normals = new();
    public List<Vector2> UVs = new();

    [Range(3, 100)] public int Segments;
    [Range(0.1f, 10f)] public float Radius1;
    [Range(0.1f, 10f)] public float Radius2;

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
        _CalcBezier(curCurve == Points.Count - 1);

        Mover.position = bezier;
        T += speed;

        if (T >= 1)
        {
            T = 0;
            curCurve++;
            if (curCurve == Points.Count)
                curCurve = 0;
        }
    }

    void OnDrawGizmos()
    {
        _DrawBezier();

        OrientedPoint point = OrientedPoint.CalculateOrientedPoint(Tsimulation, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), 
            Points[curCurve + 1].GetAnchorPos(), Points[curCurve + 1].GetControlPos(0));
        Mover.SetPositionAndRotation(point.pos, point.rot);

        Gizmos.DrawSphere(point.LocalToWorld(Vector3.right * 3), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 4) + Vector3.up), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 5) + Vector3.up), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 5) + Vector3.down), 0.1f);

        Gizmos.DrawSphere(point.LocalToWorld(Vector3.right * -3), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -4) + Vector3.up), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -5) + Vector3.up), 0.1f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -5) + Vector3.down), 0.1f);

        Gizmos.DrawLine(point.LocalToWorld(Vector3.right * 3), point.LocalToWorld((Vector3.right * 4) + Vector3.up));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * 4) + Vector3.up), point.LocalToWorld((Vector3.right * 5) + Vector3.up));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * 5) + Vector3.up), point.LocalToWorld((Vector3.right * 5) + Vector3.down));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * 5) + Vector3.down), point.LocalToWorld((Vector3.right * -5) + Vector3.down));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * -5) + Vector3.down), point.LocalToWorld((Vector3.right * -5) + Vector3.up));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * -5) + Vector3.up), point.LocalToWorld((Vector3.right * -4) + Vector3.up));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * -4) + Vector3.up), point.LocalToWorld((Vector3.right * -4) + Vector3.up));
        Gizmos.DrawLine(point.LocalToWorld((Vector3.right * -4) + Vector3.up), point.LocalToWorld(Vector3.right * -3));
        Gizmos.DrawLine(point.LocalToWorld(Vector3.right * -3), point.LocalToWorld(Vector3.right * 3));

    }

    private void _DrawBezier()
    {
        for (int i = 1; i < Points.Count; i++)
        {
            Handles.DrawBezier(Points[i - 1].GetAnchorPos(), Points[i].GetAnchorPos(),
                Points[i - 1].GetControlPos(1), Points[i].GetControlPos(0), Color.green, Texture2D.whiteTexture, 2f);
        }
        if (Closed)
            Handles.DrawBezier(Points[Points.Count - 1].GetAnchorPos(), Points[0].GetAnchorPos(),
                Points[Points.Count - 1].GetControlPos(1), Points[0].GetControlPos(0), Color.green, Texture2D.whiteTexture, 2f);
    }

    private void _CalcBezier(bool last)
    {
        if (!last)
            bezier = OrientedPoint.CalculateOrientedPoint(T, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), 
                Points[curCurve + 1].GetControlPos(0), Points[curCurve + 1].GetAnchorPos()).pos;
        else
            bezier = OrientedPoint.CalculateOrientedPoint(T, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), 
                Points[0].GetControlPos(0), Points[0].GetAnchorPos()).pos;
    }

    private void _GenerateMesh()
    {
        Vertices.Clear();
        Triangles.Clear();
        Normals.Clear();
        UVs.Clear();

        _Slice();

        Mesh mesh = new Mesh();
        mesh.SetVertices(Vertices);
        mesh.SetTriangles(Triangles, 0);
        mesh.SetNormals(Normals);
        mesh.SetUVs(0, UVs);

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }


    private void _Slice()
    {
        OrientedPoint point = OrientedPoint.CalculateOrientedPoint(Tsimulation, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), 
            Points[curCurve + 1].GetAnchorPos(), Points[curCurve + 1].GetControlPos(0));

        Vertices.Add(point.LocalToWorld(Vector3.right * 3));
        Vertices.Add(point.LocalToWorld((Vector3.right * 4) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * 5) + Vector3.down));

        Vertices.Add(point.LocalToWorld(Vector3.right * -3));
        Vertices.Add(point.LocalToWorld((Vector3.right * -4) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.up));
        Vertices.Add(point.LocalToWorld((Vector3.right * -5) + Vector3.down));

        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));
        Normals.Add(point.LocalToWorld(Vector3.forward));

        Triangles.Add(2);
        Triangles.Add(1);
        Triangles.Add(0);

        Triangles.Add(3);
        Triangles.Add(2);
        Triangles.Add(0);

        Triangles.Add(7);
        Triangles.Add(3);
        Triangles.Add(0);

        Triangles.Add(0);
        Triangles.Add(4);
        Triangles.Add(7);

        Triangles.Add(5);
        Triangles.Add(7);
        Triangles.Add(4);

        Triangles.Add(5);
        Triangles.Add(6);
        Triangles.Add(7);

    }

    private void _Road()
    {

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