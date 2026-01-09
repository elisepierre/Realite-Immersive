using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Respawn")]
    public Transform respawnRoot;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Awake()
    {
        if (respawnRoot == null)
            respawnRoot = transform.root;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        spawnPosition = respawnRoot.position;
        spawnRotation = respawnRoot.rotation;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
            Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;

        respawnRoot.position = spawnPosition;
        respawnRoot.rotation = spawnRotation;

        var rb = respawnRoot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}