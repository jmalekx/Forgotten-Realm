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
        SetRandomDirection();
    }

    void Update()
    {
        characterController.Move(new Vector3(RandomDirection.x, 0f, RandomDirection.z) * EnemySpeed * Time.fixedDeltaTime);
        timePassed += Time.deltaTime;
        if (timePassed >= interval)
        {
            SetRandomDirection();
            timePassed = 0;  
        }
        
        SetTargetPositionToGround();

    }

    private void SetRandomDirection()
    {
        RandomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    
    void SetTargetPositionToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, ground))
        {
            float newYPosition = hit.point.y + objectHeight;
            Vector3 verticalMove = new Vector3(0f, newYPosition - transform.position.y, 0f);
            characterController.Move(verticalMove); 
        }
    }


}
