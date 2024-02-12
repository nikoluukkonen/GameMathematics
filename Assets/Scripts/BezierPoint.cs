using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    [SerializeField] Transform[] Controls = new Transform[2];

    public Vector3 GetAnchorPos() => transform.position;
    public Vector3 GetControlPos(int index) => Controls[index].position;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(GetAnchorPos(), GetControlPos(0));
        Gizmos.DrawLine(GetAnchorPos(), GetControlPos(1));
    }
}
