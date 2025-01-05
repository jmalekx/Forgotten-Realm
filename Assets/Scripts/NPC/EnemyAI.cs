using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform playerVariable;
    public float maxDistance = 2f; //distance for enemy to attack
    public float EnemyDetectionDistance = 20.0f; //distance at which enemy detects player
    public float enemyAttackAmount; //amount of damage dealt to player. set in main menu
    public float attackCooldown = 2f; //cooldown between attacks so no overflow
    private float timeSinceLastAttack = 0f;
    private float EnemyHealthPoints = 3; //each enemy has a health of 3
    private bool EnemyAlive = true;
    NavMeshAgent NavAgent; //pathfinding for enemies AI
    Animator animator; 
    bool AttackingState = false;
    private Slider playerHealthBar; 
    public Slider EnemyHealthBar; 
    

    void Start()
    {
        //iniitalises navmeshagent and animator. sets the initial enemy health onto its UI healthbar and gets the players health bar from playercontroller script
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealthBar = playerVariable.GetComponent<PlayerController>().HealthBar;
        enemyAttackAmount = MainMenu.ChosenenemyAttackAmount;
        EnemyHealthBar.value = EnemyHealthPoints; // Set the current value
    }


    void Update()
    {
        if (!EnemyAlive) return; //don't do these actions whilst enemy is dead, avoids enemy still functioning whilst death animation is played out. 

        //distance calculated between the enemy and the player
        float detectionDistance = Vector3.Distance(playerVariable.position, transform.position);

        if(detectionDistance < EnemyDetectionDistance){
            //moves towards the player when in the detection distance but stops moving when in attack range.
            if(detectionDistance > maxDistance){
                NavAgent.destination = playerVariable.position;    
                animator.SetFloat("Speed", NavAgent.velocity.magnitude);
                AttackingState = false;
            }
            
            else if (!AttackingState){//attacks player if within attack range
                NavAgent.destination = transform.position;  // Stop moving
                animator.SetFloat("Speed", 0); //stop moving animation
                animator.SetTrigger("Attack");//attack animation
                AttackingState = true;
                playerHealthBar.value -= enemyAttackAmount; //players health is reduced
                timeSinceLastAttack = attackCooldown;
            }
        }
        else{//stops the enemy movement when the player is outside the detection distance again
            NavAgent.destination = transform.position; 
            animator.SetFloat("Speed", 0);
            AttackingState = false; 
        }
        if (timeSinceLastAttack > 0) //attack cooldown timer
        {
            timeSinceLastAttack -= Time.deltaTime; 
        }
        else
        {
            AttackingState = false; 
        }
    
    }

public void TakeHit(float damage)
{
    if (!EnemyAlive) return; //ignore the hits if enemy is dead

    //enemies health is decreased and updates its health bar aswell
    EnemyHealthPoints -= damage;
    EnemyHealthBar.value = EnemyHealthPoints; 

    if (EnemyHealthPoints <= 0)
    {
        EnemyAlive = false;
        StartCoroutine(Destroyed());
    }
}
    IEnumerator Destroyed()
    {
        //plays death animation, stops the moevement and destroys the enemy object.
        animator.Play("Death", 0, 0f); 
        ObjectiveManager.Instance.TrackObjective("Fight off an enemy");
        NavAgent.isStopped = true;
        yield return new WaitForSeconds(5f); 
        Destroy(gameObject); 
    }

    
}

