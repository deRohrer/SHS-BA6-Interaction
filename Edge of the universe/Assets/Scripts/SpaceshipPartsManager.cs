using UnityEngine;
using UnityEngine.UI;

public class SpaceshipPartsManager : MonoBehaviour
{
    public int spaceshipPartsCollected = 0;
    public Text partsCounterText; // Optional UI hook

    public void AddPart()
    {
        spaceshipPartsCollected++;
        Debug.Log("Collected spaceship part! Total: " + spaceshipPartsCollected);

        if (partsCounterText != null)
        {
            partsCounterText.text = "Parts: " + spaceshipPartsCollected;
        }
    }
}
