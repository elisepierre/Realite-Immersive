using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChute : MonoBehaviour
{
    [Header("Réglages Impact")]
    public float forceTremblement = 0.1f; // Doux pour la VR
    public float dureeTremblement = 0.3f;
    public GameObject particulesImpact; // Ton effet de poussière/explosion
    public AudioSource sonImpact;       // Le bruit "BOUM"
    
    [Header("Animation")]
    public Animator bossAnimator;
    public string nomTriggerAtterrissage = "Land";

    private bool aAtterri = false;

    void OnCollisionEnter(Collision collision)
    {
        // On vérifie qu'on touche le sol (Tag "Ground" ou nom contenant "Plane" ou "Terrain")
        if (!aAtterri && (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Plane") || collision.gameObject.name.Contains("Terrain")))
        {
            FaireLImpact();
        }
    }

    void FaireLImpact()
    {
        aAtterri = true;
        Debug.Log("BOUM ! Hades a touché le sol.");

        // --- 1. TREMBLEMENT VR ---
        // On appelle notre script spécial VR
        if (CameraShakeVR.instance != null)
        {
            CameraShakeVR.instance.Secouer(dureeTremblement, forceTremblement);
        }
        else
        {
            Debug.LogWarning("Attention : Le script CameraShakeVR n'est pas trouvé dans la scène (sur XR Origin ?)");
        }

        // --- 2. AUDIO ---
        if (sonImpact != null) sonImpact.Play();

        // --- 3. PARTICULES ---
        if (particulesImpact != null)
        {
            // On fait apparaître la poussière aux pieds du boss (légèrement relevée)
            Vector3 pos = transform.position;
            pos.y += 0.1f; 
            Instantiate(particulesImpact, pos, Quaternion.identity);
        }

        // --- 4. ANIMATION ---
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger(nomTriggerAtterrissage);
        }

        // NOTE : On ne lance plus l'IA ici, car HadesAI a son propre minuteur (les 21.57s) !
    }
}