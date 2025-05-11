using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Planet currentPlanet;
    public void HideChildObjectsByParent(string parentObjectName)
    {
        GameObject parentObject = GameObject.Find(parentObjectName);  // Find parent object (Menu)
        if (parentObject != null)
        {
            // Get all child objects under the parent object
            foreach (Transform child in parentObject.transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();  // Get the Renderer of each child
                if (childRenderer != null)
                {
                    childRenderer.enabled = false;  // Disable the renderer to hide the child object
                    Debug.Log(child.name + " is now hidden.");
                }
                else
                {
                    Debug.LogWarning(child.name + " does not have a Renderer component.");
                }
            }
        }
        else
        {
            Debug.LogError("Parent object not found with name: " + parentObjectName);
        }
    }

    void Awake()
    {
        // Singleton pattern to ensure one instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional if you want it to persist between scenes
        }
    }

    public void SetCurrentPlanet(Planet newPlanet)
    {
        currentPlanet = newPlanet;
    }

    public bool IsCurrentPlanet(Planet planet)
    {
        return currentPlanet == planet;
    }
}

