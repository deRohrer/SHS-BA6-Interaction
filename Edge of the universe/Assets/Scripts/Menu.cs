using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public KeyCode button = KeyCode.M;
    public float fadeDuration = 1f;

    private SpriteRenderer[] spriteRenderers;
    private Camera mainCamera;
    private bool isVisible = false;

    private List<MenuPlanet> menuPlanets;

    void Awake()
    {
        // Get all MenuPlanet components in children of this Menu object
        menuPlanets = new List<MenuPlanet>(GetComponentsInChildren<MenuPlanet>());
    }
    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        mainCamera = Camera.main;
        SetAlpha(0f);  // Start hidden
        foreach (MenuPlanet planet in menuPlanets)
        {
            planet.SetVisible(false);  // Enable sprite and collider on each planet
        }
    }

    void Update()
    {
        // Follow camera
        Vector3 camPos = mainCamera.transform.position;
        transform.position = new Vector3(camPos.x - 0.29f, camPos.y - 2.5f, transform.position.z);

        // Press M to show/hide menu instantly
        if (Input.GetKeyDown(button))
        {
            if (isVisible)
                HideMenuInstantly();
            else
                ShowMenuInstantly();
        }
    }

    public void HideMenuWithFade() => StartCoroutine(FadeOutMenu());

    private void ShowMenuInstantly()
    {
        isVisible = true;
        SetAlpha(1f);

        foreach (MenuPlanet planet in menuPlanets)
        {
            planet.SetVisible(true);  // Enable sprite and collider on each planet
        }
    }

    private void HideMenuInstantly()
    {
        isVisible = false;
        SetAlpha(0f);
        foreach (MenuPlanet planet in menuPlanets)
        {
            planet.SetVisible(false);  // Enable sprite and collider on each planet
        }
    }

    private IEnumerator FadeOutMenu()
    {
        isVisible = false;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        foreach (MenuPlanet planet in menuPlanets)
        {
            planet.SetVisible(false);  // Enable sprite and collider on each planet
        }
    }

    private void SetAlpha(float alpha)
    {
        foreach (var sr in spriteRenderers)
        {
            if (sr != null)
            {
                Color c = sr.color;
                sr.color = new Color(c.r, c.g, c.b, alpha);
            }
        }
    }
}
