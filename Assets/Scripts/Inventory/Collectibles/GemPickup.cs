using UnityEngine;

public class GemCollect : MonoBehaviour
{
    private bool isCollected = false; // Ensure each gem is collected only once

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player")) // Ensure it's the player and only trigger once
        {
            isCollected = true; // Mark this gem as collected
            GameManager.instance.CollectGem(); // Notify the GameManager
            Destroy(gameObject); // Destroy the gem
        }
    }
}
