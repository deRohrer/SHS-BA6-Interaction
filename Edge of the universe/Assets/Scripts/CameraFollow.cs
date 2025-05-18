using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Reference to the player object
    public Vector3 offset;     // Offset to control camera distance

    void LateUpdate()
    {
        // Set the camera position based on the player's position + the offset
        transform.position = new Vector3(player.position.x + offset.x, player.position.y+offset.y, transform.position.z+offset.z);
    }
}