using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private Rigidbody rb;
    private float homingTime = 0.2f;
    private Transform target;
    private bool isHoming = false;
    public float homingDistance = 30.0f; // Adjust this value as needed
    Vector3 randomBullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(killBullet());
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        randomBullet = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), Random.Range(-1f, 2f));
        randomBullet = target.position + randomBullet;
    }

    private void Update()
    {
        if (isHoming)
        {
            if (target != null)
            {
                Vector3 direction = randomBullet - transform.position;
                rb.velocity = direction.normalized * speed;
            }
        }

        homingTime -= Time.deltaTime;
        if (homingTime <= 0)
        {
            isHoming = false;
        }
        else
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= homingDistance)
            {
                isHoming = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("OOF");
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator killBullet()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }
}