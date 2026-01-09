using UnityEngine;
using UnityEngine.Events;

public class EnemyHitZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private string requiredTag = "Projectile";
    [SerializeField] private UnityEvent m_OnHit;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        if (enemyHealth == null)
            Debug.LogWarning("EnemyHealth non trouv� sur le parent.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag))
            return;

        m_OnHit.Invoke();
        enemyHealth?.TakeDamage(damageAmount);
    }
}
