using UnityEngine;

public class EnemyHealthAnimator : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private EnemyAIAnimator enemyAI;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAIAnimator>();
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
        Destroy(gameObject, 3f);
    }
}
