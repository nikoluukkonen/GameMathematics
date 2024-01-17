using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingPopup : MonoBehaviour
{
    void OnDrawGizmos()
    {
        DrawPopup();
    }

    public void DrawPopup()
    {
        // Popup size calculation
        int width = (int)(Camera.main.pixelWidth * 0.60);
        int height = (int)(Camera.main.pixelHeight * 0.35);

        // Debug to check the calculations
        //Debug.Log(Camera.main.pixelWidth + ", " + Camera.main.pixelHeight + "\n" + width + ", " + height);

        // Drawing of the "popup" into the middle of the camera
        Gizmos.DrawCube(new Vector3(Camera.main.rect.position.x, Camera.main.rect.position.y, 750), new Vector3(width, height));
    }
}
