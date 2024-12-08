using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlacksmithDialogue : MonoBehaviour
{
    public GameObject dialogueUI; // The panel for the dialogue text
    public TMP_Text Etext; // The prompt text (Press E to interact)
    public TMP_Text dialogueText; // The TextMeshPro UI component for the dialogue
    public GameObject optionButtons; // The panel for dialogue options
    public Button option1Button; // Button for the first option
    public Button option2Button; // Button for the second option
    public string interactKey = "e"; // Which key to press to interact with the villager

    public GameObject daggerPrefab; // Reference to the dagger object prefab
    public Transform playerInventory; // Optional: if you have an inventory system, store the player's inventory transform

    private bool playerInRange = false; // Tracks if the player is in range
    private bool isInteracting = false; // Tracks if the player is currently interacting
    private int dialogueIndex = 0; // Tracks the current line of dialogue

    private string[] dialogueLines = {
        "Hello, welcome to my blacksmith!",
        "How can I help you today?"
    };

    void Update()
    {
        // Check for interaction when the player is in range
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (!isInteracting)
            {
                StartInteraction(); // Start the interaction
            }
            else
            {
                if (dialogueIndex < dialogueLines.Length)
                {
                    ShowNextDialogue(); // Show the next line of dialogue
                }
                else
                {
                    ShowOptions(); // Show options after all dialogue lines
                }
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true; // Mark the interaction as started
        dialogueUI.SetActive(true); // Show the dialogue UI
        Etext.text = ""; // Hide the Etext prompt
        ShowNextDialogue(); // Display the first line of dialogue
    }

    void ShowNextDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex]; // Display the current dialogue line
        dialogueIndex++;
    }

    void ShowOptions()
    {
        dialogueUI.SetActive(false); // Hide the dialogue text panel
        optionButtons.SetActive(true); // Show the options panel
    }

    public void OnOption1Selected()
    {
        Debug.Log("Option 1 selected: I need tools");

        // Instantiate the dagger in front of the player or attach it to the player's hand/inventory
        if (daggerPrefab != null)
        {
            // If you want to give the dagger to the player directly, instantiate it at the player's position
            GameObject dagger = Instantiate(daggerPrefab, playerInventory.position, Quaternion.identity);

            // Optionally, you can adjust the dagger's position or parent it to the player's hand or inventory slot
            // For example, if you want to attach it to the player's hand:
            // dagger.transform.SetParent(playerHandTransform); // Replace with the actual hand transform

            Debug.Log("Dagger given to the player.");
        }
        else
        {
            Debug.LogWarning("Dagger prefab is not assigned!");
        }

        // Reset dialogue and hide options
        ResetDialogue();
    }

    public void OnOption2Selected()
    {
        Debug.Log("Option 2 selected: Nevermind");

        // Hide the options panel and dialogue panel
        optionButtons.SetActive(false);  // Hide the options panel
        dialogueUI.SetActive(false);     // Hide the dialogue UI

        // Reset the interaction state and dialogue index
        isInteracting = false;          // Set interaction flag to false
        dialogueIndex = 0;              // Reset the dialogue index
        
        // Reset the prompt to allow future interactions
        Etext.text = "Press E to interact with the villager";

        // If player leaves during the interaction, make sure the interaction is reset
        if (playerInRange)  // Optionally, check if player is still in range
        {
            // You can reset additional states here if necessary (like enabling player controls, etc.)
        }
    }

    void ResetDialogue()
    {
        optionButtons.SetActive(false); // Hide the options
        dialogueUI.SetActive(false); // Hide the dialogue UI
        isInteracting = false; // Reset interaction state
        dialogueIndex = 0; // Reset the dialogue index
        Etext.text = "Press E to interact with the villager"; // Reset the Etext prompt

        // Unlock the cursor (if it was locked) and make it visible again
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Player is in range
            Debug.Log("Player entered range.");  // Check if player enters the range
            Etext.text = "Press E to spawn your dagger"; // Show the prompt
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Player is no longer in range
            Debug.Log("Player exited range.");  // Check if player exits the range
            Etext.text = ""; // Clear the prompt
            if (isInteracting)
            {
                ResetDialogue(); // Reset the dialogue if the player leaves while interacting
            }
        }
    }
}
