using UnityEngine;
using TMPro;

public class VillagerDialogueController : MonoBehaviour
{
    // Reference to the TextMeshPro Text component for dialogue
    public TextMeshProUGUI dialogueText;
    // Reference to the player GameObject
    public GameObject player;
    // Distance to trigger dialogue
    public float dialogueRange = 5f;

    // A list of dialogue lines that the blacksmith will say
    public string[] dialogueLines;
    private int currentDialogueIndex = 0;

    private bool isPlayerNear = false;
    private bool isDialogueActive = false;

    void Start()
    {
        // Ensure the dialogue text is hidden at the start
        dialogueText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check the distance between the player and the villager
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is within range, show dialogue
        if (distanceToPlayer <= dialogueRange)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
            }

            // Wait for player to press a key (e.g., "E") to start dialogue
            if (!isDialogueActive && Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                EndDialogue();
            }
        }
    }

    // Start the dialogue (show the first line)
    void StartDialogue()
    {
        isDialogueActive = true;
        currentDialogueIndex = 0;
        ShowDialogue(dialogueLines[currentDialogueIndex]);
    }

    // Show the dialogue text on screen
    void ShowDialogue(string message)
    {
        dialogueText.text = message; // Set the dialogue text
        dialogueText.gameObject.SetActive(true); // Make it visible
    }

    // End the dialogue (hide the text)
    void EndDialogue()
    {
        dialogueText.gameObject.SetActive(false); // Hide the dialogue text
        isDialogueActive = false; // Reset the flag
    }

    // Function to go to the next line of dialogue
    public void NextDialogue()
    {
        // Check if there are more lines to show
        if (currentDialogueIndex < dialogueLines.Length - 1)
        {
            currentDialogueIndex++;
            ShowDialogue(dialogueLines[currentDialogueIndex]);
        }
        else
        {
            EndDialogue(); // If no more lines, end dialogue
        }
    }
}
