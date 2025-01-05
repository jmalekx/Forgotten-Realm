using UnityEngine;
using TMPro;

using UnityEngine;
using TMPro;

public class BlacksmithInteraction : MonoBehaviour
{
    //variables for prefabs and UI
    public GameObject swordPrefab; 
    public Transform swordSpawnPoint; 
    public GameObject dialogueUI;
    
    private bool playerInRange = false;
    public TMP_Text Etext;


    //Give e text when player is in range
    void Update()
    {
        if (playerInRange)
        {
            // Optionally, you can display the interaction hint for the "I" key here
            Etext.text = "Press I to interact with the villager";
        }
    }

    //Check if player is in range and set to true
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered range.");

        }
    }

    //Stop the interaction when player exits the range
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


    // Spawn the sword at the specified location
    void GivePlayerSword()
    {
        
        Instantiate(swordPrefab, swordSpawnPoint.position, swordSpawnPoint.rotation);
        Debug.Log("Player received a sword!");
    }
}

