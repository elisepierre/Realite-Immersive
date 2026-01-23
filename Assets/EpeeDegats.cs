using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpeeDegats : MonoBehaviour
{
    [Header("Réglages")]
    public int degats = 10;              // Degats par coup
    public float delaiEntreCoups = 0.5f; // Temps min entre deux coups 

    [Header("Audio (Optionnel)")]
    public AudioSource sonImpact;       

    private float dernierCoup = 0f;  

    //epee touche physiquement un objet
    void OnCollisionEnter(Collision collision)
    {
        // Verification du delai
        if (Time.time - dernierCoup < delaiEntreCoups) return;

        // Recherche du script HadesAI sur l'objet touche à mettre pour chaque ennemi à faire apres fin j'espere
        HadesAI boss = collision.gameObject.GetComponent<HadesAI>();

        
        if (boss == null)
        {
            boss = collision.gameObject.GetComponentInParent<HadesAI>();
        }

        // on tape 
        if (boss != null)
        { 
            dernierCoup = Time.time;

            boss.PrendreDegats(degats);
            if (sonImpact != null)
            {
                sonImpact.Play();
            }

            Debug.Log("TOUCHÉ ! Hades a pris " + degats + " dégâts.");
        }
    }
}