using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 1. INDISPENSABLE pour changer de scène

public class PorteAventure : MonoBehaviour
{
    [Header("Réglages Porte")]
    public float hauteurOuverture = 3f;
    public float vitesse = 2f;
    public GameObject texteAventure;

    [Header("Réglages Destination")]
    public string nomSceneDestination; // 2. Ecris le nom exact de la scène ici

    private bool estEnOuverture = false;
    private bool chargementEnCours = false;
    private Vector3 positionFinale;

    void Start()
    {
        positionFinale = transform.position + Vector3.up * hauteurOuverture;
    }

    void Update()
    {
        // Animation d'ouverture
        if (estEnOuverture)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionFinale, vitesse * Time.deltaTime);
            // On ne désactive plus le script ici, car on a besoin de OnTriggerEnter
        }
    }

    // --- INTERACTION ---
    private void OnMouseDown()
    {
        LancerAventure();
    }

    public void LancerAventure()
    {
        if (!estEnOuverture)
        {
            estEnOuverture = true;
            if (texteAventure != null) texteAventure.SetActive(false);
        }
    }

    // --- TELEPORTATION ---
    // 3. Cette fonction se déclenche quand tu marches DANS la porte
   private void OnTriggerStay(Collider other)
    {
        // 1. Sécurité : Si on est déjà en train de charger, on arrête tout
        if (chargementEnCours) { Debug.Log("Bouge pas ! ça charge..."); return; }

        // 2. On vérifie en continu si c'est le joueur ET si la porte est ouverte
        if (other.CompareTag("Player"))
        {
            Debug.Log("Joueur détecté dans la zone ! Vérification ouverture...");

            if (estEnOuverture)
            {
                Debug.Log("C'est bon ! Téléportation...");
                chargementEnCours = true; // On verrouille pour ne le faire qu'une fois
                SceneManager.LoadScene(nomSceneDestination);
            }
            else
            {
                Debug.Log("La porte n'est pas encore assez ouverte...");
            }
        }
    }
}