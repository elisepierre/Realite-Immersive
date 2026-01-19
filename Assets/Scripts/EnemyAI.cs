using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float rotationSpeed = 5f;

    [Header("Distances")]
    public float activationDistance = 10f;
    public float dashDistance = 3f;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.25f;
    public float dashPreparationTime = 0.4f;
    public float dashCooldown = 2f;

    private float originalSpeed;
    private Rigidbody rb;

    private bool isPreparingDash = false;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;

    private void Start()
    {
        originalSpeed = speed;
        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.freezeRotation = true;

        if (player == null)
        {
            GameObject cam = GameObject.Find("Main Camera");
            if (cam != null)
                player = cam.transform;
            else
                Debug.LogWarning("Main Camera introuvable. L'ennemi ne pourra pas suivre le joueur.");
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 flatDirection = player.position - transform.position;
        flatDirection.y = 0f;
        float distance = flatDirection.magnitude;

        if (distance > activationDistance)
            return;

        if (isDashing)
        {
            DashMovement(flatDirection);
            return;
        }

        if (isPreparingDash)
            return;

        if (distance <= dashDistance && canDash)
        {
            StartCoroutine(PrepareDash());
            return;
        }

        MoveNormally(flatDirection);
        RotateTowardsPlayer();
    }

    private void MoveNormally(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();
            Vector3 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0f, 180f, 0f);
            Quaternion smoothRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(smoothRotation);
        }
    }

    private System.Collections.IEnumerator PrepareDash()
    {
        isPreparingDash = true;
        canDash = false;
        speed = 0f;

        yield return new WaitForSeconds(dashPreparationTime);

        isPreparingDash = false;
        isDashing = true;
        dashTimer = dashDuration;
    }

    private void DashMovement(Vector3 direction)
    {
        direction.Normalize();

        Vector3 dashPosition = rb.position + direction * dashSpeed * Time.fixedDeltaTime;
        rb.MovePosition(dashPosition);

        dashTimer -= Time.fixedDeltaTime;

        if (dashTimer <= 0f)
        {
            isDashing = false;
            speed = originalSpeed;

            StartCoroutine(DashCooldownRoutine());
        }
    }

    private System.Collections.IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ResetSpeed()
    {
        speed = originalSpeed;
    }
}
