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

        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);

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

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.enabled = false;

        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Die");
    }
}
