using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        StopAndWait();
        dialogueText.gameObject.SetActive(false); //disable dialogue text
    }

    void FixedUpdate() //for consistent physics calculations
    {
        CheckPlayerDistance();

        if (isNearPlayer && !dialogueActivated)
        {
            //pause and do dialogue if the player nearby
            SaySomething();
        }
        else if (!dialogueActivated)
        {
            //normal random movement if the player not nearby
            if (isMoving)
            {
                MoveTowardsTarget();
            }
            else if (!moveToVillage)
            {
                stopTimer -= Time.fixedDeltaTime;
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
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
        SetTargetPositionToGround();
        moveToVillage = true;
    }

    void ChooseNewTargetPosition()
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-moveDistance, moveDistance),
            0,
            Random.Range(-moveDistance, moveDistance)
        );

        targetPosition = transform.position + randomDirection;
        SetTargetPositionToGround();
    }

    void SetTargetPositionToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            targetPosition = hit.point + Vector3.up * objectHeight; //ensure object stays above the terrain
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 targetXZ = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        Vector3 direction = (targetXZ - transform.position).normalized;
        characterController.Move(direction * moveSpeed * Time.fixedDeltaTime);

        //raycast down to adjust Y position based on terrain
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            //set object y position based on the terrain height
            transform.position = new Vector3(transform.position.x, hit.point.y + objectHeight, transform.position.z);
        }

        //check if reached target
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.1f)
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //only react to collisions with objects that are not ground
        if ((ground & (1 << hit.collider.gameObject.layer)) == 0)
        {
            //currently moving to village
            if (moveToVillage)
            {
                MoveToVillage(); //re-route to village
            }
            else
            {
                ChooseNewTargetPosition(); //choose new position
            }
        }
    }
}
