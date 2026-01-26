using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI - Interface")]
    public Slider healthSlider;

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

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        UpdateHealthUI();

        if (currentHealth <= 0f)
            Respawn();
    }

    private void Respawn()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();

        respawnRoot.position = spawnPosition;
        respawnRoot.rotation = spawnRotation;

        var rb = respawnRoot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}