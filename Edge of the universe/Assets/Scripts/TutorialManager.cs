using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    private bool hasMovedRight = false;
    private bool hasJumped = false;
    private bool hasMovedLeft = false;
    private bool hasPressedM = false;

    private bool startedTut = false;

    public void StartMovementTutorial()
    {
        startedTut = true;
        PromptManager.Instance.ShowPrompt("Press → to move right.");
    }

    void Update()
    {
        if (!startedTut)
            return;

        if (!hasMovedRight && Input.GetKeyDown(KeyCode.RightArrow))
        {
            hasMovedRight = true;
            StartCoroutine(ShowPromptWithDelay("Now press ← to move left.", 3f));
        }
        else if (hasMovedRight && !hasMovedLeft && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            hasMovedLeft = true;
            StartCoroutine(ShowPromptWithDelay("Nice! Now press ↑ to jump.", 3f));
        }
        else if (hasMovedLeft && !hasJumped && Input.GetKeyDown(KeyCode.UpArrow))
        {
            hasJumped = true;
        }
    }

    IEnumerator ShowPromptWithDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        PromptManager.Instance.ShowPrompt(message);
    }

}
