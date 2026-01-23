using UnityEngine;
using System.Collections;

public class PlayerHitZone : MonoBehaviour
{
    public float damageAmount = 10f;
    public string enemyTag = "Enemy";

    private PlayerHealth playerHealth;
    private bool isDamaging = false;

    private void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(enemyTag))
            return;

        if (!isDamaging)
        {
            isDamaging = true;
            StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(enemyTag))
            return;

        StopAllCoroutines();
        isDamaging = false;
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            playerHealth.TakeDamage(damageAmount);
            yield return new WaitForSeconds(1f);
        }
    }
}
