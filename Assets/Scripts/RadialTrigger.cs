using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    public Transform Player;

    public float Radius = 1f;
    public float Height = 1f;

    public Light Light;

    void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position + Vector3.up * Height / 2, Vector3.up, Radius);
        Handles.DrawWireDisc(transform.position - Vector3.up * Height / 2, Vector3.up, Radius);
    }

    void Update()
    {
        if ((Player.position - transform.position).magnitude <= Radius)
            Light.enabled = true;
        else
            Light.enabled = false;
    }
}
