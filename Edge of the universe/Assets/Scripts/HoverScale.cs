using UnityEngine;

public class HoverScale : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(0.6f, 0.6f, 1f); // scale when hovered

    void Start()
    {
        originalScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        transform.localScale = hoverScale;
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}
