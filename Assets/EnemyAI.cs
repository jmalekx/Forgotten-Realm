using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public Transform playerVariable;
    public float maxDistance = 1.5f;
    public float EnemyDetectionDistance = 20.0f;
    public float enemyAttackAmount;
    public float attackCooldown = 1.5f;  // Time before the enemy can attack again
    private float attackTimer = 0f;
    NavMeshAgent NavAgent;
    Animator animator;
    bool AttackingState = false;
    private Slider playerHealthBar; 
  //  public TMP_Text DeathText;

    // Start is called before the first frame update
    void Start()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealthBar = playerVariable.GetComponent<PlayerController>().HealthBar;
        enemyAttackAmount = MainMenu.ChosenenemyAttackAmount;
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
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Attack");
                AttackingState = true;
                playerHealthBar.value -= enemyAttackAmount;
                attackTimer = attackCooldown;
            }
        }
        else{
            NavAgent.destination = transform.position; //stop moving
            animator.SetFloat("Speed", 0);
            AttackingState = false; 
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;  // Reduce the timer by deltaTime
        }
        else
        {
            AttackingState = false;  // Reset the attack state after cooldown
        }
    
    }

    
}




    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player")) 
    //     {
    //         playerHealthBar.value -= enemyAttackAmount;
    //         if(playerHealthBar.value <= 0){
    //           //  string displayText = "GAME OVER";
    //             //DeathText.text  = displayText;
    //             enabled = false;
    //         }
    //     }
    // }