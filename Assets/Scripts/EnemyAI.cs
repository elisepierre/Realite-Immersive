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
    private float dashTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        rb.isKinematic = true; 
        rb.freezeRotation = true;

        agent.updateRotation = false; 

        if (player == null)
        {
            GameObject cam = GameObject.FindWithTag("MainCamera");
            if (cam != null) player = cam.transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isDashing)
        {
            RotateTowards(player.position);
        }

        if (isDashing || isPreparingDash) return;

        if (distance > activationDistance)
        {
            if (agent.enabled) agent.isStopped = true;
            return;
        }

        if (distance <= dashDistance && canDash)
        {
            StartCoroutine(PrepareDash());
            return;
        }

        if (agent.enabled)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(0, 180, 0);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private System.Collections.IEnumerator PrepareDash()
    {
        isPreparingDash = true;
        canDash = false;
        agent.isStopped = true; 

        yield return new WaitForSeconds(dashPreparationTime);

        isPreparingDash = false;
        isDashing = true;
        dashTimer = dashDuration;

        agent.enabled = false; 
        rb.isKinematic = false;

        Vector3 dashDir = (player.position - transform.position).normalized;
        dashDir.y = 0;

        while (dashTimer > 0f)
        {
            rb.velocity = dashDir * dashSpeed;
            dashTimer -= Time.deltaTime;
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
}