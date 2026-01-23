using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Nécessaire pour les Coroutines

public class VieJoueur : MonoBehaviour
{
    public int pointsDeVieMax = 100;
    private int vieActuelle;

    [Header("Feedback")]
    public AudioSource sonDouleur;
    public CanvasGroup ecranRouge; // GLISSE L'IMAGE AVEC LE CANVAS GROUP ICI

    void Start()
    {
        vieActuelle = pointsDeVieMax;
    }

    public void RecevoirDegats(int degats)
    {
        vieActuelle -= degats;
        
        // Lancer le flash rouge
        if (ecranRouge != null)
        {
            StopAllCoroutines(); // Arrête le flash précédent s'il y en a un
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
        // 1. On met l'alpha à 0.8 (Rouge visible)
        ecranRouge.alpha = 0.8f;
        
        // 2. On attend une fraction de seconde
        yield return new WaitForSeconds(0.1f);

        // 3. On fait disparaitre le rouge doucement
        while (ecranRouge.alpha > 0)
        {
            ecranRouge.alpha -= Time.deltaTime * 2; // Vitesse de disparition
            yield return null;
        }
        
        // 4. Sécurité pour être sûr qu'il est invisible
        ecranRouge.alpha = 0;
    }

    void Mourir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}