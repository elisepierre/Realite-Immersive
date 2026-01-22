using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChute : MonoBehaviour
{
    [Header("Réglages Impact")]
    public float forceTremblement = 0.2f;
    public float dureeTremblement = 0.5f;
    public GameObject particulesImpact; // Glisse un prefab de poussière/explosion ici
    public AudioSource sonImpact; // Glisse ton bruitage "BOUM" ici
    
    [Header("Animation")]
    public Animator bossAnimator;
    public string nomTriggerAtterrissage = "Land"; // Le nom du paramètre dans l'Animator

    private bool aAtterri = false;

    void OnCollisionEnter(Collision collision)
    {
        // Si on touche le sol et qu'on n'a pas encore atterri
        if (!aAtterri && (collision.gameObject.CompareTag("Ground") || collision.gameObject.name.Contains("Plane")))
        {
            FaireLImpact();
        }
    }

    void FaireLImpact()
    {
        aAtterri = true;
        Debug.Log("BOUM ! Le boss est au sol.");

        // 1. Lancer le tremblement (appelle le script de l'étape 1)
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Secouer(dureeTremblement, forceTremblement);
        }

        // 2. Jouer le son
        if (sonImpact != null) sonImpact.Play();

        // 3. Créer des particules (poussière)
        if (particulesImpact != null)
        {
            Instantiate(particulesImpact, transform.position, Quaternion.identity);
        }

        // 4. Lancer l'animation d'atterrissage (passage de "Falling" à "Idle/Combat")
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger(nomTriggerAtterrissage);
        }
        
        // Optionnel : Désactiver la gravité ou passer en Kinematic si le boss utilise un NavMesh après
        // GetComponent<Rigidbody>().isKinematic = true; 
    }
}