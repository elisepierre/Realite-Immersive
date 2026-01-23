using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpeeDegats : MonoBehaviour
{
    [Header("Réglages")]
    public int degats = 10;              // Dégâts par coup
    public float delaiEntreCoups = 0.5f; // Temps min entre deux coups (pour éviter le spam)

    [Header("Audio (Optionnel)")]
    public AudioSource sonImpact;        // Glisse ton AudioSource ici

    private float dernierCoup = 0f;      // Timer interne

    // Cette fonction se déclenche quand l'épée touche physiquement un objet
    void OnCollisionEnter(Collision collision)
    {
        // 1. Vérification du délai (Anti-spam)
        if (Time.time - dernierCoup < delaiEntreCoups) return;

        // 2. Recherche du script HadesAI sur l'objet touché
        HadesAI boss = collision.gameObject.GetComponent<HadesAI>();

        // Si on ne le trouve pas directement (ex: on a touché le bras), on cherche sur le parent
        if (boss == null)
        {
            boss = collision.gameObject.GetComponentInParent<HadesAI>();
        }

        // 3. Si on a trouvé le boss, on tape !
        if (boss != null)
        {
            // Mise à jour du timer
            dernierCoup = Time.time;

            // Application des dégâts
            boss.PrendreDegats(degats);

            // Son
            if (sonImpact != null)
            {
                sonImpact.Play();
            }

            Debug.Log("TOUCHÉ ! Hades a pris " + degats + " dégâts.");
        }
    }
}