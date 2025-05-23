using UnityEngine;

public class HoverScale : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 hoverScale = new Vector3(0.6f, 0.6f, 1f); // scale when hovered
    private bool isPlanet3;


    void Start()
    {
        originalScale = transform.localScale;
        isPlanet3 = (gameObject.name == "Planet 3");

    }

    void OnMouseEnter()
    {
        if (!isPlanet3) { 
            transform.localScale = hoverScale;
        }
    }

    void OnMouseExit()
    {
        if (!isPlanet3)
        {
            transform.localScale = originalScale;
        }
    }



}
