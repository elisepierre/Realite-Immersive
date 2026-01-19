using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyAIAnimator : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float rotationSpeed = 5f;

    [Header("Distance d'activation")]
    public float activationDistance = 10f;

    private Rigidbody rb;
    private Animator animator;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        rb.useGravity = true;

        if (player == null)
        {
            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                player = cam.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player == null || isDead) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        float distance = dir.magnitude;

        if (distance > activationDistance)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        bool isMoving = dir.sqrMagnitude > 0.001f;
        animator.SetBool("IsMoving", isMoving);

        if (!isMoving) return;

        dir.Normalize();
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);

        Quaternion targetRot = Quaternion.LookRotation(dir);
        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime)
        );
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Die");
        rb.isKinematic = true;
    }
}
