using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PorteAventure : MonoBehaviour
{
    [Header("Réglages Porte")]
    public float hauteurOuverture = 3f;
    public float vitesse = 2f;
    public GameObject texteAventure;

    [Header("Réglages Destination")]
    public string nomSceneDestination;

    [Header("Debug")]
    public bool estEnOuverture = false;
    private bool chargementEnCours = false;
    private Vector3 positionFinale;

    void Start()
    {
        positionFinale = transform.position + Vector3.up * hauteurOuverture;
    }

    void Update()
    {
        if (estEnOuverture)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionFinale, vitesse * Time.deltaTime);
        }
    }

    // C'est ici que toute la magie opère
    private void OnTriggerEnter(Collider other)
    {
        // -----------------------------------------------------------
        // CAS 1 : C'est l'ARME qui touche -> On OUVRE la porte
        // -----------------------------------------------------------
        if (other.CompareTag("Weapon"))
        {
            if (!estEnOuverture)
            {
                Debug.Log("L'arme a touché la porte ! Ouverture...");
                estEnOuverture = true;
                if (texteAventure != null) texteAventure.SetActive(false);
            }
        }

        // -----------------------------------------------------------
        // CAS 2 : C'est le JOUEUR qui touche -> On CHANGE de scène
        // -----------------------------------------------------------
        if (other.CompareTag("Player"))
        {
            // On vérifie que la porte est bien en train de s'ouvrir avant de téléporter
            if (estEnOuverture && !chargementEnCours)
            {
                Debug.Log("Le joueur traverse ! Au revoir.");
                chargementEnCours = true;
                SceneManager.LoadScene(nomSceneDestination);
            }
        }
    }
}