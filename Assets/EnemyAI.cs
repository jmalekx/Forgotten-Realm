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
    public float attackCooldown = 2f; 
    private float timeSinceLastAttack = 0f;
    private int EnemyHealthPoints = 3;
    private bool EnemyAlive = true;
    //private int currentHits = 0; // Track the current hit count
    NavMeshAgent NavAgent;
    Animator animator;
    bool AttackingState = false;
    private Slider playerHealthBar; 
    public Slider EnemyHealthBar; 

    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealthBar = playerVariable.GetComponent<PlayerController>().HealthBar;
        enemyAttackAmount = MainMenu.ChosenenemyAttackAmount;
        EnemyHealthBar.value = EnemyHealthPoints; // Set the current value
    }


    void Update()
    {
        if (!EnemyAlive) return;
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

    public void TakeHit()
    {
        if (!EnemyAlive) return;
        EnemyHealthPoints--;
        EnemyHealthBar.value = EnemyHealthPoints;

        if (EnemyHealthPoints <= 0) // Check if the enemy has been hit enough times
        {
            EnemyAlive = false;
            StartCoroutine(Destroyed());
        }
        //healthslider
    }

    IEnumerator Destroyed()
    {
        animator.Play("Death", 0, 0f); // Play death animation
        ObjectiveManager.Instance.TrackObjective("Fight off an enemy");
        NavAgent.isStopped = true; // Stop enemy movement
        yield return new WaitForSeconds(5f); // Wait for the animation or any delay
        Destroy(gameObject); // Destroy the enemy
    }

    
}

