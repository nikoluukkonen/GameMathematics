using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public Transform NPC, Player, NPClookDirection;

    public float Radius = 1f;
    public float LookRadius = 0.75f;
    [Range(0f, 180f)]
    public float AngleTreshold = 45f;
    public float Height = 1f;

    void OnDrawGizmos()
    {
        // Vectors from origin to the player and the npc
        //Vector.DrawVector(transform.position, NPC.position, Color.white);
        //Vector.DrawVector(transform.position, Player.position, Color.white);

        // Draw the radial trigger
        if ((Player.position - NPC.position).magnitude <= Radius) // Is player inside?
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(NPC.position, Radius);
            Vector.DrawVector(NPC.position, Player.position - NPC.position, Color.red);
        }
        else
        {
            Gizmos.color = Color.green;
            //Gizmos.DrawWireSphere(NPC.position, Radius);
            Vector.DrawVector(NPC.position, Player.position - NPC.position, Color.green);
        }

        // Vector to NPC's look direction
        Vector.DrawVector(NPC.position, NPClookDirection.position - NPC.position, Color.magenta);

        // Cone 
        if (Vector3.Dot((NPClookDirection.position - NPC.position).normalized, (Player.position - NPC.position).normalized) > LookRadius // Look-at-trigger
            && (Player.position - NPC.position).magnitude < (NPClookDirection.position - NPC.position).magnitude // Check if player is inside the viewing cone
            && (Player.position.y > NPC.position.y - Height && Player.position.y < NPC.position.y + Height)) // Clamp the Y-axis
        {
            Vector.DrawVector(NPC.position, Player.position - NPC.position, Color.blue);
        }

        Handles.color = Color.white;
        Handles.DrawWireDisc(NPC.position - Height / 2 * Vector3.up, Vector3.up, Radius);
        Handles.DrawWireDisc(NPC.position - Height / 2 * Vector3.down, Vector3.down, Radius);

        // Draw the wedge
        Quaternion q_rot = Quaternion.Euler(0, AngleTreshold, 0);
        Vector3 rotated = q_rot * NPClookDirection.position.normalized;
        //Vector.DrawVector(NPC.position, rotated * Radius, Color.white);
        q_rot = Quaternion.Euler(0, -AngleTreshold, 0);
        Vector3 rotated2 = q_rot * NPClookDirection.position.normalized;
        //Vector.DrawVector(NPC.position, rotated2 * Radius, Color.white);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(NPC.position + Vector3.up * Height / 2, NPC.position + Vector3.up * Height / 2 + rotated * Radius);
        Gizmos.DrawLine(NPC.position + Vector3.up * Height / 2, NPC.position + Vector3.up * Height / 2 + rotated2 * Radius);
        
        Gizmos.DrawLine(NPC.position + Vector3.down * Height / 2, NPC.position + Vector3.down * Height / 2 + rotated * Radius);
        Gizmos.DrawLine(NPC.position + Vector3.down * Height / 2, NPC.position + Vector3.down * Height / 2 + rotated2 * Radius);

        Gizmos.DrawLine(NPC.position, NPC.position + Vector3.up * Height / 2);
        Gizmos.DrawLine(NPC.position, NPC.position + Vector3.down * Height / 2);

        Gizmos.DrawLine(NPC.position + Vector3.up * Height / 2 + rotated * Radius, NPC.position + Vector3.down * Height / 2 + rotated * Radius);
        Gizmos.DrawLine(NPC.position + Vector3.up * Height / 2 + rotated2 * Radius, NPC.position + Vector3.down * Height / 2 + rotated2 * Radius);
    }
}
