using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float rotationSpeed = 5f;

    [Header("Distance d'activation")]
    public float activationDistance = 10f;

    private float originalSpeed;
    private Rigidbody rb;

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
            {
                player = cam.transform;
            }
            else
            {
                Debug.LogWarning("Main Camera introuvable. L'ennemi ne pourra pas suivre le joueur.");
            }
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

        if (flatDirection.sqrMagnitude > 0.001f)
        {
            flatDirection.Normalize();
            Vector3 newPosition = rb.position + flatDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }

        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0f, 180f, 0f);

            Quaternion smoothRotation = Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(smoothRotation);
        }
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
