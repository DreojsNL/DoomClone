using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HitScanAI : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public int damage = 15;
    public float shootingRange = 10f;
    public float detectionRange = 15f; // Detection range for the player
    public float shootCooldown = 1.5f; // Cooldown in seconds
    private float wanderTimerBetweenShots = 1.5f; // Time for wandering between shots
    private float lateralMovementRange = 2f; // Range for lateral movement after each shot
    public float turnSpeed = 5f; // Turn speed in degrees per second
    public float wanderRadius = 5f; // Radius for wandering
    private NavMeshAgent navMeshAgent;
    public float shootingAngle;
    public bool isShooting = false;
    private bool detected = false;
    public bool isMoving = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        navMeshAgent.speed = moveSpeed; // Set the initial movement speed
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

            // Check if not currently shooting
            if (!isShooting)
            {
                // Set the destination to the player's position
                navMeshAgent.SetDestination(player.transform.position);

                // Check if the player is within shooting range
                if (distanceToPlayer < shootingRange)
                {
                    // Rotate the enemy smoothly towards the player

                    // Start shooting coroutine
                    StartCoroutine(ShootCoroutine());
                }
            }
        }

        // Check if the AI is moving at least at a speed of 1
        isMoving = navMeshAgent.velocity.magnitude >= 1f;
    }

    IEnumerator WanderCoroutine()
    {
        while (!detected)
        {
            // If not detected, enter wander state
            Vector3 randomPoint = RandomNavSphere(transform.position, wanderRadius, -1);

            // Add a random lateral movement
            Vector3 lateralMovement = new Vector3(Random.Range(-lateralMovementRange, lateralMovementRange), 0f, 0f);
            randomPoint += lateralMovement;

            navMeshAgent.SetDestination(randomPoint);

            // Wait for the specified wander time
            yield return new WaitForSeconds(wanderTimerBetweenShots);
        }
    }

    void SmoothLookAt(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    IEnumerator ShootCoroutine()
    {
        // Set shooting state to true
        isShooting = true;

        // Shoot if the enemy has a clear line of sight
        Shoot();

        // Stop walking while shooting
        navMeshAgent.isStopped = true;

        // Wait for the cooldown
        yield return new WaitForSeconds(shootCooldown);

        // Resume walking and set shooting state to false
        navMeshAgent.isStopped = false;
        isShooting = false;

        // Wander between shots
        StartCoroutine(WanderCoroutine());
    }

    void Shoot()
    {
        // Calculate a random angle adjustment
        float randomAngle = Random.Range(-shootingAngle, shootingAngle); // Adjust the range as needed

        // Apply the random angle to the forward direction
        Vector3 shootingDirection = Quaternion.Euler(0f, randomAngle, 0f) * transform.forward;

        Ray ray = new Ray(transform.position, shootingDirection);
        RaycastHit hit;

        // Draw the ray in the scene view for visualization
        Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.red, 0.1f);

        if (Physics.Raycast(ray, out hit, shootingRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<Player>().TakeDamage(damage);
                Debug.Log("Enemy shoots player!");
            }
        }
    }

    // Helper function to get a random point within a specified radius on the NavMesh
    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;

        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
