using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlacksmithDialogue : MonoBehaviour
{
    public GameObject dialogueUI; // Panel for dialogue
    public TMP_Text Etext; // Prompt text
    public TMP_Text dialogueText; // Text for dialogue content
    public GameObject optionButtons; // Panel for options
    public Button option1Button; // First option button
    public Button option2Button; // Second option button
    public string interactKey = "i"; // Interaction key

    public GameObject daggerPrefab; // Dagger prefab
    public GameObject swordPrefab; // Sword prefab
    public Transform spawnPoint; // Item spawn point

    private bool playerInRange = false; // Is player in range
    private bool isInteracting = false; // Is interacting with NPC
    private int dialogueIndex = 0; // Current dialogue line index
    private bool awaitingPlayerInput = false; // Waiting for player input to end dialogue

    private string[] dialogueLines = {
        "Hello, welcome to my blacksmith!",
        "How can I help you today?"
    };

    private string daggerExplanation = "Here is your gem! Good luck on your quest!";
    private string swordExplanation = "Here is your sword! May it help you in battle!";

    void Update()
    {
        // Handles player interactions and dialogue progression
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (awaitingPlayerInput)
            {
                EndInteraction(); // End interaction
            }
            else if (!isInteracting)
            {
                StartInteraction();
            }
            else if (dialogueIndex < dialogueLines.Length)
            {
                ShowNextDialogue();
            }
            else
            {
                ShowOptions();
            }
        }
    }

    void StartInteraction()
    {
        // Starts interaction with the blacksmith
        isInteracting = true;
        dialogueUI.SetActive(true);
        Etext.text = "";
        ShowNextDialogue();
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true;
    }

    void ShowNextDialogue()
    {
        // Displays the next line of dialogue
        dialogueText.text = dialogueLines[dialogueIndex];
        dialogueIndex++;
    }

    void ShowOptions()
    {
        // Displays dialogue options to the player
        dialogueUI.SetActive(false);
        optionButtons.SetActive(true);
    }

    public void OnOption1Selected()
    {
        // Handles the selection of the first option (dagger)
        Debug.Log("Option 1 selected: I need tools");

        if (daggerPrefab != null && spawnPoint != null)
        {
            Instantiate(daggerPrefab, spawnPoint.position, spawnPoint.rotation); // Spawn dagger
            Debug.Log("Dagger spawned.");
        }
        else
        {
            Debug.LogWarning("Dagger prefab or spawn point not assigned!");
        }

        ShowDaggerExplanation();

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective("Collect gem from blacksmith");
        }
        else
        {
            Debug.LogError("ObjectiveManager is null!");
        }
    }

    void ShowDaggerExplanation()
    {
        // Displays explanation for selecting the dagger
        optionButtons.SetActive(false); 
        dialogueUI.SetActive(true); 
        dialogueText.text = daggerExplanation;
        awaitingPlayerInput = true; // Wait for input
    }

    public void OnOption2Selected()
    {
        // Handles the selection of the second option (sword)
        Debug.Log("Option 2 selected: I need a sword");

        if (swordPrefab != null && spawnPoint != null)
        {
            Instantiate(swordPrefab, spawnPoint.position, spawnPoint.rotation); // Spawn sword
            Debug.Log("Sword spawned.");
        }
        else
        {
            Debug.LogWarning("Sword prefab or spawn point not assigned!");
        }

        ShowSwordExplanation();

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.TrackObjective("Collect sword from blacksmith");
        }
        else
        {
            Debug.LogError("ObjectiveManager is null!");
        }
    }

    void ShowSwordExplanation()
    {
        // Displays explanation for selecting the sword
        optionButtons.SetActive(false);
        dialogueUI.SetActive(true);
        dialogueText.text = swordExplanation;
        awaitingPlayerInput = true;
    }

    void EndInteraction()
    {
        // Ends the current interaction
        ResetDialogue();
    }

    void ResetDialogue()
    {
        // Resets dialogue and interaction states
        dialogueUI.SetActive(false);
        optionButtons.SetActive(false);
        isInteracting = false;
        awaitingPlayerInput = false;
        dialogueIndex = 0;
        Etext.text = "Press I to interact with the villager";
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Detects when the player enters interaction range
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Etext.text = "Press I to interact with the villager";
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detects when the player exits interaction range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Etext.text = "";
            if (isInteracting || awaitingPlayerInput)
            {
                ResetDialogue();
            }
        }
    }
}
