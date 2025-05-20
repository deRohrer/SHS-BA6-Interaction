using UnityEngine;
using UnityEngine.UI;

public class SpaceshipPartsManager : MonoBehaviour
{
    public int spaceshipPartsCollected = 0;
    public string partsCounterText; // Optional UI hook
    private int totalSpaceshipParts = 9;

    public void AddPart()
    {

        spaceshipPartsCollected++;
        Debug.Log("Collected spaceship part! Total: " + spaceshipPartsCollected);

        int partsRemaining = totalSpaceshipParts - spaceshipPartsCollected;
        PromptManager.Instance.ShowPrompt("That's a piece of my spaceship. Only " + partsRemaining + " more and I can go home!");
        
        
    }
}
