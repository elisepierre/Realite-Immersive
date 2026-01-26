using UnityEngine;

public class HUDFollow : MonoBehaviour
{
    [Header("Cible")]
    public Transform cameraTransform;

    [Header("Position")]
    public float distance = 2.0f;
    public float heightOffset = 0.5f;

    [Header("Fluidité")]
    public float smoothSpeed = 5.0f;

    void Update()
    {
        if (cameraTransform == null) return;

        Vector3 targetPosition = cameraTransform.position
                               + (cameraTransform.forward * distance)
                               + (cameraTransform.up * heightOffset);

        Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}