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
