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
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            targetPosition = hit.point + Vector3.up * objectHeight;
            isMoving = true;
        }
        else
        {
            // If raycast fails, set isMoving to false
            isMoving = false;
        }

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
        Vector3 targetXZ = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, targetXZ, moveSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            //set the object Y pos to follow the terrain
            transform.position = new Vector3(transform.position.x, hit.point.y + objectHeight, transform.position.z);
        }

        //check if reached
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
}
