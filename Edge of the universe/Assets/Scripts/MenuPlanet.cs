using UnityEngine;
using System.Collections;


public class MenuPlanet : MonoBehaviour
{
    public Planet destinationPlanet; // assign this in Inspector
    private bool isPlanet3;
    public TMPro.TextMeshProUGUI currentPromptText;
    public GameObject playerMarkerPrefab; // Assign in Inspector
    public Vector3 miniMillyposition;
    private GameObject playerMarkerInstance;
    private SpriteRenderer playerMarkerRenderer;
    private bool isTyping=false;


    public void ShowPlayerMarker(bool show)
    {
        if (show)
        {
            if (playerMarkerInstance == null && playerMarkerPrefab != null)
            {
                playerMarkerInstance = Instantiate(playerMarkerPrefab, transform);
                playerMarkerInstance.transform.localPosition = miniMillyposition; 
                playerMarkerRenderer = playerMarkerInstance.GetComponent<SpriteRenderer>();

                //playerMarkerInstance.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            }
            else if (playerMarkerInstance != null)
            {
                playerMarkerInstance.SetActive(true);
            }
        }
        else if (playerMarkerInstance != null)
        {
            playerMarkerInstance.SetActive(false);
        }
    }
    private void Start()
    {

        isPlanet3 = (gameObject.name == "Planet 3");


    }
    public void SetVisible(bool visible)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();

        if (sr != null) sr.enabled = visible;
        if (col != null) col.enabled = visible;
        if (playerMarkerInstance != null)
        {
            playerMarkerRenderer = playerMarkerInstance.GetComponent<SpriteRenderer>();
            playerMarkerInstance.SetActive(visible);
        }
    }
    void OnMouseDown()
    {
        if (destinationPlanet == null)
        {
            Debug.LogWarning("No destination planet assigned!");
            return;
        }

        if (GameManager.Instance.IsCurrentPlanet(destinationPlanet))
        {
            // Already there
            return;
        }

        if (isPlanet3)
        {
            if (!isTyping)
            {
                // Show message instead of scaling
                StartCoroutine(TypeText("Sorry! Planet not available yet"));
            }
            else return;
           
        }
        else
        {

            StartCoroutine(HandlePlanetClick());
        }
    }

    private IEnumerator TypeText(string message)
    {
        currentPromptText.text = "";
        isTyping = true;

        foreach (char letter in message.ToCharArray())
        {
            currentPromptText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);

        // Wait before clearing
        currentPromptText.text = "";
        isTyping = false;

    }

    public void SetAlpha(float alpha)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, alpha);
        }

        if (playerMarkerInstance != null && playerMarkerRenderer != null)
        {
            Color c = playerMarkerRenderer.color;
            playerMarkerRenderer.color = new Color(c.r, c.g, c.b, alpha);
        }
    }

   
    private IEnumerator HandlePlanetClick()
    {
        /*
        MenuPlanet activeMarkerOwner = null;
        Animator activeAnimator = null;

        MenuPlanet[] allPlanets = Object.FindObjectsByType<MenuPlanet>(FindObjectsSortMode.None);

        foreach (var planet in allPlanets)
        {
            if (planet.playerMarkerInstance != null)
            {
                activeMarkerOwner = planet;
                activeAnimator = planet.playerMarkerInstance.GetComponent<Animator>();
                Debug.Log("Found playerMarker");
                break;
            }
        }
        */
        float waitTime = 0.5f; // fallback in case no animator is found

       /*
        if (activeAnimator != null)
        {
            activeAnimator.SetTrigger("Click");

            // Wait for the animation to finish based on clip length
            yield return null;  // wait for next frame

            AnimatorClipInfo[] clipInfos = activeAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfos.Length > 0)
            {
                waitTime = clipInfos[0].clip.length;
                Debug.Log($"Waiting for {waitTime} seconds for animation to finish.");
            }
        }
        */
        yield return new WaitForSeconds(waitTime);

        GameObject menu = GameObject.FindWithTag("Menu");
        if (menu != null)
        {
            menu.GetComponent<Menu>().HideMenuWithFade();
        }

        // Move player and camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera mainCamera = Camera.main;

        if (player != null && mainCamera != null)
        {
            Transform targetCamera = destinationPlanet.destinationPointCamera;
            Transform targetPlayer = destinationPlanet.destinationPointPlayer;

            player.transform.position = targetPlayer.position;
            mainCamera.transform.position = new Vector3(
                targetCamera.position.x,
                targetCamera.position.y,
                mainCamera.transform.position.z
            );

            GameManager.Instance.SetCurrentPlanet(destinationPlanet);
            MusicManager.Instance.PlayMusic(destinationPlanet.planetMusic);

        }
    }


}
