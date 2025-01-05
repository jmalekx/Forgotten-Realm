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
    public GameObject swordPrefab; // Reference to the sword object prefab (new addition)
    public Transform spawnPoint; // Assign this in the Inspector to the empty GameObject representing the spawn point

    private bool playerInRange = false; // Tracks if the player is in range
    private bool isInteracting = false; // Tracks if the player is currently interacting
    private int dialogueIndex = 0; // Tracks the current line of dialogue
    private bool awaitingPlayerInput = false; // Tracks if waiting for player input to end the dialogue

    private string[] dialogueLines = {
        "Hello, welcome to my blacksmith!",
        "How can I help you today?"
    };

    private string daggerExplanation = "Here is your gem! Good luck on your quest!";
    private string swordExplanation = "Here is your sword! May it help you in battle!"; // New explanation for the sword

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

        // Mark the objective as complete
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective("Collect gem from blacksmith");
            Debug.Log("Objective 'Collect gem from blacksmith' marked as complete.");
        }
        else
        {
            Debug.LogError("ObjectiveManager.Instance is null!");
        }
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
        Debug.Log("Option 2 selected: I need a sword");

        if (swordPrefab != null && spawnPoint != null)
        {
            // Spawn the sword at the specified spawn point
            Instantiate(swordPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Sword spawned at the spawn point.");
        }
        else
        {
            Debug.LogWarning("Sword prefab or spawn point is not assigned!");
        }

        // Show the sword explanation dialogue
        ShowSwordExplanation();

        // Mark the objective as complete
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective("Collect sword from blacksmith");
            Debug.Log("Objective 'Collect sword from blacksmith' marked as complete.");
        }
        else
        {
            Debug.LogError("ObjectiveManager.Instance is null!");
        }
    }

    void ShowSwordExplanation()
    {
        optionButtons.SetActive(false); // Hide the options panel
        dialogueUI.SetActive(true); // Show the dialogue UI
        dialogueText.text = swordExplanation; // Display the sword explanation

        // Wait for player input to end dialogue
        awaitingPlayerInput = true;
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
