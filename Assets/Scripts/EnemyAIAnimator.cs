using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIAnimator : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 10f;

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
            if (cam != null) player = cam.transform;
        }
    }

    private void Update()
    {
        if (player == null || isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= activationDistance)
        {
            agent.SetDestination(player.position);
            agent.isStopped = false;
            
            animator.SetBool("IsMoving", true);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("IsMoving", false);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        agent.enabled = false;
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Die");
    }
}