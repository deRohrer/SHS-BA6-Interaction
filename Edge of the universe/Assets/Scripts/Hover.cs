using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverSpeed = 2f;    // Speed of the up/down movement
    public float hoverHeight = 0.5f; // How far it moves up/down

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = startPos + new Vector3(0f, offset, 0f);
    }
}
