using UnityEngine;
using TMPro; // For TextMeshPro

public class PortalPickup : MonoBehaviour
{
    public GameObject portalPrefab; // Reference to the portal prefab
    public Transform portalSpawnPoint; // Where to spawn the portal
    public TextMeshProUGUI finalGemText; // UI text for displaying messages
    private float messageDuration = 2f; // Duration to show the message

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is tagged as "Player"
        {
            if (GameManager.instance != null && GameManager.instance.GemCount >= 10)
            {
                SpawnPortal(); // Call the method to spawn the portal
                Destroy(gameObject); // Destroy the pickup object
            }
            else
            {
                ShowMessage("You need 10 gems to unlock the portal!");
            }
        }
    }

    void SpawnPortal()
    {
        // Instantiate the portal at the specified spawn point
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

    void ShowMessage(string message)
    {
        if (finalGemText != null)
        {
            finalGemText.text = message; // Set the message text
            finalGemText.gameObject.SetActive(true); // Show the message
            Invoke("HideMessage", messageDuration); // Hide the message after a delay
        }
    }

    void HideMessage()
    {
        if (finalGemText != null)
        {
            finalGemText.gameObject.SetActive(false); // Hide the message
        }
    }
}
