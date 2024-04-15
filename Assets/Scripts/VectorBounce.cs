using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorBounce : MonoBehaviour
{
    [Range(1, 1000)]
    public int Bounces;

    void OnDrawGizmos()
    {
        _Bounce(transform.position, transform.forward, Bounces);
    }

    void _Bounce(Vector3 startPos, Vector3 dir, int bounces)
    {
        Ray ray = new(startPos, dir);
        Physics.Raycast(ray, out RaycastHit hit);

        Vector3 newRay = dir - 2 * Vector3.Dot(dir, hit.normal) * hit.normal;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, hit.point);
        //Gizmos.DrawLine(hit.point, newRay + hit.point);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(hit.point, hit.normal + hit.point);

        if (bounces > 0)
        {
            _Bounce(hit.point, newRay, bounces - 1);
        }
    }
}
