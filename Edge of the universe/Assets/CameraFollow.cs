using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Reference to the player object
    public Vector3 offset;     // Offset to control camera distance

    void LateUpdate()
    {
        // Set the camera position based on the player's position + the offset
        transform.position = player.position + offset;
    }
}