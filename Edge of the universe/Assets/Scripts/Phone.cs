using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class Phone : DestinationTarget
{

    public SpriteRenderer phoneSpriteRenderer;
    public Sprite sadPhoneSprite ;
    public Sprite happyPhoneSprite;

    public override void OnItemPlaced(HoldableItem item)
    {
        Debug.Log("Phone will be changed after delay");

        StartCoroutine(ChangePhoneAfterDelay(item));
    }

    private IEnumerator ChangePhoneAfterDelay(HoldableItem item)
    {
        // Wait for 2 seconds (or whatever delay you want)
        yield return new WaitForSeconds(3f);

        phoneSpriteRenderer.sprite = happyPhoneSprite; // Change sprite
        Debug.Log("Phone changed");
        ShowPrompt("Thank you! Now I can hear again. I feel whole");
        yield return new WaitForSeconds(3f);
        StartCoroutine(RewardPlayer());

    }



}
