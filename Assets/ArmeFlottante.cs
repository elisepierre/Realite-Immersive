using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmeFlottante : MonoBehaviour
{
    [Header("Réglages Flottement")]
    public float vitesseRotation = 50f;
    public float amplitudeHautBas = 0.1f; // Distance du mouvement
    public float vitesseHautBas = 2f;     // Rapidité du mouvement

    private Vector3 positionDepart;
    private bool estRamasse = false;
    private Rigidbody rb;

    void Start()
    {
        positionDepart = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Si l'arme est ramassée, on arrête l'animation de flottement
        if (estRamasse) return;

        // 1. Rotation sur elle-même
        transform.Rotate(0, vitesseRotation * Time.deltaTime, 0);

        // 2. Mouvement de haut en bas (Sinusoïdale)
        float newY = positionDepart.y + Mathf.Sin(Time.time * vitesseHautBas) * amplitudeHautBas;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Cette fonction sera appelée par le XR Interaction Toolkit
    public void OnGrab()
    {
        estRamasse = true;
        
        // On réactive la gravité pour que si on la lâche, elle tombe au sol
        if(rb != null)
        {
            rb.useGravity = true;
        }
    }
}