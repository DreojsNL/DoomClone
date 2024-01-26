using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HitScanAI : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    public int damage = 15;
    public float shootingRange = 10f;
    public float detectionRange = 15f;
    public float shootCooldown = 1.5f;
    private float wanderTimerBetweenShots = 1.5f;
    private float lateralMovementRange = 2f;
    public float turnSpeed = 5f;
    public float wanderRadius = 5f;
    private NavMeshAgent navMeshAgent;
    public float shootingAngle;
    public bool isShooting = false;
    private bool detected = false;
    public bool isMoving = false;

    // Audio variables


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        navMeshAgent.speed = moveSpeed;

        // Initialize the audio source and clip


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

            if (!isShooting)
            {
                navMeshAgent.SetDestination(player.transform.position);

                if (distanceToPlayer < shootingRange)
                {
                    StartCoroutine(ShootCoroutine());
                }
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

    IEnumerator ShootCoroutine()
    {
        isShooting = true;

        Shoot();

        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(shootCooldown);

        navMeshAgent.isStopped = false;
        isShooting = false;

        StartCoroutine(WanderCoroutine());
    }

    void Shoot()
    {
        float randomAngle = Random.Range(-shootingAngle, shootingAngle);
        Vector3 shootingDirection = Quaternion.Euler(0f, randomAngle, 0f) * transform.forward;

        Ray ray = new Ray(transform.position, shootingDirection);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * shootingRange, Color.red, 0.1f);

        if (Physics.Raycast(ray, out hit, shootingRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponent<Player>().TakeDamage(damage);
                Debug.Log("Enemy shoots player!");

                // Play shooting audio
               
            }
        }
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
