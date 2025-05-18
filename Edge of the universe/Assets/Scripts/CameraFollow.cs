using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Reference to the player object
    public Vector3 offset;     // Offset to control camera distance
    public bool followY=false;
    void LateUpdate()
    {
        // Set the camera position based on the player's position + the offset
        if (followY)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z + offset.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z + offset.z);
        }
    }
}