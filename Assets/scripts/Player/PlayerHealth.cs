using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed {amount}. Current health: {currentHealth}");
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died.");

        if (anim != null)
        {
            anim.SetTrigger("Dead");
        }

        GameManager.Instance.PlayerDied(); // Notify GameManager

        // Optional: disable movement or trigger respawn logic here
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
