using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIAnimatorGolem : MonoBehaviour
{
    public Transform player;

    [Header("Distances")]
    public float activationDistance = 10f;
    public float attackDistance = 1.8f;
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
    private Animator anim;

    private bool isPreparingDash = false;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

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
    }

    private void Update()
    {
        if (isDead || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isDashing)
            RotateTowards(player.position);

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            return;
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }

        if (distance > activationDistance)
        {
            agent.isStopped = true;
            anim.SetBool("isWalking", false);
            return;
        }

        if (distance <= dashDistance && canDash && !isPreparingDash)
        {
            StartCoroutine(PrepareDash());
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetBool("isWalking", true);
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

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


    public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        anim.SetBool("isDead", true);

        rb.isKinematic = true;
        agent.enabled = false;

        Destroy(gameObject, 4f);
    }
}
