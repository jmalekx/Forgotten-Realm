using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.MainActions input;
    CharacterController controller;
    Animator animator;


    [Header("UI")]
    public TMP_Text DeathText;
    public Slider sprintSlider;
    public Slider HealthBar;
    public float HealthDecreaseSpeed;
   


    [Header("Moving")]
    private float moveSpeed;
    public float groundDrag;
    public float walkSpeed;

    public Camera cam;

    [Header("Sprinting")]
    public float sprintSpeed;
    public float sprintDuration;
    public float sprintCooldown;
    public float regenDelay;
    private float sprintTimer;
    private bool isCooldown;
    private bool isSprinting;
    private float regenTimer;
    private bool isSprintRegen;
    public float sprintRegenMultiplier;
    public float SprintDecreaseSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Detection")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform playerBody;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Vector2 movementInput;
    Rigidbody rb;

    [Header("Attacking")]
    public float attackDistance = 7f;

    //[Header("Crafting")]
    //public GameObject craftingPanel;

    [Header("Crafting")]
    public GameObject craftingPanel;
    public Transform inventoryPanel; // Reference to the inventory panel
    public Transform craftingSlotsParent; // Parent transform of crafting slots

    private List<Transform> craftingSlots = new List<Transform>();



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
        readyToJump = true;
        isCooldown = false;
        isSprinting = false;
        sprintTimer = sprintDuration;
        sprintSlider.maxValue = sprintDuration;
        sprintSlider.value = sprintDuration;
        HealthBar.value = 100;
        HealthDecreaseSpeed = MainMenu.ChosenHealthDecreaseSpeed;
        SprintDecreaseSpeed = MainMenu.ChosensSprintDecreaseSpeed;
        sprintRegenMultiplier = MainMenu.ChosensSprintRegenSpeed;

        // Set crafting panel to be inactive by default
        if (craftingPanel != null)
        {
            craftingPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Crafting panel is not set in the inspector.");
        }
    }
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();


        playerInput = new PlayerInput();
        input = playerInput.Main;
        input.Enable();

        AssignInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void AssignInputs()
    {
        input.Jump.performed += ctx => JumpAttempt();
        input.Attack.started += ctx => Attack();
        input.Sprint.performed += ctx => SprintStart();
        input.Sprint.canceled += ctx => SprintStop();
        input.ToggleCrafting.performed += ToggleCraftingPanel; // Assign the toggle crafting action
    }

    // public void easy(){
    //     HealthDecreaseSpeed = 0.5f;
    // }
    // public void medium(){
    //     HealthDecreaseSpeed = 1f;

    // }

    // public void hard(){
    //     HealthDecreaseSpeed = 4f; 
    // }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground); //checking if ground
        if (HealthBar.value > 0)
        {
            HealthBar.value -= Time.deltaTime * HealthDecreaseSpeed;
        }
        if (HealthBar.value <= 0)
        {
            // string displayText = "GAME OVER";
            //  DeathText.text  = displayText;
            //  enabled = false;
            SceneManager.LoadScene("GameOverScene");
        }
        GetInput();
        ControlSpeed();
        UpdateSprintUI();
        HandleSprint();
        HandleSprintRegen();

        if (input.Attack.IsPressed())
        { Attack(); }

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void GetInput()
    {
        movementInput = input.Move.ReadValue<Vector2>();
    }

    //-----------------------MOVING
    void MovePlayer()
    {
        moveDirection = playerBody.forward * movementInput.y + playerBody.right * movementInput.x; //walk in direction youre looking

        if (OnSlope()) //when on slope
            rb.AddForce(GetSlopeDirection() * moveSpeed * 2f, ForceMode.Force);

        if (grounded) //when player on ground
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded) //when player in air
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * moveSpeed; // if faster than set movespeed, calculate max velocity and apply it
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    //-----------------------JUMPING
    void JumpAttempt()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //resetting y so you can jump exact same height each time
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //impulse cos only applying force once
    }
    void ResetJump()
    {
        readyToJump = true;
    }


    //-----------------------SPRINTING
    void SprintStop()
    {
        Debug.Log("sprint released");
        isSprinting = false;
        moveSpeed = walkSpeed;

        if (!isCooldown)
        {
            regenTimer = sprintCooldown;
            isSprintRegen = false;
        }
    }
    void SprintStart()
    {
        Debug.Log("sprint pressed");
        bool isMoving = movementInput.x != 0 || movementInput.y != 0;//so dont lose sprint when pressed but not moving
        if (grounded && !isCooldown && isMoving)
        {
            isSprinting = true;
            isSprintRegen = false;
        }
    }
    void HandleSprint()
    {
        if (isSprinting && sprintTimer > 0 && !isCooldown)
        {
            moveSpeed = sprintSpeed;
            sprintTimer -= Time.deltaTime * SprintDecreaseSpeed;

            if (sprintTimer <= 0)
            {
                Debug.Log("sprint expired, start cooldown");
                isSprinting = false;
                isCooldown = true;
                moveSpeed = walkSpeed;
                Invoke(nameof(ResetCooldown), sprintCooldown);
            }
        }

        if (!isSprinting && !isCooldown && sprintTimer < sprintDuration && regenTimer <= 0 && !isSprintRegen)
        {
            isSprintRegen = true;
        }
        if (!isSprinting && !isCooldown)
        {
            moveSpeed = walkSpeed;
        }
    }
    void HandleSprintRegen()
    {
        if (!isSprinting && !isCooldown)
        {
            if (regenTimer > 0)
            {
                regenTimer -= Time.deltaTime * sprintRegenMultiplier;
            }
            else if (isSprintRegen)
            {
                sprintTimer += (sprintDuration / sprintCooldown) * Time.deltaTime; //smooth gradual bar

                if (sprintTimer >= sprintDuration)
                {
                    sprintTimer = sprintDuration;
                    isSprintRegen = false;
                }
            }
        }
    }

    void ResetCooldown()
    {
        Debug.Log("sprint reset");
        isCooldown = false;
        isSprintRegen = true;
    }
    void UpdateSprintUI()
    {
        sprintSlider.value = sprintTimer;
    }

    void Attack()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                GameObject enemyHit = hit.collider.gameObject;

                UnityEngine.AI.NavMeshAgent enemyNavAgent = enemyHit.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (enemyNavAgent != null)
                {
                    enemyNavAgent.isStopped = true; // Disable movement
                }
                Animator enemyAnimator = enemyHit.GetComponent<Animator>();
                if (enemyAnimator != null)
                {
                    enemyAnimator.SetTrigger("Death");
                    ObjectiveManager.Instance.TrackObjective("Fight off an enemy");
                }

                StartCoroutine(Destroyed(enemyHit, 5f));
            }

        }

    }
    IEnumerator Destroyed(GameObject TargetedEnemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(TargetedEnemy);
    }


    //-----------------------TOGGLE CRAFTING PANEL
    void ToggleCraftingPanel(InputAction.CallbackContext context)
    {
        if (craftingPanel != null)
        {
            bool isActive = craftingPanel.activeSelf;
            craftingPanel.SetActive(!isActive);
            SetCursorState(!isActive);
            Debug.Log($"Crafting panel is now {(craftingPanel.activeSelf ? "active" : "inactive")}.");
        }
        else
        {
            Debug.LogError("Crafting panel is not assigned in the inspector.");
        }
    }

    void SetCursorState(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    //-----------------------OBJECTIVE COLLIDER
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Village"))
        {
            ObjectiveManager.Instance.TrackObjective("Locate a village");
        }
    }
}









