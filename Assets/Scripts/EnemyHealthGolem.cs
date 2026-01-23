using UnityEngine;

public class EnemyHealthGolem : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;

    public bool isEnraged = false;

    private EnemyAIAnimatorGolem ai;

    private void Start()
    {
        currentHealth = maxHealth;
        ai = GetComponent<EnemyAIAnimatorGolem>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (!isEnraged && currentHealth <= maxHealth * 0.3f)
        {
            isEnraged = true;
            ai.EnterEnrageMode();
        }

        if (currentHealth <= 0f)
        {
            ai.Die();
        }
    }
}
