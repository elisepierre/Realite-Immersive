using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserEnemyAttack : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;

    public float attackRange = 20f;
    public float damagePerSecond = 10f;

    [Header("Cycle d'attaque")]
    public float laserDuration = 3f;
    public float cooldownDuration = 5f;

    private float timer = 0f;
    private bool laserActive = false;

    private LineRenderer lineRenderer;
    private PlayerHealth playerHealth;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = 0.08f;
        lineRenderer.endWidth = 0.08f;

        if (player == null)
        {
            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                player = cam.transform;
        }

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (player == null || firePoint == null || playerHealth == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        float distance = Vector3.Distance(firePoint.position, player.position);

        timer += Time.deltaTime;

        if (laserActive)
        {
            if (timer >= laserDuration)
            {
                laserActive = false;
                timer = 0f;
                lineRenderer.enabled = false;
            }
        }
        else
        {
            if (timer >= cooldownDuration)
            {
                laserActive = true;
                timer = 0f;
            }
        }

        if (distance > attackRange || !laserActive)
        {
            lineRenderer.enabled = false;
            return;
        }

        FireLaser();
    }

    private void FireLaser()
    {
        Vector3 direction = (player.position - firePoint.position).normalized;
        Vector3 visualOffset = transform.right * 0.15f;

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position + visualOffset);

        if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, attackRange))
        {
            lineRenderer.SetPosition(1, hit.point + visualOffset);

            PlayerHealth health = hit.transform.GetComponentInParent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, firePoint.position + direction * attackRange + visualOffset);
        }
    }
}