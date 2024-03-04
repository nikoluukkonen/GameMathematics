using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshPath : MonoBehaviour
{
    public List<BezierPoint> Points = new();

    public bool Closed = false;

    public Transform Mover;

    private int curCurve = 0;
    private Vector3 bezier;
    private float T = 0;
    private float speed = 0.01f;

    [SerializeField]
    [Range(0f, 1f)]
    float Tsimulation;

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

        OrientedPoint point = _GetBezierPoint(Tsimulation, Points[0].GetAnchorPos(), Points[0].GetControlPos(1), Points[1].GetAnchorPos(), Points[1].GetControlPos(0));
        Mover.SetPositionAndRotation(point.pos, point.rot);

        Gizmos.DrawSphere(point.LocalToWorld(Vector3.right * 3), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 4) + Vector3.up), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 5) + Vector3.up), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * 5) + Vector3.down), 0.15f);
        
        Gizmos.DrawSphere(point.LocalToWorld(Vector3.right * -3), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -4) + Vector3.up), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -5) + Vector3.up), 0.15f);
        Gizmos.DrawSphere(point.LocalToWorld((Vector3.right * -5) + Vector3.down), 0.15f);
        
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
            bezier = _GetBezierPoint(T, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), Points[curCurve + 1].GetControlPos(0), Points[curCurve + 1].GetAnchorPos()).pos;
        else
            bezier = _GetBezierPoint(T, Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), Points[0].GetControlPos(0), Points[0].GetAnchorPos()).pos;
    }

    private OrientedPoint _GetBezierPoint(float t, Vector3 first_a, Vector3 first_c, Vector3 second_a, Vector3 second_c)
    {
        Vector3 pointA = Vector3.Lerp(first_a, first_c, t);
        Vector3 pointB = Vector3.Lerp(first_c, second_c, t);
        Vector3 pointC = Vector3.Lerp(second_c, second_a, t);

        Vector3 X = Vector3.Lerp(pointA, pointB, t);
        Vector3 Y = Vector3.Lerp(pointB, pointC, t);

        return new(Vector3.Lerp(X, Y, t), Quaternion.LookRotation(Y - X));
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
}