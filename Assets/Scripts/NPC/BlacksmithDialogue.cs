using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlacksmithDialogue : MonoBehaviour
{
    public GameObject dialogueUI; // The panel for the dialogue text
    public TMP_Text Etext; // The prompt text (Press I to interact)
    public TMP_Text dialogueText; // The TextMeshPro UI component for the dialogue
    public GameObject optionButtons; // The panel for dialogue options
    public Button option1Button; // Button for the first option
    public Button option2Button; // Button for the second option
    public string interactKey = "i"; // Which key to press to interact with the villager

    public GameObject daggerPrefab; // Reference to the dagger object prefab
    public Transform spawnPoint; // Assign this in the Inspector to the empty GameObject representing the spawn point

    private bool playerInRange = false; // Tracks if the player is in range
    private bool isInteracting = false; // Tracks if the player is currently interacting
    private int dialogueIndex = 0; // Tracks the current line of dialogue
    private bool awaitingPlayerInput = false; // Tracks if waiting for player input to end the dialogue

    private string[] dialogueLines = {
        "Hello, welcome to my blacksmith!",
        "How can I help you today?"
    };

    private string daggerExplanation = "Here is your gem! good luck on your quest!";

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (awaitingPlayerInput)
            {
                EndInteraction(); // End the interaction if we're waiting for player input
            }
            else if (!isInteracting)
            {
                StartInteraction();
            }
            else
            {
                if (dialogueIndex < dialogueLines.Length)
                {
                    ShowNextDialogue();
                }
                else
                {
                    ShowOptions();
                }
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        dialogueUI.SetActive(true);
        Etext.text = "";
        ShowNextDialogue();

        // Unlock the cursor for interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowNextDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        dialogueIndex++;
    }

    void ShowOptions()
    {
        dialogueUI.SetActive(false);
        optionButtons.SetActive(true);
    }

    public void OnOption1Selected()
    {
        Debug.Log("Option 1 selected: I need tools");

        if (daggerPrefab != null && spawnPoint != null)
        {
            // Spawn the dagger at the specified spawn point
            Instantiate(daggerPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Dagger spawned at the spawn point.");
        }
        else
        {
            Debug.LogWarning("Dagger prefab or spawn point is not assigned!");
        }

        // Show the dagger explanation dialogue
        ShowDaggerExplanation();
    }

    void ShowDaggerExplanation()
    {
        optionButtons.SetActive(false); // Hide the options panel
        dialogueUI.SetActive(true); // Show the dialogue UI
        dialogueText.text = daggerExplanation; // Display the dagger explanation

        // Wait for player input to end dialogue
        awaitingPlayerInput = true;
    }

    public void OnOption2Selected()
    {
        Debug.Log("Option 2 selected: Nevermind");

        optionButtons.SetActive(false);
        dialogueUI.SetActive(false);

        ResetDialogue();
    }

    void EndInteraction()
    {
        Debug.Log("Ending interaction.");
        ResetDialogue();
    }

    void ResetDialogue()
    {
        dialogueUI.SetActive(false);
        optionButtons.SetActive(false);
        isInteracting = false;
        awaitingPlayerInput = false;
        dialogueIndex = 0;
        Etext.text = "Press I to interact with the villager";

        // Lock the cursor back after interaction
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered range.");
            Etext.text = "Press I to interact with the villager";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited range.");
            Etext.text = "";
            if (isInteracting || awaitingPlayerInput)
            {
                ResetDialogue();
            }
        }
    }
}
