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

        if (item.itemType == acceptedType)
        {
            // Trigger reward after the sprite change (or move this before if you prefer)
            StartCoroutine(RewardPlayer());
        }
    }

    private IEnumerator ChangePhoneAfterDelay(HoldableItem item)
    {
        // Wait for 2 seconds (or whatever delay you want)
        yield return new WaitForSeconds(3f);

        phoneSpriteRenderer.sprite = happyPhoneSprite; // Change sprite
        Debug.Log("Phone changed");

    }



}
