using UnityEngine;

public class SlowBlessingPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MainCamera"))
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ActivateSlowBlessing();
        }

        Destroy(gameObject);
    }
}
