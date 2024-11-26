using TMPro;
using UnityEngine;

public class MushroomMan : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float moveDistance; //max distance to move per move
    public float noiseScale = 0.1f; //scale of Perlin noise for movement
    public float minRange; //range to move
    public float maxRange;

    [Header("Ground detection")]
    public LayerMask ground;
    public float objectHeight;

    [Header("Animation")]
    [SerializeField]
    private Animator _animator;

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

    //variables for perlin noise
    private float noiseOffsetX;
    private float noiseOffsetZ;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        StopAndWait();
        dialogueText.gameObject.SetActive(false); //disable dialogue text

        //initialize perlin noise offsets to random values
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetZ = Random.Range(0f, 100f);
    }

    void FixedUpdate() //for consistent physics calculations
    {
        CheckPlayerDistance();

        if (isNearPlayer && !dialogueActivated)
        {
            _animator.SetBool("isReact", true);
            //pause and do dialogue if the player nearby
            ObjectiveManager.Instance.TrackObjective("Locate the mushroom man");
            SaySomething();
        }
        else if (!dialogueActivated)
        {
            _animator.SetBool("isReact", false);
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
        isNearPlayer = distanceToPlayer <= dialogueRange;
    }
    void SaySomething()
    {
        _animator.SetBool("isMoving", false);
        dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueTimer = dialogueDuration;

        dialogueActivated = true;
    }
    void HideDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        _animator.SetBool("isReact", false);
    }
    void MoveToVillage()
    {
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
        SetTargetPositionToGround();
        moveToVillage = true;
    }

    void ChooseNewTargetPosition()
    {
        float offsetX = (Mathf.PerlinNoise(Time.time * noiseScale + noiseOffsetX, 0) - 0.5f) * 2.0f;
        float offsetZ = (Mathf.PerlinNoise(0, Time.time * noiseScale + noiseOffsetZ) - 0.5f) * 2.0f;

        Vector3 randomDirection = new Vector3(offsetX, 0, offsetZ) * moveDistance;
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
            _animator.SetBool("isMoving", true);
        }
        else
        {
            isMoving = false;
            _animator.SetBool("isMoving", false);
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 targetXZ = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        Vector3 direction = (targetXZ - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetXZ);

        //rotation towards target for more natural movement
        if (direction != Vector3.zero) //ensure direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f); //5f=rotation speed
        }

        if (distanceToTarget > 0.1f)
        {
            Vector3 movement = direction * moveSpeed * Time.fixedDeltaTime;
            //custom gravity to keep the character grounded
            if (!characterController.isGrounded)
            {
                movement.y -= 9.81f * Time.fixedDeltaTime;
            }
            characterController.Move(movement);
        }
        else
        {
            isMoving = false;
            _animator.SetBool("isMoving", false);
            StopAndWait();

            if (moveToVillage)
            {
                moveToVillage = false;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + objectHeight, transform.position.z);
        }
    }



    void StopAndWait()
    {
        stopDuration = Random.Range(minRange, maxRange);
        stopTimer = stopDuration;
        isMoving = false;
        _animator.SetBool("isMoving", false);
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
