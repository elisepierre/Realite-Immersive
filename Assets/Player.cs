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

    [Header("Interface UI")]
    // ON AJOUTE CETTE VARIABLE POUR FAIRE LE LIEN MANUELLEMENT
    public HUDPermanent hudScript;

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
        Debug.Log("ÉTAPE 1 : La fonction AjouterBienfait est appelée !");

        if (b == null)
        {
            Debug.LogError("ERREUR : Le bienfait est vide (NULL) !");
            return;
        }

        Debug.Log("ÉTAPE 2 : Le bienfait est valide : " + b.nom);

        // On applique les stats
        b.Appliquer(this);
        bienfaitsActifs.Add(b);

        // Vérification Historique
        if (!historiqueDesBienfaits.Contains(b.nom))
        {
            Debug.Log("ÉTAPE 3 : C'est un nouveau bienfait, on l'ajoute à l'historique.");
            historiqueDesBienfaits.Add(b.nom);

            // TENTATIVE DE RECUPERATION DU HUD
            if (hudScript == null)
            {
                Debug.LogWarning("ATTENTION : hudScript est vide. Je tente de le trouver tout seul...");
                hudScript = GetComponentInChildren<HUDPermanent>();
            }

            if (hudScript != null)
            {
                Debug.Log("ÉTAPE 4 : HUD trouvé ! J'envoie l'icône.");
                hudScript.AjouterIconeAuHUD(b.icone);
            }
            else
            {
                Debug.LogError("ERREUR FATALE : Impossible de trouver le script 'HUDPermanent' ni manuellement ni automatiquement !");
                Debug.LogError("Vérifie que le Canvas est bien un ENFANT du Player XR Origin.");
            }
        }
        else
        {
            Debug.Log("INFO : Bienfait déjà connu. Pas d'icône.");
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
        historiqueDesBienfaits.Clear();
        Debug.Log("Mémoire des bienfaits effacée pour nouvelle partie.");
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
