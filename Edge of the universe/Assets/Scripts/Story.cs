using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public static Story Instance { get; private set; }

    public Camera mainCamera;
    private List<GameObject> frames = new List<GameObject>();

    void Awake()
    {
        // Singleton pattern: destroy duplicate instances
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Get the main camera reference if not assigned in Inspector
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        frames.Clear();
        foreach (Transform child in transform)
        {
            frames.Add(child.gameObject);
        }
    }

    public IEnumerator PlayStoryAndWait()
    {
        yield return StartCoroutine(BeginningStoryCoroutine());
    }

    public void StartBeginningStory()
    {
        StartCoroutine(BeginningStoryCoroutine());
    }

    private IEnumerator BeginningStoryCoroutine()
    {
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found.");
            yield break;
        }

        Vector3 camPos = mainCamera.transform.position;
        transform.position = new Vector3(-12.8f, 7.5f, transform.position.z);

        foreach (GameObject frame in frames)
        {
            SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.enabled = true;              // Show this frame
                yield return new WaitForSeconds(5f); // Wait for some time
                sr.enabled = false;             // Hide it before the next one
            }
        }
    }
}
