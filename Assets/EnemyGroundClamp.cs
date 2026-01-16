using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyGroundClamp : MonoBehaviour
{
    public float groundOffset = 3f;
    public float rayDistance = 20f;

    Rigidbody rb;
    Collider myCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        Vector3 origin = transform.position + Vector3.up * (groundOffset + 1f);

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (!hit.collider.isTrigger && hit.collider != myCollider)
            {
                Vector3 targetPos = rb.position;
                targetPos.y = hit.point.y + groundOffset;

                rb.position = targetPos;
            }
        }

        Debug.DrawRay(origin, Vector3.down * rayDistance, Color.red);
    }
}
