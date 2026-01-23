using UnityEngine;
using System.Collections;

public class GolemFlameAura : MonoBehaviour
{
    public float auraRadius = 3f;
    public float auraDamage = 5f;
    public float tickRate = 0.5f;
    public string playerTag = "MainCamera";

    private bool auraActive = false;

    public void StartAura()
    {
        if (!auraActive)
        {
            auraActive = true;
            StartCoroutine(AuraRoutine());
        }
    }

    private IEnumerator AuraRoutine()
    {
        while (auraActive)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, auraRadius);

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag(playerTag))
                {
                    PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                    if (ph != null)
                        ph.TakeDamage(auraDamage);
                }
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, auraRadius);
    }
}
