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

    private Player playerStats;


    private bool slowBlessingActive = false;
    public bool IsSlowBlessingActive => slowBlessingActive;

    private void Awake()
    {
        if (respawnRoot == null)
            respawnRoot = transform.root;

        playerStats = GetComponent<Player>();
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

        if (playerStats != null)
            playerStats.Mourir();

        respawnRoot.position = spawnPosition;
        respawnRoot.rotation = spawnRotation;

        Rigidbody rb = respawnRoot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    public void ActivateSlowEnemiesBlessing()
    {
        if (slowBlessingActive)
            return;

        slowBlessingActive = true;

        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

        foreach (EnemyAI enemy in enemies)
            enemy.ApplySlowBlessing(true);
    }
}
