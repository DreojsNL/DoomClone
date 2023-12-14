using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 1f;
    public float projectileSpeed = 1000000000000f;
    public int damage = 10;

    private Transform player;
    private bool canShoot = false; 
    public GameObject projectile;

    private void Start()
    {
        // Find the player object using the FindObjectOfType method
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = player.position - transform.position;

            // Start shooting after 1 second of detecting the player
            if (!canShoot)
            {
                canShoot = true;
                Invoke("ShootProjectile", 0.3f);
            }

            // Rotate the turret to face the player
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
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