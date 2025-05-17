using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    public string itemType;
    public Transform destination;
    public GameObject milly;
    private bool hasShownPrompt = false;


    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}");
        if (hasShownPrompt) return;

        if (other.gameObject == milly)
        {
            Debug.Log($"Eventerd if condition");

            PromptManager.Instance.ShowPrompt("Oh look! the mum lost her egg\nPress space bar to drop");
            hasShownPrompt = true;
        }
    }
}

//    void Update()
//    {
//        if (hasShownPrompt) return;

//        GameObject nearbyObject;

//        // Get the CharacterStateMachine component from Milly
//        CharacterStateMachine millyState = milly.GetComponent<CharacterStateMachine>();
//        if (itemType == "Egg" && millyState != null && millyState.IsObjectNearby(out nearbyObject))
//        {
//            PromptManager.Instance.ShowPrompt("?!!!!!!!!!");
//            PromptManager.Instance.ShowPrompt("Press space bar to pick it up");
//            hasShownPrompt = true;
//        }
//    }
//}
