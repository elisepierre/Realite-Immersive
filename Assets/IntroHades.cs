using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IntroNarrative : MonoBehaviour
{
    [System.Serializable]
    public class Etape
    {
        public Sprite image;      // L'image à afficher
        public AudioClip audio;   // Le son à jouer
    }

    [Header("Configuration")]
    public GameObject canvasIntro; // Le Canvas entier (pour l'afficher/cacher)
    public Image imageAfficheur;   // L'Image à l'intérieur du Canvas
    public AudioSource sourceAudio;

    [Header("Réglages")]
    public float pauseEntrePhrases = 0.5f;
    public List<Etape> scenario; // Ta liste de phrases

    void Start()
    {
        StartCoroutine(JouerIntro());
    }

    IEnumerator JouerIntro()
    {
        // 1. On affiche le Canvas au début
        if (canvasIntro != null) canvasIntro.SetActive(true);

        // 2. On joue toute la liste
        foreach (Etape etape in scenario)
        {
            // Changer l'image
            if (imageAfficheur != null && etape.image != null)
            {
                imageAfficheur.sprite = etape.image;
                // imageAfficheur.SetNativeSize(); // Optionnel : adapter la taille
            }

            // Jouer le son
            float duree = 2.0f; // Temps par défaut si pas de son

            if (sourceAudio != null && etape.audio != null)
            {
                sourceAudio.clip = etape.audio;
                sourceAudio.Play();
                duree = etape.audio.length; // Durée exacte du fichier son
            }

            // Attendre la fin du son
            yield return new WaitForSeconds(duree + pauseEntrePhrases);
        }

        // 3. C'est fini : On cache le Canvas
        if (canvasIntro != null) canvasIntro.SetActive(false);
        
        // Optionnel : On détruit l'objet Intro pour faire propre
        Destroy(gameObject); 
    }
}