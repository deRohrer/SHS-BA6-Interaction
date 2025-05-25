using UnityEngine;
using System.Collections;
using TMPro;


public class DestinationTarget : MonoBehaviour
{
    public string acceptedType;
    public GameObject rewardPrefab;
    public Transform rewardDropPoint;
    public float rewardLifetime = 2f;
    public Transform itemDropPoint;

    public float typingSpeed = 0.05f;
    public float displayDuration = 3f; // Time after typing before disappearing
    public GameObject speechBubblePrefab;
    public Transform speechBubbleAnchor;

    private Coroutine currentTypingCoroutine;
    private GameObject currentBubble;
    private TMP_Text currentPromptText;


    public virtual void OnItemPlaced(HoldableItem item)
    {
        // Base class can be empty or have generic logic
        Debug.Log("GenericOnItamPlaces");

    }
    public IEnumerator RewardPlayer()
    {
        Debug.Log("Starting reward player");

        // Delay or any animation
        yield return new WaitForSeconds(4.0f);

        // Spawn spaceship part at spawn point
        if (rewardPrefab != null)
        {
            GameObject reward = Instantiate(rewardPrefab, rewardDropPoint.position, Quaternion.identity);
            Destroy(reward, rewardLifetime);
        }


        // Increase spaceship part count
        FindFirstObjectByType<SpaceshipPartsManager>()?.AddPart();
    }


    public void ShowPrompt(string message)
    {
        if (currentTypingCoroutine != null)
        {
            StopCoroutine(currentTypingCoroutine);
        }

        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
            currentPromptText = null;
        }

        // Instantiate the speech bubble prefab as a child of the anchor transform
        currentBubble = Instantiate(speechBubblePrefab, speechBubbleAnchor.position, Quaternion.identity, speechBubbleAnchor);


        // Get the TMP_Text component inside the prefab (assumes it's on the prefab or child)
        currentPromptText = currentBubble.GetComponentInChildren<TMP_Text>();

        if (currentPromptText == null)
        {
            Debug.LogError("Speech bubble prefab is missing a TMP_Text component!");
            return;
        }

        //currentBubble.transform.localScale = Vector3.one;  // or new Vector3(1, 1, 1)

        currentTypingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        currentPromptText.text = ""; // Clear previous text

        foreach (char letter in message.ToCharArray())
        {
            currentPromptText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait before clearing
        yield return new WaitForSeconds(displayDuration);
        currentPromptText.text = "";

        if (currentBubble != null)
        {
            Destroy(currentBubble);
            currentBubble = null;
            currentPromptText = null;
        }
    }

}
