using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Keybinds")]
    public KeyCode jump = KeyCode.Space;
    public KeyCode sprint = KeyCode.LeftShift;

    [Header("Moving")]
    private float moveSpeed;
    public float groundDrag;
    public float walkSpeed;

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

    public isMoving state;
    public enum isMoving
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
    }
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground); //checking if ground
        KeyboardInputs();
        ControlSpeed();
        MoveState();

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

        if (Input.GetKey(jump) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

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

    void Jump()
    {
        //resetting y so you can jump exact same height each time
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //impulse cos only applying force once
    }
    void ResetJump()
    {
        readyToJump = true;
    }

    void MoveState()
    {
        //sprint
        if (grounded && Input.GetKey(sprint) && !isCooldown)
        {
            if (sprintTimer > 0)
            {
                state = isMoving.sprint;
                moveSpeed = sprintSpeed;
                sprintTimer -= Time.deltaTime;
                Debug.Log("Sprinting! Timer: " + sprintTimer);
            }
            else
            {
                isCooldown = true;
                Invoke(nameof(ResetCooldown), sprintCooldown);
                state = isMoving.walk;
                moveSpeed = walkSpeed;
                Debug.Log("Sprint on cooldown!");
            }
        }
        //walk
        else if (grounded)
        {
            state = isMoving.walk;
            moveSpeed = walkSpeed;
        }
        //air
        else
        {
            state = isMoving.air;
        }
        
    }

    void ResetCooldown()
    {
        isCooldown = false;
        sprintTimer = sprintDuration;
    }
}
