using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class Nest : DestinationTarget
{

    public SpriteRenderer birdSpriteRenderer;
    public Sprite sadBirdSprite;
    public Sprite happyBirdSprite;
  
    public override void OnItemPlaced(HoldableItem item)
    {
        Debug.Log("Bird will be changed after delay");

        StartCoroutine(ChangeBirdAfterDelay(item));

    
    }

    private IEnumerator ChangeBirdAfterDelay(HoldableItem item)
    {
        // Wait for 2 seconds (or whatever delay you want)
        yield return new WaitForSeconds(4f);

        birdSpriteRenderer.sprite = happyBirdSprite; // Change sprite
        Debug.Log("Bird changed");
        ShowPrompt("Chirp, my baby! Chirp, thank you for bringing my egg back!");
        yield return new WaitForSeconds(3f);
        StartCoroutine(RewardPlayer());
    }



}
