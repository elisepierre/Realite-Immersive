using UnityEngine;
using System.Collections;

public class PlayerHitZone : MonoBehaviour
{
    public float damageAmount = 10f;
    public float golemDamageAmount = 25f;

    public string enemyTag = "Enemy";
    public string golemTag = "Golem";

    private PlayerHealth playerHealth;
    private bool isDamaging = false;
    private float currentDamage;

    private void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(golemTag))
        {
            currentDamage = golemDamageAmount;
        }
        else if (other.CompareTag(enemyTag))
        {
            currentDamage = damageAmount;
        }
        else
        {
            return;
        }

        if (!isDamaging)
        {
            isDamaging = true;
            StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(enemyTag) && !other.CompareTag(golemTag))
            return;

        StopAllCoroutines();
        isDamaging = false;
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            playerHealth.TakeDamage(currentDamage);
            yield return new WaitForSeconds(1f);
        }
    }
}
