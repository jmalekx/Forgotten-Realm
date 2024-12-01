using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform playerVariable;
    public float maxDistance = 2f;
    public float EnemyDetectionDistance = 20.0f;
    public float enemyAttackAmount;
    public float attackCooldown = 1f; 
    private float timeSinceLastAttack = 0f;
    NavMeshAgent NavAgent;
    Animator animator;
    bool AttackingState = false;
    private Slider playerHealthBar; 

    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealthBar = playerVariable.GetComponent<PlayerController>().HealthBar;
        enemyAttackAmount = MainMenu.ChosenenemyAttackAmount;
    }


    void Update()
    {
        float detectionDistance = Vector3.Distance(playerVariable.position, transform.position);

        if(detectionDistance < EnemyDetectionDistance){

            if(detectionDistance > maxDistance){
                NavAgent.destination = playerVariable.position;    
                animator.SetFloat("Speed", NavAgent.velocity.magnitude);
                AttackingState = false;
            }
            
            else if (!AttackingState){
                NavAgent.destination = transform.position;  // Stop moving
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Attack");
                AttackingState = true;
                playerHealthBar.value -= enemyAttackAmount;
                timeSinceLastAttack = attackCooldown;
            }
        }
        else{
            NavAgent.destination = transform.position; 
            animator.SetFloat("Speed", 0);
            AttackingState = false; 
        }
        if (timeSinceLastAttack > 0)
        {
            timeSinceLastAttack -= Time.deltaTime; 
        }
        else
        {
            AttackingState = false;  
        }
    
    }

    
}

