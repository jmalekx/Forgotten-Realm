using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.MainActions input;
    CharacterController controller;
    Animator animator;

   // [Header("Keybinds")]
  //  public KeyCode jump = KeyCode.Space;
   // public KeyCode sprint = KeyCode.LeftShift;
  //  public KeyCode attack = KeyCode.Mouse0;

    [Header("UI")]
    public Slider sprintSlider;


    [Header("Moving")]
    private float moveSpeed;
    public float groundDrag;
    public float walkSpeed;
    [Header("Camera")]
    public Camera cam;
    [Header("Sprinting")]
    public float sprintSpeed;
    public float sprintDuration;
    public float sprintCooldown;
    private float sprintTimer;
    private bool isCooldown;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Detection")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    public Transform playerBody;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public Moving state;
    public enum Moving
    {
        walk,
        sprint,
        air
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        isCooldown = false;
        sprintTimer = sprintDuration;
        sprintSlider.maxValue = sprintDuration;
        sprintSlider.value = sprintDuration;
        
    }
    void Awake()
    { 
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        

        playerInput = new PlayerInput();
        input = playerInput.Main;
        AssignInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //resetting y so you can jump exact same height each time
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //impulse cos only applying force once
    }
    void JumpAttempt(){
        if (readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    void ResetJump()
    {
        readyToJump = true;
    }

    void AssignInputs(){
        input.Jump.performed += ctx => JumpAttempt();
        input.Attack.started += ctx => Attack();
        input.Sprint.performed += ctx => SprintStart();
       
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground); //checking if ground
     
        KeyboardInputs();
        ControlSpeed();
        MoveState();
        UpdateSprintUI();
        if(input.Attack.IsPressed())
        { Attack();}

        

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    void KeyboardInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");



    }
    //-----------------------MOVING
    void MovePlayer()
    {
        moveDirection = playerBody.forward * verticalInput + playerBody.right * horizontalInput; //walk in direction youre looking

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


    //-----------------------JUMPING


    //-----------------------SPRINTING
    void MoveState()
    {

        //walk
        if (grounded)
        {
            state = Moving.walk;
            moveSpeed = walkSpeed;
        }
        //air
        else
        {
            state = Moving.air;
        }
        
    }
    void SprintStart(){
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        //sprint
        if (grounded && !isCooldown && isMoving)
        {
            if (sprintTimer > 0)
            {
                state = Moving.sprint;
                moveSpeed = sprintSpeed;
                sprintTimer -= Time.deltaTime;
                Debug.Log("Sprinting! Timer: " + sprintTimer);
            }
            else
            {
                isCooldown = true;
                Invoke(nameof(ResetCooldown), sprintCooldown);
                state = Moving.walk;
                moveSpeed = walkSpeed;
                Debug.Log("Sprint on cooldown!");
            }
        }
    }
    void ResetCooldown()
    {
        isCooldown = false;
        sprintTimer = sprintDuration;
    }
    void UpdateSprintUI()
    {
        if (!isCooldown)
        {
            sprintSlider.value = sprintTimer;
        }
        else
        {
            float refillSpeed = sprintDuration / sprintCooldown; //bar refilling based on cooldown
            sprintSlider.value += refillSpeed * Time.deltaTime;
           
            if (sprintSlider.value >= sprintDuration) //prevent overfill
            {
                sprintSlider.value = sprintDuration;
            }
        }
    }





    
    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
  

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void Attack()
    {
        if(!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);


        if(attackCount == 0)
        {
            
            attackCount++;
        }
        else
        {
           
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        { 
            HitTarget(hit.point);

        } 
    }

    void HitTarget(Vector3 pos)
    {

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}

