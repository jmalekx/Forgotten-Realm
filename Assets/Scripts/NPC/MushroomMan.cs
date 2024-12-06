using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MushroomMan : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float moveDistance; //max distance to move per move
    public float noiseScale = 0.1f; //scale of Perlin noise for movement
    public float minWait;
    public float maxWait;

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

    [Header("Patrol Area")]
    public Vector3 patrolCenter;  //center
    public float patrolRadius = 10f;  //area radius

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool moveToVillage = false;
    private float stopDuration;
    private float stopTimer;
    private bool isNearPlayer = false;  //track if player nearby

    private NavMeshAgent navAgent;

    //variables for perlin noise
    private float noiseOffsetX;
    private float noiseOffsetZ;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = moveSpeed;
        StopAndWait();
        dialogueText.gameObject.SetActive(false); //disable dialogue text

        //initialize perlin noise offsets to random values
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetZ = Random.Range(0f, 100f);
    }

    void Update() //for consistent physics calculations
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
            if (isMoving && !moveToVillage)
            {
                CheckIfReachedDestination();
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
            CheckIfReachedDestination();
        }
    }

    void CheckPlayerDistance() //if player close, do reaction
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        isNearPlayer = distanceToPlayer <= dialogueRange;
    }

    void SaySomething() //display text
    {
        _animator.SetBool("isMoving", false);
        navAgent.isStopped = true;
        dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueTimer = dialogueDuration;

        dialogueActivated = true;
    }

    void HideDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        _animator.SetBool("isReact", false);
        navAgent.isStopped = false;
    }

    void MoveToVillage()
    {
        targetPosition = new Vector3(targetX, transform.position.y, targetZ);
        navAgent.SetDestination(targetPosition);
        moveToVillage = true;
        isMoving = true;
        _animator.SetBool("isMoving", true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; //to see visually
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
    }

    void ChooseNewTargetPosition()
    {
        for (int i = 0; i < 10; i++) //try up to 10 times to find a valid position
        {
            float offsetX = (Mathf.PerlinNoise(Time.time * noiseScale + noiseOffsetX, 0) - 0.5f) * 2.0f;
            float offsetZ = (Mathf.PerlinNoise(0, Time.time * noiseScale + noiseOffsetZ) - 0.5f) * 2.0f;

            Vector3 randomDirection = new Vector3(offsetX, 0, offsetZ) * moveDistance;
            Vector3 potentialPosition = transform.position + randomDirection;
     
            //check if within the circular patrol area
            if (Vector3.Distance(patrolCenter, potentialPosition) > patrolRadius)
                continue; // skip pos if outside
    

            //check if pos valid navmesh
            if (NavMesh.SamplePosition(potentialPosition, out NavMeshHit hit, moveDistance, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
                navAgent.SetDestination(targetPosition);
                isMoving = true;
                _animator.SetBool("isMoving", true);
                return;
            }
        }

        //if none valid
        StopAndWait();
    }

    void CheckIfReachedDestination()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            isMoving = false;
            _animator.SetBool("isMoving", false);

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
        stopDuration = Random.Range(minWait, maxWait);
        stopTimer = stopDuration;
        isMoving = false;
        _animator.SetBool("isMoving", false);

        int randomIdleIndex = GetWeightedRandomIdleIndex();
        _animator.SetInteger("IdleIndex", randomIdleIndex); 

        navAgent.ResetPath();

    }

    //weighted random
    int GetWeightedRandomIdleIndex()
    {
        int[] weights = { 50, 40, 10 }; //idle0 chance, idle1, idle2 respectively
        int randomValue = Random.Range(0, 100); //sum of weight

        //idle anim to reutrn based on weight
        if (randomValue < weights[0])
            return 0; 
        else if (randomValue < weights[0] + weights[1])
            return 1;
        else
            return 2; 
    }
}
