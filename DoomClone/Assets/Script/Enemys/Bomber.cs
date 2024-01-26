using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Bomber : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public int damage = 15;
    public float explosionRadius = 5f;
    public float detectionRange = 15f;
    public float explosionDelay = 2f;
    private float wanderTimerBetweenShots = 1.5f;
    private float lateralMovementRange = 2f;
    public float turnSpeed = 5f;
    public float wanderRadius = 5f;
    public float explodeDistance = 5f;  // New variable for the distance to explode
    private NavMeshAgent navMeshAgent;
    public bool detected = false;
    public bool isMoving = false;
    public bool isExploding = false;
    public float knockbackForce = 10;
    private SpriteAnimation anim;

    void Start()
    {
        anim = GetComponent<SpriteAnimation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        navMeshAgent.speed = moveSpeed;

        StartCoroutine(WanderCoroutine());
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < detectionRange)
        {
            detected = true;
        }

        if (detected)
        {
            SmoothLookAt(player.transform.position);

        
                if (distanceToPlayer > explodeDistance)
                {
                    // Calculate the direction and distance to the player
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                    Vector3 targetPosition = player.transform.position - directionToPlayer * explodeDistance;

                    // Move towards the calculated position
                    navMeshAgent.SetDestination(targetPosition);
                }
                else
                {
                    // Stop moving and start exploding
                    navMeshAgent.SetDestination(gameObject.transform.position);
                    StartCoroutine(ExplodeCoroutine());
                }
            
        }

        isMoving = navMeshAgent.velocity.magnitude >= 1f;
    }


    IEnumerator WanderCoroutine()
    {
        while (!detected)
        {
            Vector3 randomPoint = RandomNavSphere(transform.position, wanderRadius, -1);
            Vector3 lateralMovement = new Vector3(Random.Range(-lateralMovementRange, lateralMovementRange), 0f, 0f);
            randomPoint += lateralMovement;

            navMeshAgent.SetDestination(randomPoint);

            yield return new WaitForSeconds(wanderTimerBetweenShots);
        }
    }

    void SmoothLookAt(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

   public IEnumerator ExplodeCoroutine()
    {
        isExploding = true;
        anim.SetAnimation(anim.shooting);
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= explosionRadius)
        {
            Player playerScript = player.GetComponent<Player>();
            playerScript.TakeDamage(damage);

            // Apply knockback force to the player
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                playerRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }

            Debug.Log("Enemy explodes near player!");
        }

        // Optionally, you can add explosion effects or sound here
        Destroy(gameObject);
    }


    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
