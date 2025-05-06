using UnityEngine;

public class HoverScale : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); // scale when hovered

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
