using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Interpolation : MonoBehaviour
{
    public GameObject Cube;
    public Transform PointA, PointB;

    public float InterpTime = 5.0f;

    private float elapsedTime = 0f;
    private float t = 0f;


    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > InterpTime)
            elapsedTime = InterpTime;

        // Interpolate until InterpTime
        t = Easing(elapsedTime / InterpTime);

        Vector3 pos = (1 - t) * PointA.position + t * PointB.position;
        Cube.transform.position = pos;
    }

    void OnDrawGizmos()
    {
        DrawVector(Vector3.zero, PointA.position, Color.green);
        DrawVector(Vector3.zero, PointB.position, Color.red);

        DrawVector(Vector3.zero, (1 - t) * PointA.position, Color.black);
        DrawVector((1 - t) * PointA.position, t * PointB.position, Color.gray);
    }

    public void DrawVector(Vector3 pos, Vector3 v, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos + v);
        Handles.color = c;
        Handles.ConeHandleCap(0, pos + v - v.normalized * 0.35f, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);
    }

    private float Easing(float x)
    {
        return (float)(x < 0.5 ? 2 * x * x : 1 - (-2 * (1 - x) * (1 - x)));
    }
}
