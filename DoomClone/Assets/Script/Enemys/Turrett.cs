using UnityEngine;

public class Turrett : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 0.1f;
    public float projectileSpeed = 10f;
    public int damage = 10;
    float shootingRange;

    private Transform player;
    private bool canShoot = false;
    private GameObject projectile;

    private void Start()
    {
        shootingRange = projectilePrefab.GetComponent<Projectile>().homingDistance; // ShootingRange gets set to the homing range of the projectile
        // Find the player object using the FindObjectOfType method
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = player.position - transform.position;

            // Check if the player is within shooting range
            if (directionToPlayer.magnitude <= shootingRange)
            {
                // Start shooting after 1 second of detecting the player
                if (!canShoot)
                {
                    canShoot = true;
                    Invoke("ShootProjectile", shootInterval);
                }

                // Rotate the turret to face the player
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
        }
    }

    private void ShootProjectile()
    {

        canShoot = false;
        // Instantiate the projectile and set its properties
        projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetDamage(damage);
        projectileScript.SetSpeed(projectileSpeed);
    }
}