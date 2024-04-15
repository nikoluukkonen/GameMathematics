using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementRotation : MonoBehaviour
{
    public GameObject Object;

    private void OnDrawGizmos()
    {
        Ray ray = new(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit);

        Vector3 right = Vector3.Cross(hit.normal, transform.forward);
        Vector3 forward = Vector3.Cross(hit.normal, right);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, hit.point);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hit.point, hit.normal + hit.point);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hit.point, right + hit.point);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(hit.point, forward + hit.point);

        Object.transform.position = hit.point;
        Object.transform.rotation = Quaternion.LookRotation(forward, hit.normal);
    }
}
