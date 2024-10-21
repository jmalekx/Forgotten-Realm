using UnityEngine;

public class PortalPickup : MonoBehaviour
{
    public GameObject portalPrefab; // Reference to the portal prefab
    public Transform portalSpawnPoint; // Where to spawn the portal

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object is tagged as "Player"
        {
            SpawnPortal(); // Call the method to spawn the portal
            Destroy(gameObject); // Destroy the pickup object
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
}
