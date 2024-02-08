using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public Transform point0, point1, point2, point3;
    [Range(0f, 1f)]
    public float t;

    private Vector3 pointA, pointB, pointC, X, Y, bezier;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawLine(point0.position, point1.position);
        Gizmos.DrawLine(point1.position, point2.position);
        Gizmos.DrawLine(point2.position, point3.position);

        pointA = Vector3.Lerp(point0.position, point1.position, t);
        pointB = Vector3.Lerp(point1.position, point2.position, t);
        pointC = Vector3.Lerp(point2.position, point3.position, t);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(pointA, 0.15f);
        Gizmos.DrawSphere(pointB, 0.15f);
        Gizmos.DrawSphere(pointC, 0.15f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawLine(pointB, pointC);

        X = Vector3.Lerp(pointA, pointB, t);
        Y = Vector3.Lerp(pointB, pointC, t);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(X, 0.15f);
        Gizmos.DrawSphere(Y, 0.15f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(X, Y);

        bezier = Vector3.Lerp(X, Y, t);

        Gizmos.color=Color.black;
        Gizmos.DrawSphere(bezier, 0.15f);

        Handles.DrawBezier(point0.position, point3.position, point1.position, point2.position, Color.green, Texture2D.whiteTexture, 2f);
    }
}
