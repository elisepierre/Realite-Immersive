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

    [Header("Réglage de Hauteur")]
    [Tooltip("L'onde apparaîtra à cette hauteur par rapport au sol")]
    public float shockwaveSpawnHeight = 1.5f;
    [Tooltip("Marge autorisée au-dessus de l'onde pour valider le saut (ex: 0.8)")]
    public float jumpLeeway = 0.8f;

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
            if (!isDead && player != null && Vector3.Distance(transform.position, player.position) <= activationDistance)
            {
                UseSpecialAbility();
            }
        }
    }

    private void UseSpecialAbility()
    {
        if (shockwavePrefab != null)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + shockwaveSpawnHeight, transform.position.z);
            GameObject wave = Instantiate(shockwavePrefab, spawnPos, Quaternion.identity);
            StartCoroutine(AnimateShockwave(wave));
        }
    }

    private IEnumerator AnimateShockwave(GameObject wave)
    {
        LineRenderer line = wave.GetComponent<LineRenderer>();
        line.positionCount = ringSegments + 1;

        float currentRadius = 0f;
        bool hasHitPlayer = false;
        Vector3 waveOrigin = wave.transform.position;

        while (currentRadius < shockwaveRadius)
        {
            currentRadius += Time.deltaTime * shockwaveExpandSpeed;
            DrawCircle(line, currentRadius, waveOrigin);

            if (!hasHitPlayer && player != null)
            {
                float playerDist = Vector2.Distance(
                    new Vector2(player.position.x, player.position.z),
                    new Vector2(waveOrigin.x, waveOrigin.z)
                );

                if (playerDist <= currentRadius)
                {
                    float heightDiff = Mathf.Abs(player.position.y - waveOrigin.y);

                    if (heightDiff < jumpLeeway)
                    {
                        player.GetComponent<PlayerHealth>()?.TakeDamage(shockwaveDamage);
                        Debug.Log("<color=red>ONDE TOUCHÉE ! Y Joueur: " + player.position.y + " | Y Onde: " + waveOrigin.y + "</color>");
                    }
                    else
                    {
                        Debug.Log("<color=green>ONDE ÉVITÉE ! Trop haut ou trop bas par rapport à l'onde.</color>");
                    }
                    hasHitPlayer = true;
                }
            }
            yield return null;
        }
        Destroy(wave);
    }

    private void DrawCircle(LineRenderer line, float radius, Vector3 center)
    {
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