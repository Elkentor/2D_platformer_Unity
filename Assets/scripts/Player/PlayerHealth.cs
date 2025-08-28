using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private float totalDamageTaken = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        totalDamageTaken += amount;

        Debug.Log($"Player took {amount} damage. Current HP: {currentHealth}. Total damage taken: {totalDamageTaken}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died.");
        // Add death logic here (animation, respawn, etc.)
    }
}
