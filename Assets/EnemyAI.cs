using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform playerVariable;
    public float maxDistance = 1.0f;
    public float EnemyDetectionDistance = 5.0f;
    NavMeshAgent NavAgent;
    Animator animator;
    bool AttackingState = false;

    // Start is called before the first frame update
    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       // float distance = (playerVariable.position - NavAgent.destination).magnitude;
        float detectionDistance = Vector3.Distance(playerVariable.position, transform.position);

        if(detectionDistance < EnemyDetectionDistance){

            if(detectionDistance > maxDistance){
                NavAgent.destination = playerVariable.position;    
                animator.SetFloat("Speed", NavAgent.velocity.magnitude);
                AttackingState = false;
            }
            
            else if (!AttackingState){
                NavAgent.destination = transform.position;  // Stop moving
                animator.SetTrigger("Attack");
                AttackingState = true;
            }
        }
        else{
            NavAgent.destination = transform.position;
            animator.SetFloat("Speed", 0);
            AttackingState = false; 
        }
    
    }
}
