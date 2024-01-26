using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public GameObject guts;
    private Bomber bm;

    private void Start()
    {
        bm = GetComponent<Bomber>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (guts != null)
        {
            Instantiate(guts, gameObject.transform.position, Quaternion.identity);

        }
        if (bm != null)
        {
            StartCoroutine(bm.ExplodeCoroutine());
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
}