using UnityEngine;

public class DamageProjectile : MonoBehaviour
{
    public float damageAmount = 25f;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
        }

        Destroy(gameObject);
    }
}

