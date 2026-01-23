using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChute : MonoBehaviour
{
    [Header("Réglages Impact")]
    public float forceTremblement = 0.1f; 
    public float dureeTremblement = 0.3f;
    public AudioSource sonImpact;       // Le bruit 
    
    [Header("Animation")]
    public Animator bossAnimator;
    public string nomTriggerAtterrissage = "Land";

    private bool aAtterri = false;

    void OnCollisionEnter(Collision collision)
    {
        // On vérifie qu'on touche le sol 
        if (!aAtterri && (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Plane") || collision.gameObject.name.Contains("Terrain")))
        {
            FaireLImpact();
        }
    }

    void FaireLImpact()
    {
        aAtterri = true;
        Debug.Log("BOUM ! Hades a touché le sol.");
        // On appelle notre script spécial VR
        CameraShakeVR.instance.Secouer(dureeTremblement, forceTremblement);
        sonImpact.Play();
        bossAnimator.SetTrigger(nomTriggerAtterrissage);

    }
}