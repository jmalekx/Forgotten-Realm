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
        ResetDialogue(); // Reset the dialogue and options
    }

    public void OnOption2Selected()
    {
        Debug.Log("Option 2 selected: I’m not sure");
        ResetDialogue(); // Reset the dialogue and options
        optionButtons.SetActive(false); // Hide the options
        dialogueUI.SetActive(false); // Hide the dialogue UI
        isInteracting = false; // Reset interaction state
        dialogueIndex = 0; // Reset the dialogue index
    }

    void ResetDialogue()
    {
        optionButtons.SetActive(false); // Hide the options
        dialogueUI.SetActive(false); // Hide the dialogue UI
        isInteracting = false; // Reset interaction state
        dialogueIndex = 0; // Reset the dialogue index
        Etext.text = "Press E to interact with the villager"; // Reset the Etext prompt
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true; // Player is in range
            Debug.Log("Player entered range.");
            Etext.text = "Press E to interact with the villager"; // Show the prompt
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Player is no longer in range
            Debug.Log("Player exited range.");
            Etext.text = ""; // Clear the prompt
            if (isInteracting)
            {
                ResetDialogue(); // Reset the dialogue if the player leaves while interacting
            }
        }
    }
}
