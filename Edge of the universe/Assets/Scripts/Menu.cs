using UnityEngine;

public class Menu : MonoBehaviour
{
    public KeyCode button = KeyCode.M;
    private SpriteRenderer[] spriteRenderers;
    private Camera mainCamera;
    private bool isVisible = false;

    void Start()
    {
        // Get all SpriteRenderers in this object and its children
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Hide all at start
        foreach (var sr in spriteRenderers)
        {
            sr.enabled = false;
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        // Follow camera x, y (keep current z)
        Vector3 camPos = mainCamera.transform.position;
        transform.position = new Vector3(camPos.x-0.29f, camPos.y-2.5f, transform.position.z);

        if (Input.GetKeyDown(button))
        {
            isVisible = !isVisible;

            foreach (var sr in spriteRenderers)
            {
                sr.enabled = isVisible;
            }
        }

        
    }
}
