using UnityEngine;
using TMPro;

using UnityEngine;
using TMPro;

public class BlacksmithInteraction : MonoBehaviour
{
    public GameObject swordPrefab; // This is the prefab for the sword
    public Transform swordSpawnPoint; // Where the sword will spawn and appear
    public GameObject dialogueUI; // UI for feedback
    
    private bool playerInRange = false;
    public TMP_Text Etext;

    void Update()
    {
        if (playerInRange)
        {
            // Optionally, you can display the interaction hint for the "I" key here
            Etext.text = "Press I to interact with the villager";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered range.");

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited range.");

            // Hide the interaction UI when the player leaves the range
            if (dialogueUI != null)
            {
                dialogueUI.SetActive(false);
            }

            // Clear the text prompt
            Etext.text = "";
        }
    }


    void GivePlayerSword()
    {
        // Spawn the sword at the specified location
        Instantiate(swordPrefab, swordSpawnPoint.position, swordSpawnPoint.rotation);
        Debug.Log("Player received a sword!");
    }
}

