using System.Collections;
using UnityEngine;
using TMPro;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    public TMP_Text promptText; // Assign this in the Inspector
    public float typingSpeed = 0.05f;
    public float displayDuration = 3f; // Time after typing before disappearing

    private Coroutine currentTypingCoroutine;

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

        currentTypingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        promptText.text = ""; // Clear previous text

        foreach (char letter in message.ToCharArray())
        {
            promptText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait before clearing
        yield return new WaitForSeconds(displayDuration);
        promptText.text = "";
    }
}
