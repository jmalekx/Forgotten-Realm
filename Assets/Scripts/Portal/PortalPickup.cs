using UnityEngine;
using TMPro; // For TextMeshPro

public class PortalPickup : MonoBehaviour
{
    //variables
    public GameObject portalPrefab; //portal prefab
    public Transform portalSpawnPoint; // Where to spawn the portal
    public TextMeshProUGUI finalGemText; // UI text to display final gem text 
    private float messageDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player")) 
        {
            if (GameManager.instance != null && GameManager.instance.GemCount >= 10)
            {
                SpawnPortal(); //Spawn the portal
                Destroy(gameObject); //Destroy the gem object
            }
            else
            {
                //if the player tries to collect the gem before collecting all 10 gems show below message
                ShowMessage("You need 10 gems to unlock the portal!");
            }
        }
    }

    //method to spawn the portal 
    void SpawnPortal()
    {
        // Instantiate the portal at the specific spawn point
        if (portalPrefab != null && portalSpawnPoint != null)
        {
            Instantiate(portalPrefab, portalSpawnPoint.position, portalSpawnPoint.rotation);
            Debug.Log("Portal has appeared!");
        }
        else
        {
            Debug.LogWarning("Portal prefab or spawn point not assigned!");
        }
    }

    //method to show the message if 10 gems arent met 
    void ShowMessage(string message)
    {
        if (finalGemText != null)
        {
            finalGemText.text = message; // Set the message text
            finalGemText.gameObject.SetActive(true); // Show the message
            Invoke("HideMessage", messageDuration); // Hide the message after a delay
        }
    }

    //method to hide the message
    void HideMessage()
    {
        if (finalGemText != null)
        {
            finalGemText.gameObject.SetActive(false); // Hide the message
        }
    }
}
