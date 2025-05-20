using System.Collections;
using UnityEngine;
using TMPro;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    public float typingSpeed = 0.05f;
    public float displayDuration = 3f; // Time after typing before disappearing
    public GameObject speechBubblePrefab;
    public Transform speechBubbleAnchor;
  
    private Coroutine currentTypingCoroutine;
    private GameObject currentBubble;
    private TMP_Text currentPromptText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
