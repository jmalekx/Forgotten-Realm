using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMovement : MonoBehaviour
{
    public float EnemySpeed = 2f;
    public float interval = 1.5f; 
    private Vector3 RandomDirection;     
    private float timePassed;          

    void Start()
    {
        // Set initial random direction in the X plane
        SetRandomDirection();
    }

    void Update()
    {
        transform.Translate(new Vector3(RandomDirection.x, 0f, RandomDirection.z) * EnemySpeed * Time.deltaTime);
        timePassed += Time.deltaTime;
        if (timePassed >= interval)
        {
            SetRandomDirection();
            timePassed = 0;  // Reset timer
        }
    }

    // Function to set a new random direction (X plane only)
    private void SetRandomDirection()
    {
        RandomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

}
