using UnityEditor;
using UnityEngine;

public class Vectors : MonoBehaviour
{
    public Transform target;

    void OnDrawGizmos()
    {
        // X-axis
        DrawVector(Vector3.zero, new Vector3(5, 0, 0), Color.red);
        // Y-axis
        DrawVector(Vector3.zero, new Vector3(0, 5, 0), Color.green);

        // Unit sphere
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector3(-1, 0, 0), new Vector3(1, 0, 0));
        Gizmos.DrawLine(new Vector3(0, -1, 0), new Vector3(0, 1, 0));
        Gizmos.DrawLine(new Vector3(0, 0, -1), new Vector3(0, 0, 1));
        Gizmos.DrawWireSphere(Vector3.zero, 1);

        // Draw vector to target's position
        DrawVector(Vector3.zero, target.position, Color.magenta);
    }

    public void DrawVector(Vector3 pos, Vector3 v, Color c)
    {
        Gizmos.color = c;
        Gizmos.DrawLine(pos, pos + v);
        Handles.color = c;
        Handles.ConeHandleCap(0, pos + v - v.normalized * 0.35f, Quaternion.LookRotation(v), 0.5f, EventType.Repaint);
    }
}
