using UnityEngine;

public class PlayerLooper : MonoBehaviour
{
    public float minX = 0f;    // Start of the world
    public float maxX = 100f;  // End of the world

    void Update()
    {
        Vector3 pos = transform.position;

        if (pos.x > maxX)
        {
            pos.x = minX;
            transform.position = pos;
        }
        else if (pos.x < minX)
        {
            pos.x = maxX;
            transform.position = pos;
        }
    }
}
