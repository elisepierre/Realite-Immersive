using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour
{

    [Header("Aura Globale")]
    //1.0 = Vitesse normale (100%)
    //0.9 = 90% de vitesse (donc 10% de ralentissement)
    public float multiplicateurVitesseEnnemis = 1.0f;

    [Header("Degats Ennemis")]
    public float multiplicateurEnnemis = 1.0f;


    // ELEMENT DE BASE
    public float vie = 100; // vie pour vie actuelle, celle qui diminue ave les degats subis
    public float degatsBase = 5;
    public float bonusDegats = 20;

    // VIE //
    public float vieMax = 100; // vie max, ne change pas sauf par bienfait
    public float reductionDegats = 0; // en %
    public bool invincible = false;
    public float duree_invincibilite = 20;
    public float debut_invincibilite;

    // DASH //
    public int nombreDash = 0;
    public float cooldownDash = 10;
    public float vitesseDeplacement = 10;
    public float bonusVitesse = 10;

    public List<Bienfait> bienfaitsActifs = new List<Bienfait>();
    public static List<string> historiqueDesBienfaits = new List<string>(); // cela marche meme apres la mort pour savoir les bienfaits qu'on a deja utilises par le passe


    // Référence vers le système de déplacement VR
    private ActionBasedContinuousMoveProvider moveProvider;

    public void AjouterBienfait(Bienfait b)
    {
        if (b != null)
        {
            b.Appliquer(this);
            bienfaitsActifs.Add(b);
            Debug.Log($"Bienfait ajouté : {b.nom} (Permanent: {b.estPermanent})");
            if (!historiqueDesBienfaits.Contains(b.nom))
            {
                historiqueDesBienfaits.Add(b.nom);
            }
        }

        if (!historiqueDesBienfaits.Contains(b.nom))
        {
            historiqueDesBienfaits.Add(b.nom);

            // --- AJOUT POUR L'UI PERMANENTE ---
            // On cherche le HUD dans les enfants (le Canvas)
            HUDPermanent hud = GetComponentInChildren<HUDPermanent>();
            if (hud != null)
            {
                hud.AjouterIconeAuHUD(b.icone);
            }
            // ----------------------------------
        }
    }


    public void Mourir()
    {
        Debug.Log("Mort du joueur. Nettoyage des bienfaits temporaires...");

        // On boucle à l'envers (i--) car on va supprimer des éléments de la liste
        for (int i = bienfaitsActifs.Count - 1; i >= 0; i--)
        {
            Bienfait b = bienfaitsActifs[i];

            // Si le bienfait est TEMPORAIRE (estPermanent == false)
            if (!b.estPermanent)
            {
                // 1. On annule ses effets sur les stats
                b.Retirer(this);

                // 2. On le supprime de la liste
                bienfaitsActifs.RemoveAt(i);
            }
        }

        // Ici : Code pour recharger la scène du Hub (Hades) ou Respawn            _  _  _ 
        // UnityEngine.SceneManagement.SceneManager.LoadScene("01_Hades_Start");  /!\/!\/!\
    }




    // Start is called before the first frame update
    void Start()
    {
        // On récupère le script qui gère les déplacements (situé sur Locomotion System ou ici)
        // On cherche dans les enfants au cas où il serait sur "Locomotion System"
        moveProvider = GetComponentInChildren<ActionBasedContinuousMoveProvider>();

        if (moveProvider != null)
        {
            // On initialise la vitesse réelle du jeu avec ta variable
            moveProvider.moveSpeed = vitesseDeplacement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (invincible)
        {
            reductionDegats = 100;
            if (Time.time - debut_invincibilite > duree_invincibilite)
            {
                invincible = false;
                reductionDegats = 0;  // c'est en pourcentage. Par simplicite, on perds un bienfait qui ameliorerait cet attribut (...?)
            }
        }    
        
        // Evenement d'acquisition d'un bienfait : 
/*
        AjouterBienfait(b);
*/
    }
}
