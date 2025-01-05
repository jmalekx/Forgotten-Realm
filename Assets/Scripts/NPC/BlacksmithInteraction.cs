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

    // Flag to track if the objective is already completed
    private bool objectiveCompleted = false;

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

            // Complete the objective if it hasn't been completed already
            if (!objectiveCompleted)
            {
                CompleteBlacksmithObjective();
            }

            // Show the interaction UI
            if (dialogueUI != null)
            {
                dialogueUI.SetActive(true);
            }
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

    void CompleteBlacksmithObjective()
    {
        // Track the objective when the player enters the range for the first time
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective("Speak to the blacksmith");
            Debug.Log("Objective Completed: Speak to the blacksmith");
            objectiveCompleted = true; // Set the flag to prevent repeated completion
        }
        else
        {
            Debug.LogError("ObjectiveManager.Instance is null. Make sure it is set up correctly.");
        }

        // After completing the objective, give the player the sword
        GivePlayerSword();
    }

    void GivePlayerSword()
    {
        // Spawn the sword at the specified location
        Instantiate(swordPrefab, swordSpawnPoint.position, swordSpawnPoint.rotation);
        Debug.Log("Player received a sword!");
    }
}

