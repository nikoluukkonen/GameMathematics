using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public List<BezierPoint> Points = new();


    public bool Closed = false;

    public Transform Mover;

    private int curCurve = 0;
    private Vector3 pointA, pointB, pointC, X, Y, bezier;
    private float T = 0;
    private float speed = 0.01f;

    void Update()
    {
        CalcBezier(curCurve == Points.Count - 1);

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
        DrawBezier();
    }

    private void DrawBezier()
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

    private void CalcBezier(bool last)
    {
        if (!last)
        {
            pointA = Vector3.Lerp(Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), T);
            pointB = Vector3.Lerp(Points[curCurve].GetControlPos(1), Points[curCurve + 1].GetControlPos(0), T);
            pointC = Vector3.Lerp(Points[curCurve + 1].GetControlPos(0), Points[curCurve + 1].GetAnchorPos(), T);
        }
        else
        {
            pointA = Vector3.Lerp(Points[curCurve].GetAnchorPos(), Points[curCurve].GetControlPos(1), T);
            pointB = Vector3.Lerp(Points[curCurve].GetControlPos(1), Points[0].GetControlPos(0), T);
            pointC = Vector3.Lerp(Points[0].GetControlPos(0), Points[0].GetAnchorPos(), T);

        }

        X = Vector3.Lerp(pointA, pointB, T);
        Y = Vector3.Lerp(pointB, pointC, T);

        bezier = Vector3.Lerp(X, Y, T);
    }
}
