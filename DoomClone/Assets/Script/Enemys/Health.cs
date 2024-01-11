using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    private void Start()
    {
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
       Destroy(gameObject);
    }
}