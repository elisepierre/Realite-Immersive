using UnityEngine;

public class EnemyHealthGolem : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private EnemyAIAnimatorGolem enemyAI;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAIAnimatorGolem>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        enemyAI?.Die();
        Destroy(gameObject, 4f);
    }
}
