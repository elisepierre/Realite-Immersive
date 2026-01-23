using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VieJoueur : MonoBehaviour
{
    public int pointsDeVieMax = 100;
    private int vieActuelle;

    [Header("Feedback")]
    public AudioSource sonDouleur;
    public CanvasGroup ecranRouge; 

    [Header("Menu de Défaite VR")]
    public MenuVictoire scriptMenuDefaite; 

    private bool estMort = false;

    void Start()
    {
        vieActuelle = pointsDeVieMax;
    }

    public void RecevoirDegats(int degats)
    {
        if (estMort) return; // Empêche de mourir plusieurs fois

        vieActuelle -= degats;
        
        // Lancer le flash rouge
        if (ecranRouge != null)
        {
            StopAllCoroutines(); 
            StartCoroutine(FlashRouge());
        }

        if (sonDouleur != null) sonDouleur.Play();

        if (vieActuelle <= 0)
        {
            Mourir();
        }
    }

    IEnumerator FlashRouge()
    {
        ecranRouge.alpha = 0.8f;
        yield return new WaitForSeconds(0.1f);

        while (ecranRouge.alpha > 0)
        {
            ecranRouge.alpha -= Time.deltaTime * 2; 
            yield return null;
        }
        
        ecranRouge.alpha = 0;
    }

    void Mourir()
    {
        if (estMort) return;
        estMort = true;

        // Au lieu de charger la scene, on affiche le menu puis appuy bouton pour recommencer
        if (scriptMenuDefaite != null)
        {
            scriptMenuDefaite.ApparaitreDevantJoueur();
        }
        else
        {
            // au cas ou
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}