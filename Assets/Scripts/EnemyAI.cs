using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;

    [Header("Distances")]
    public float activationDistance = 10f;
    public float dashDistance = 3f;

    [Header("Movement Speed")]
    public float baseMoveSpeed = 3.5f;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.25f;
    public float dashPreparationTime = 0.4f;
    public float dashCooldown = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    private NavMeshAgent agent;
    private Rigidbody rb;

    private bool isPreparingDash = false;
    private bool isDashing = false;
    private bool canDash = true;

    private float speedMultiplier = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        rb.isKinematic = true;
        rb.freezeRotation = true;

        agent.updateRotation = false;
        agent.speed = baseMoveSpeed;

        if (player == null)
        {
            GameObject cam = GameObject.FindWithTag("MainCamera");
            if (cam != null)
                player = cam.transform;
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null && playerHealth.IsSlowBlessingActive)
        {
            ApplySlowBlessing(true);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isDashing)
            RotateTowards(player.position);

        if (isDashing || isPreparingDash)
            return;

        if (distance > activationDistance)
        {
            agent.isStopped = true;
            return;
        }

        if (distance <= dashDistance && canDash)
        {
            StartCoroutine(PrepareDash());
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        targetRotation *= Quaternion.Euler(0f, 180f, 0f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private System.Collections.IEnumerator PrepareDash()
    {
        isPreparingDash = true;
        canDash = false;
        agent.isStopped = true;

        yield return new WaitForSeconds(dashPreparationTime);

        isPreparingDash = false;
        isDashing = true;

        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 dashDir = (player.position - transform.position).normalized;
        dashDir.y = 0f;

        float timer = dashDuration;

        while (timer > 0f)
        {
            rb.velocity = dashDir * dashSpeed;
            timer -= Time.deltaTime;
            yield return null;
        }

        StopDash();
    }

    private void StopDash()
    {
        isDashing = false;

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        agent.enabled = true;

        StartCoroutine(DashCooldownRoutine());
    }

    private System.Collections.IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    public void ApplySlowBlessing(bool active)
    {
        speedMultiplier = active ? 0.3f : 1f;
        agent.speed = baseMoveSpeed * speedMultiplier;
    }
}
