using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingPopup : MonoBehaviour
{
    void OnDrawGizmos()
    {
        DrawPopupLine();
        //DrawPopupBox();
    }

    /// Draws the popup as described in the homework assignment
    public void DrawPopupLine()
    {
        // Popup size calculation
        int width = (int)(Camera.main.pixelWidth * 0.60);
        int height = (int)(Camera.main.pixelHeight * 0.35);

        // Start cordinate calculation
        Vector2 startCordinates = new Vector2((Camera.main.pixelWidth - width) / 2, -((Camera.main.pixelHeight - height) / 2));

        // Drawing of the "popup" into the middle of the "screen"
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startCordinates, new Vector2(startCordinates.x + width, startCordinates.y));
        Gizmos.DrawLine(startCordinates, new Vector2(startCordinates.x, startCordinates.y - height));
        Gizmos.DrawLine(new Vector2(startCordinates.x + width, startCordinates.y), new Vector2(startCordinates.x + width, startCordinates.y - height));
        Gizmos.DrawLine(new Vector2(startCordinates.x, startCordinates.y - height), new Vector2(startCordinates.x + width, startCordinates.y - height));

        // Drawing of the "screen"
        Gizmos.color = Color.black;
        Gizmos.DrawLine(Vector2.zero, new Vector2(Camera.main.pixelWidth, 0));
        Gizmos.DrawLine(Vector2.zero, new Vector2(0, -Camera.main.pixelHeight));
        Gizmos.DrawLine(new Vector2(0, -Camera.main.pixelHeight), new Vector2(Camera.main.pixelWidth, -Camera.main.pixelHeight));
        Gizmos.DrawLine(new Vector2(Camera.main.pixelWidth, 0), new Vector2(Camera.main.pixelWidth, -Camera.main.pixelHeight));
    }

    /// Draws the popup into the middle of the actual ingame camera
    public void DrawPopupBox()
    {
        // Popup size calculation
        int width = (int)(Camera.main.pixelWidth * 0.60);
        int height = (int)(Camera.main.pixelHeight * 0.35);

        // Drawing of the "popup" into the middle of the camera
        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(Camera.main.rect.position.x, Camera.main.rect.position.y, 750), new Vector3(width, height));

        // 750 in the above DrawCube call moves the drawn cube far enough for it to be about the correct size
        // when viewn through the Unity camera with it's default settings
    }
}
