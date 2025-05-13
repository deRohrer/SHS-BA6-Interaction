using UnityEngine;

public class MenuPlanet : MonoBehaviour
{
    public Planet destinationPlanet; // assign this in Inspector

    void OnMouseDown()
    {
        if (destinationPlanet == null)
        {
            Debug.LogWarning("No destination planet assigned!");
            return;
        }

        if (GameManager.Instance.IsCurrentPlanet(destinationPlanet))
        {
            // Already there
            return;
        }

        GameObject menu = GameObject.FindWithTag("Menu");
        if (menu != null)
        {
            menu.GetComponent<Menu>().HideMenuWithFade();
        }

        // Move player and camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera mainCamera = Camera.main;

        if (player != null && mainCamera != null)
        {
            Transform targetCamera = destinationPlanet.destinationPointCamera;
            Transform targetPlayer= destinationPlanet.destinationPointPlayer;


            player.transform.position = targetPlayer.position;
            mainCamera.transform.position = new Vector3(
                targetCamera.position.x,
                targetCamera.position.y,
                mainCamera.transform.position.z
            );

            GameManager.Instance.SetCurrentPlanet(destinationPlanet);

            //GameManager.Instance.HideChildObjectsByParent("Menu");

        }
    }
}
