using UnityEngine;

public class SwarmEnemy : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public float attackCooldown = 0.7f;

    private Transform player;
    private Player playerHealth;
    private bool canAttack = true;
    private Rigidbody rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (player != null)
        {
            // Move towards the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            rb.velocity = directionToPlayer * movementSpeed;

            // Check if within attack range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                // Attack the player
                if (canAttack)
                {
                    AttackPlayer();
                    canAttack = false;
                    Invoke("ResetAttackCooldown", attackCooldown);
                }
            }
        }
    }

    private void AttackPlayer()
    {
        // Perform melee attack on the player
        // Code for melee attack goes here
        playerHealth.TakeDamage(attackDamage);
        Debug.Log("Enemy performs melee attack on the player!");
        
        // Apply attack damage to the player
        // Code for applying attack damage goes here
    }

    private void ResetAttackCooldown()
    {
        canAttack = true;
    }
}