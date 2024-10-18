using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed;
    public float moveDistance; //max distance to move per move
    public LayerMask ground;
    public float objectHeight;
    public float minRange;
    public float maxRange;

    public GameObject player;  // Reference to the player
    public float dialogueRange = 5f;  // Distance at which the player triggers dialogue
    public string dialogue = "Hello, Player!";  // Dialogue message

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float stopDuration;
    private float stopTimer;
    private bool isNearPlayer = false;  // Track if player is nearby

    void Start()
    {
        StopAndWait();
    }

    void Update()
    {
        CheckPlayerDistance();  // Check if the player is nearby

        if (isNearPlayer)
        {
            // Pause movement and initiate dialogue when the player is nearby
            SaySomething();
        }
        else
        {
            // Normal random movement if the player is not nearby
            if (isMoving)
            {
                MoveTowardsTarget();
            }
            else
            {
                stopTimer -= Time.deltaTime;
                if (stopTimer <= 0f)
                {
                    ChooseNewTargetPosition();
                }
            }
        }
    }

    // Check the distance between the object and the player
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= dialogueRange)
        {
            isNearPlayer = true;  // Player is within range, pause movement
        }
        else
        {
            isNearPlayer = false;  // Player is out of range, resume movement
        }
    }

    // Dialogue or action when the player is near
    void SaySomething()
    {
        // Log dialogue to the console (replace with UI or other dialogue system if needed)
        Debug.Log(dialogue);

        // Optionally, you could wait for a few seconds here before resuming movement
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
            targetPosition = hit.point + Vector3.up * objectHeight; // Ensure the object stays above the terrain
            isMoving = true;
        }
        else
        {
            // If invalid target, choose again
            ChooseNewTargetPosition();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // Check if reached target
        {
            isMoving = false;
            StopAndWait();
        }
    }

    void StopAndWait()
    {
        stopDuration = Random.Range(minRange, maxRange);
        stopTimer = stopDuration;
        isMoving = false;
    }
}
