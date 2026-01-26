using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(" L'ennemi est mort !"); // Mouchard 1

        RewardSystem rewards = FindObjectOfType<RewardSystem>();

        if (rewards != null)
        {
            Debug.Log("Système de récompense trouvé ! Lancement..."); // Mouchard 2
            rewards.ShowRewards();
        }
        else
        {
            Debug.LogError(" ERREUR : Aucun 'RewardSystem' trouvé dans la scène !"); // Mouchard 3
        }

        Destroy(gameObject);
    }
}
