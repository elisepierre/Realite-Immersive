using UnityEngine;

public class GolemRoarAttack : MonoBehaviour
{
    public float roarRadius = 6f;
    public float roarDamage = 20f;
    public string playerTag = "MainCamera";

    public void DoRoarDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, roarRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag(playerTag))
            {
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(roarDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, roarRadius);
    }
}
