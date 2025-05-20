using UnityEngine;
using System.Collections;


public class DestinationTarget : MonoBehaviour
{
    public string acceptedType;
    public GameObject rewardPrefab;
    public Transform rewardDropPoint;
    public float rewardLifetime = 2f;
    public Transform itemDropPoint;
   

    public virtual void OnItemPlaced(HoldableItem item)
    {
        // Base class can be empty or have generic logic
        Debug.Log("GenericOnItamPlaces");

    }
    public IEnumerator RewardPlayer()
    {
        Debug.Log("Starting reward player");

        // Delay or any animation
        yield return new WaitForSeconds(4.0f);

        // Spawn spaceship part at spawn point
        if (rewardPrefab != null)
        {
            GameObject reward = Instantiate(rewardPrefab, rewardDropPoint.position, Quaternion.identity);
            Destroy(reward, rewardLifetime);
        }


        // Increase spaceship part count
        FindFirstObjectByType<SpaceshipPartsManager>()?.AddPart();
    }
}
