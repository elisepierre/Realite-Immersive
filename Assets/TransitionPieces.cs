using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPieces : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Nom exact de la scène à charger")]
    [SerializeField] private string sceneName;

    [Tooltip("Temps d'attente avant le chargement (pour le Fade)")]
    [SerializeField] private float waitDuration = 1.0f;

    private bool estEnTransition = false;

    // --- METHODE 1 : Appelée par le XR Simple Interactable (Clic) ---
    public void ChangerDeScene()
    {
        LancerTransition();
    }

    // --- METHODE 2 : Appelée quand on marche dedans ---
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est bien le joueur qui touche la porte, pas un ennemi ou une balle
        if (other.CompareTag("Player") && !estEnTransition)
        {
            Debug.Log("Le joueur a traversé la porte !");
            LancerTransition();
        }
    }

    // --- LOGIQUE COMMUNE ---
    private void LancerTransition()
    {
        if (estEnTransition) return; // Empêche de lancer 2 fois
        estEnTransition = true;

        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        Debug.Log("Début de la transition vers : " + sceneName);

        // TODO : Insérer ici l'appel à ton script de Fade (Fondu au noir)
        // Exemple : FadeScreen.Instance.FadeOut();

        yield return new WaitForSeconds(waitDuration);

        // Chargement Asynchrone
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}