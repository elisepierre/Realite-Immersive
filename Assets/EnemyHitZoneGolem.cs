using UnityEngine;
using UnityEngine.Events;

public class EnemyHitZoneGolem : MonoBehaviour
{
    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private string requiredTag = "Projectile";
    [SerializeField] private UnityEvent m_OnHit;

    private EnemyHealthGolem enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealthGolem>();
        if (enemyHealth == null)
            Debug.LogWarning("EnemyHealthGolem non trouvé sur le parent.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag))
            return;

        m_OnHit?.Invoke();

        enemyHealth?.TakeDamage(damageAmount);
    }
}
