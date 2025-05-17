using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class Nest : DestinationTarget
{

    public SpriteRenderer birdSpriteRenderer;
    public Sprite sadBirdSprite;
    public Sprite happyBirdSprite;
    public GameObject milly;
    private bool promptShown = false;
    public override void OnItemPlaced(HoldableItem item)
    {
        Debug.Log("Bird will be changed after delay");

        StartCoroutine(ChangeBirdAfterDelay(item));

        if (item.itemType == acceptedType)
        {
            // Trigger reward after the sprite change (or move this before if you prefer)
            StartCoroutine(RewardPlayer());
        }
    }

    private IEnumerator ChangeBirdAfterDelay(HoldableItem item)
    {
        // Wait for 2 seconds (or whatever delay you want)
        yield return new WaitForSeconds(3f);

        birdSpriteRenderer.sprite = happyBirdSprite; // Change sprite
        Debug.Log("Bird changed");
   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (promptShown) return;

        if (other.gameObject == milly)
        {
            PromptManager.Instance.ShowPrompt("Oh look! the mum lost her egg\nPress space bar to drop");
            promptShown = true;
        }
    }

}
