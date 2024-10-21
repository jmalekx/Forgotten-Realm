using UnityEngine;

public class BlacksmithInteraction : MonoBehaviour
{
    public GameObject swordPrefab; // This is the prefab for the sword
    public Transform swordSpawnPoint; // Where the sword will spawn and appear
    public GameObject dialogueUI; // UI for feedback
    public string interactKey = "e"; // Which key to press to interact with the villager
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            GivePlayerSword();
        }
    }

    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        playerInRange = true; 
        Debug.Log("Player entered range."); // Add this line
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true); // Show Interaction UI 
        }
    }
}

void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        playerInRange = false; 
        Debug.Log("Player exited range."); // Add this line
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false); // Show Interaction UI 
        }
    }
}


    void GivePlayerSword()
{
    // Spawn the sword at the specified location
    Instantiate(swordPrefab, swordSpawnPoint.position, swordSpawnPoint.rotation);
    
    // Hide the interaction UI after the player has received the sword
    if (dialogueUI != null)
    {
        dialogueUI.SetActive(false);
    }

    Debug.Log("Player received a sword!");
}

}
