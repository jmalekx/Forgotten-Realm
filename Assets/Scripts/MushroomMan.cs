using TMPro;
using UnityEngine;

public class MushroomMan : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float moveDistance; //max distance to move per move
    public float minRange; //range to move
    public float maxRange;

    [Header("Ground detection")]
    public LayerMask ground;
    public float objectHeight;

    [Header("Dialogue")]
    public GameObject player;  //player detection
    public float dialogueRange = 5f;  //distance to trigger
    public string dialogue;  //message
    public TextMeshProUGUI dialogueText;
    public float dialogueDuration;
    private float dialogueTimer;
    private bool dialogueActivated = false;

    [Header("Target Position")]
    public float targetX; 
    public float targetZ;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool moveToVillage = false;
    private float stopDuration;
    private float stopTimer;
    private bool isNearPlayer = false;  //track if player nearby

    void Start()
    {
        StopAndWait();
        dialogueText.gameObject.SetActive(false); //disable dialogue text
    }

    void Update()
    {
        CheckPlayerDistance(); //check if the player nearby

        if (isNearPlayer && !dialogueActivated)
        {
            //pause and do dialogue if player nearby
            SaySomething();
        }
        else if (!dialogueActivated)
        {
            //normal random movement if the player is not nearby
            if (isMoving)
            {
                MoveTowardsTarget();
            }
            else if (!moveToVillage)
            {
                stopTimer -= Time.deltaTime;
                if (stopTimer <= 0f)
                {
                    ChooseNewTargetPosition();
                }
            }
        }

        if (dialogueTimer > 0)
        {
            dialogueTimer -= Time.deltaTime;
            if (dialogueTimer <= 0)
            {
                HideDialogue();
                MoveToVillage();
            }
        }

        if (moveToVillage && isMoving)
        {
            MoveTowardsTarget();
        }
    }
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= dialogueRange)
        {
            isNearPlayer = true;  //pause movement if player near
        }
        else
        {
            isNearPlayer = false;
        }
    }
    void SaySomething()
    {
        dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueTimer = dialogueDuration;

        dialogueActivated = true;
    }
    void HideDialogue()
    {
        dialogueText.gameObject.SetActive(false);
    }
    void MoveToVillage()
    {
        // Set the target X and Z, leaving Y to be determined by the raycast
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);

        // Raycast from above the target position to detect the ground height
        Vector3 raycastOrigin = new Vector3(targetX, transform.position.y + 50f, targetZ); // Start raycast 50 units above current y position

        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            // Set targetPosition.y to the terrain height at the target location, plus a small objectHeight offset
            targetPosition.y = hit.point.y + objectHeight;
            Debug.Log("Raycast hit terrain at height: " + hit.point.y + ", setting target Y to: " + targetPosition.y);
        }
        else
        {
            // Fallback: Keep current y if the raycast fails (prevent object sinking into ground)
            targetPosition.y = transform.position.y;
            Debug.LogWarning("Raycast did not hit terrain, maintaining current Y position: " + targetPosition.y);
        }

        // Start moving towards the village
        isMoving = true;
        moveToVillage = true;

        // Visualize the raycast for debugging
        Debug.DrawRay(raycastOrigin, Vector3.down * 100f, Color.red, 5f); // Display for 5 seconds
    }

    void ChooseNewTargetPosition()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-moveDistance, moveDistance),
            0,
            Random.Range(-moveDistance, moveDistance)
        );

        targetPosition = transform.position + randomDirection;

        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            targetPosition = hit.point + Vector3.up * objectHeight; //ensure the object stays above the terrain
            isMoving = true;
        }
        else
        {
            //if target invalid, choose another pos
            ChooseNewTargetPosition();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) //check if reached target
        {
            isMoving = false;
            StopAndWait();
            if (moveToVillage)
            {
                moveToVillage = false;
            }
            else
            {
                StopAndWait();
            }
        }
    }

    void StopAndWait()
    {
        stopDuration = Random.Range(minRange, maxRange);
        stopTimer = stopDuration;
        isMoving = false;
    }
}
