using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepAudio : MonoBehaviour
{
    public LayerMask groundMask;
    public float rayDistance = 2f; //detect ground
    public float stepInterval = 0.56f; //time between step/clip

    [Header("Footstep Sounds")]
    public List<AudioClip> woodFootsteps;
    public List<AudioClip> stoneFootsteps;
    public List<AudioClip> waterFootsteps;
    public List<AudioClip> swimFootsteps;
    public List<AudioClip> defaultFootsteps;
    public AudioClip jumpSound;

    private AudioSource audioSource;
    private float stepTimer;
    private bool isPlayerMoving = false;
    private bool wasGroundedLastFrame = true;
    private float footstepVolume = 0.5f; //default vol
    private string currentSurfaceTag = "DefaultSound";

    private bool isJumping = false;
    private bool hasLanded = false;

    private PlayerController playerController;
    private Dictionary<string, List<AudioClip>> surfaceFootsteps;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        stepTimer = stepInterval;

        surfaceFootsteps = new Dictionary<string, List<AudioClip>>()
        {
            { "WoodSound", woodFootsteps },
            { "StoneSound", stoneFootsteps },
            { "WaterSound", waterFootsteps },
            { "SwimSound", swimFootsteps },
            { "DefaultSound", defaultFootsteps }
        };
    }

    void Update()
    {
        bool wasPlayerMoving = isPlayerMoving;

        //check if moving
        isPlayerMoving = IsPlayerMoving() && IsGrounded();


        //detect if the player jumps
        if (wasGroundedLastFrame && !IsGrounded())
        {
            //player just jumped
            isJumping = true;
            hasLanded = false;
        }
        //detect if the player lands
        else if (!wasGroundedLastFrame && IsGrounded() && isJumping)
        {
            //play the jump sound when landing
            if (!hasLanded)
            {
                PlayJumpSound();
                hasLanded = true; //mark as landed
                isJumping = false; //reset jump state
            }
        }

        wasGroundedLastFrame = IsGrounded(); //update to current ground status


        //check if sprinting
        if (playerController != null && playerController.IsSprinting)
        {
            stepInterval = 0.23f;  //speed up footsteps
        }
        else
        {
            stepInterval = 0.56f;   //default
        }


        if (isPlayerMoving && !isJumping)
        {
            stepTimer -= Time.deltaTime; //coutndown until next step audio

            //play next
            if (stepTimer <= 0f)
            {
                PlayFootstepSound();
                stepTimer = stepInterval; //reset timer
            }
        }
        else
        {
            //player not moving
            stepTimer = stepInterval; //reset timer
        }

        //stop auido on player stop
        if (!isPlayerMoving && audioSource.isPlaying && !isJumping)
        {
            audioSource.Stop();
        }

        //check if player exited valid trigger
        if (isPlayerMoving && currentSurfaceTag == "DefaultSound" && !IsInAnySurface())
        {
            currentSurfaceTag = "DefaultSound";  //reset to default if no valid surface found
        }
    }
    private bool IsInAnySurface()
    {
        //check if in valid tiregger
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rayDistance, groundMask);
        foreach (Collider col in hitColliders)
        {
            if (surfaceFootsteps.ContainsKey(col.tag))
            {
                return true; //if hit any surface with a valid tag
            }
        }
        return false; //no valid surface found
    }

    private bool IsPlayerMoving()
    {
        //check if moving
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private bool IsGrounded()
    {
        // if grounded
        bool grounded = Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask);
        if (grounded)
        {
            Debug.Log("Grounded on: " + currentSurfaceTag);
        }
        return grounded;
    }

    private void PlayFootstepSound()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance, groundMask))
        {
            AudioClip clipToPlay = GetFootstepClip(currentSurfaceTag);
            if (clipToPlay != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1f);
                audioSource.PlayOneShot(clipToPlay, footstepVolume);
            }
        }
    }
    private void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound, footstepVolume); //play jump sound when player jumps
        }
    }

    private AudioClip GetFootstepClip(string tag)
    {
        if (surfaceFootsteps.ContainsKey(tag))
        {
            return GetRandomFootstepClip(surfaceFootsteps[tag]);
        }
        //default
        return GetRandomFootstepClip(surfaceFootsteps["DefaultSound"]);
    }

    private AudioClip GetRandomFootstepClip(List<AudioClip> footstepList)
    {
        if (footstepList.Count > 0)
        {
            return footstepList[Random.Range(0, footstepList.Count)];
        }
        return null;
    }
    private void OnTriggerEnter(Collider other)
    {
        //update surface n trigger using dict map
        if (surfaceFootsteps.ContainsKey(other.tag))
        {
            currentSurfaceTag = other.tag;
        }
    }

    //reset
    private void OnTriggerExit(Collider other)
    {
        //reset
        if (surfaceFootsteps.ContainsKey(other.tag))
        {
            currentSurfaceTag = "DefaultSound";
        }
    }
}
