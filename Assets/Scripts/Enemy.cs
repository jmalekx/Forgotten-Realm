using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{
    [Header("Movement")]
    public float EnemySpeed = 2f;
    public float interval = 1.5f; 
    private Vector3 RandomDirection;     
    private float timePassed; 
    [Header("Ground detection")]
    public LayerMask ground;
    public float objectHeight; 

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Set initial random direction in the X plane
        SetRandomDirection();
    }

    void Update()
    {
       // if(canMove(RandomDirection)){

      //  transform.Translate(new Vector3(RandomDirection.x, 0f, RandomDirection.z) * EnemySpeed * Time.deltaTime);
        characterController.Move(new Vector3(RandomDirection.x, 0f, RandomDirection.z) * EnemySpeed * Time.fixedDeltaTime);
        timePassed += Time.deltaTime;
        if (timePassed >= interval)
        {
            SetRandomDirection();
            timePassed = 0;  // Reset timer
        }
        
     //   else{
    //        SetRandomDirection();
      //  }
        SetTargetPositionToGround();

    }

    // Function to set a new random direction (X plane only)
    private void SetRandomDirection()
    {
        RandomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }
  //  private bool canMove(Vector3 direction)
  //  {
        // Cast a ray in the direction of movement
     //   RaycastHit hit;
     //   if (Physics.Raycast(transform.position, direction, out hit, rayDistance))
     //   {
            // If we hit something, we cannot move in that direction
     //       return false;
    //    }
    //    return true; // No obstacles detected, we can move
 //   }
    
    void SetTargetPositionToGround()
    {
        RaycastHit hit;
        // Cast a ray downwards from the NPC's position to detect the ground
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            // Adjust the NPC's position Y based on the ground height plus objectHeight
           // transform.position = new Vector3(transform.position.x, hit.point.y + objectHeight, transform.position.z);
            float newYPosition = hit.point.y + objectHeight;

            // Use characterController.Move to adjust the NPC's Y position while retaining its XZ position
            Vector3 verticalMove = new Vector3(0f, newYPosition - transform.position.y, 0f);
            characterController.Move(verticalMove);  // Only adjust Y position
          //  characterController.Move(new Vector3(transform.position.x, hit.point.y + objectHeight, transform.position.z));
        }
    }

        //RaycastHit hit;
        //if (Physics.Raycast(RandomDirection + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        //{
            // If we hit something, we cannot move in that direction
           // RandomDirection = hit.point + Vector3.up * objectHeight; //ensure object stays above the terrain
          //  return true;
       // }
      //  else
      //  {
        //    return false;
       // }

}
