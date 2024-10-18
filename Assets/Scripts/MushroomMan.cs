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
    public string dialogue = "Hello, Player!";  //message
    public TextMeshProUGUI dialogueText;
    public float dialogueDuration;
    private float dialogueTimer;
    private bool dialogueActivated = false;

    private Vector3 targetPosition;
    private bool isMoving = false;
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
        else
        {
            //normal random movement if the player is not nearby
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

        if (dialogueTimer > 0)
        {
            dialogueTimer -= Time.deltaTime;
            if (dialogueTimer <= 0)
            {
                HideDialogue();
            }
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
        //to replace with a ui
        Debug.Log(dialogue);
        dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueTimer = dialogueDuration;

        dialogueActivated = true;
    }
    void HideDialogue()
    {
        dialogueText.gameObject.SetActive(false);
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
        }
    }

    void StopAndWait()
    {
        stopDuration = Random.Range(minRange, maxRange);
        stopTimer = stopDuration;
        isMoving = false;
    }
}
