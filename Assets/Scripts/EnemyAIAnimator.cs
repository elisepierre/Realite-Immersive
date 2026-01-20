using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIAnimator : MonoBehaviour
{
    public Transform player;
    public float activationDistance = 10f;

    [Header("Capacité Spéciale (Onde)")]
    public GameObject shockwavePrefab;
    public float specialCooldown = 15f;
    public float shockwaveRadius = 7f;
    public float shockwaveDamage = 25f;
    public float shockwaveExpandSpeed = 10f;
    public int ringSegments = 50;

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

        StartCoroutine(AbilityLoop());
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

    private IEnumerator AbilityLoop()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(specialCooldown);
            if (!isDead && Vector3.Distance(transform.position, player.position) <= activationDistance)
            {
                UseSpecialAbility();
            }
        }
    }

    private void UseSpecialAbility()
    {
        if (shockwavePrefab != null)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            GameObject wave = Instantiate(shockwavePrefab, spawnPos, Quaternion.identity);
            StartCoroutine(AnimateShockwave(wave));
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveRadius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerHealth>()?.TakeDamage(shockwaveDamage);
            }
        }
    }

    private IEnumerator AnimateShockwave(GameObject wave)
    {
        LineRenderer line = wave.GetComponent<LineRenderer>();
        line.positionCount = ringSegments + 1;

        float currentRadius = 0f;

        while (currentRadius < shockwaveRadius)
        {
            currentRadius += Time.deltaTime * shockwaveExpandSpeed;
            DrawCircle(line, currentRadius);
            yield return null;
        }

        Destroy(wave);
    }

    private void DrawCircle(LineRenderer line, float radius)
    {
        Vector3 center = line.transform.position;
        float angle = 0f;

        for (int i = 0; i <= ringSegments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, center + new Vector3(x, 0, z));
            angle += (360f / ringSegments);
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