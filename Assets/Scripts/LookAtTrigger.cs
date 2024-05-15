using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    public Transform Player, LookDirection;

    public Light Light;

    public float LookRadius = 0.75f;

    void OnDrawGizmos()
    {
        Vector.DrawVector(transform.position,LookDirection.position, Color.magenta);
    }

    void Update()
    {
        if (Vector3.Dot((LookDirection.position - transform.position).normalized, (Player.position - transform.position).normalized) > LookRadius) 
        {
            Light.enabled = true;
        }
        else
            Light.enabled = false;
    }
}
