using UnityEngine;
using System.Collections;

public class MenuFader : MonoBehaviour
{
    public float fadeDuration = 1f;

    private SpriteRenderer[] renderers;

    void Start()
    {
        // Get all SpriteRenderers in self and children
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void FadeOutMenu()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;

        // Capture original colors
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = originalColors[i];
                renderers[i].color = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, 0, t));
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set alpha to 0 at the end
        for (int i = 0; i < renderers.Length; i++)
        {
            Color c = originalColors[i];
            renderers[i].color = new Color(c.r, c.g, c.b, 0);
        }
    }
}
