/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On définit les types d'armes possibles
public enum WeaponType { Epee, Lance, Bouclier }

public class VRWeapon : MonoBehaviour
{
    [Header("Configuration de l'Arme")]
    public WeaponType typeArme;
    public int degatsDeBase = 20;

    [Header("Réglages Physiques")]
    // Vitesse min pour compter comme un coup (évite de faire des dégâts en effleurant)
    public float vitesseMinPourImpact = 0.5f; 

    private void OnCollisionEnter(Collision collision)
        {
            // Debug pour voir si Unity détecte le choc
            Debug.Log("CHOC PHYSIQUE détecté avec : " + collision.gameObject.name);

            MannequinIA cible = collision.gameObject.GetComponent<MannequinIA>();
            if (cible == null) cible = collision.gameObject.GetComponentInParent<MannequinIA>();

            if (cible != null)
            {
                // J'ai SUPPRIMÉ la condition de vitesse et de cooldown pour le test
                // if (collision.relativeVelocity.magnitude >= vitesseMinPourImpact) ...
                
                Debug.Log("Script Mannequin TROUVÉ ! Envoi des dégâts...");
                CalculerEtEnvoyerDegats(cible);
            }
            else
            {
                Debug.Log("ERREUR : Je touche quelque chose, mais ce n'est pas le Mannequin (Pas de script trouvé).");
            }
        }
    void CalculerEtEnvoyerDegats(MannequinIA cible)
    {
        // Formule : Dégâts de l'arme + Force du joueur
        int forceJoueur = PlayerStats.Instance != null ? PlayerStats.Instance.force : 0;
        int degatsTotal = degatsDeBase + forceJoueur;

        // On envoie l'info au mannequin
        cible.PrendreDegats(degatsTotal, typeArme);
        
        Debug.Log($"Coup porté avec {typeArme} ! Dégâts : {degatsTotal}");
    }
}*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Epee, Lance, Bouclier }

public class VRWeapon : MonoBehaviour
{
    [Header("Configuration de l'Arme")]
    public WeaponType typeArme;
    public int degatsDeBase = 20;

    [Header("Réglages Physiques")]
    public float vitesseMinPourImpact = 0.5f;

    [Header("Audio")] 
    public AudioSource sourceAudio; // Glisse le composant AudioSource ici
    public AudioClip sonImpact;     // Glisse ton fichier son ici

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("CHOC PHYSIQUE détecté avec : " + collision.gameObject.name);

        if (collision.gameObject.name == "HadesBossFIght") {
            JouerSonImpact();
        }

       

        MannequinIA cible = collision.gameObject.GetComponent<MannequinIA>();
        if (cible == null) cible = collision.gameObject.GetComponentInParent<MannequinIA>();

        if (cible != null)
        {
            // J'ai SUPPRIMÉ la condition de vitesse pour le test

            Debug.Log("Script Mannequin TROUVÉ ! Envoi des dégâts...");

            // --- NOUVEAU : JOUER LE SON ---
            JouerSonImpact();
            // ------------------------------

            CalculerEtEnvoyerDegats(cible);
        }
        else
        {
            Debug.Log("ERREUR : Je touche quelque chose, mais ce n'est pas le Mannequin.");
        }
    }

    void CalculerEtEnvoyerDegats(MannequinIA cible)
    {
        int forceJoueur = PlayerStats.Instance != null ? PlayerStats.Instance.force : 0;
        int degatsTotal = degatsDeBase + forceJoueur;

        cible.PrendreDegats(degatsTotal, typeArme);

        Debug.Log($"Coup porté avec {typeArme} ! Dégâts : {degatsTotal}");
    }

    // --- NOUVELLE FONCTION ---
    void JouerSonImpact()
    {
        // On vérifie qu'on a bien assigné le son et la source pour éviter les erreurs
        if (sourceAudio != null && sonImpact != null)
        {
            // PlayOneShot permet de jouer le son sans couper le précédent si on tape vite
            sourceAudio.PlayOneShot(sonImpact);
        }
    }
}