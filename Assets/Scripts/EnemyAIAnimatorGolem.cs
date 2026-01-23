using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIAnimatorGolem : MonoBehaviour
{
    public Transform player;

    [Header("Distances")]
    public float activationDistance = 10f;
    public float attackDistance = 2f;

    [Header("Attack Settings")]
    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    [Header("Roar Settings")]
    public float roarCooldown = 8f;
    private float nextRoarTime = 0f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject cam = GameObject.FindWithTag("MainCamera");
            if (cam != null)
                player = cam.transform;
        }
    }

    private void Update()
    {
        if (player == null || isDead)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
        }

        if (distance <= activationDistance && Time.time >= nextRoarTime)
        {
            agent.isStopped = true;
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Roar");
            nextRoarTime = Time.time + roarCooldown;
            return;
        }

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
            animator.SetBool("IsWalking", false);

            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }

            return;
        }

        if (distance > activationDistance)
        {
            agent.isStopped = true;
            animator.SetBool("IsWalking", false);
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("IsWalking", true);
    }

    public void EnterEnrageMode()
    {
        agent.speed *= 1.5f;
        attackCooldown *= 0.6f;
        roarCooldown *= 0.5f;

        animator.SetTrigger("Enrage");

        GolemFlameAura aura = GetComponent<GolemFlameAura>();
        if (aura != null)
            aura.StartAura();
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        agent.enabled = false;

        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Die");
    }
}
