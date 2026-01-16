using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPieces : MonoBehaviour
{
    [Header("Configuration Scène")]
    [Tooltip("Nom exact de la scène à charger")]
    [SerializeField] private string sceneName;

    [Tooltip("Temps d'attente avant le chargement")]
    [SerializeField] private float waitDuration = 1.0f;

    [Header("Conditions de Sortie")]
    [Tooltip("Si coché, la porte ne s'ouvre que s'il n'y a plus d'ennemis")]
    public bool doitEliminerEnnemis = true;

    private bool estEnTransition = false;

    // --- METHODE 1 : Clic (XR Simple Interactable) ---
    public void ChangerDeScene()
    {
        TenterTransition();
    }

    // --- METHODE 2 : Traversée (Trigger) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !estEnTransition)
        {
            TenterTransition();
        }
    }

    // --- LOGIQUE DE VÉRIFICATION ---
    private void TenterTransition()
    {
        if (estEnTransition) return;

        // VERIFICATION : Reste-t-il des ennemis ?
        if (doitEliminerEnnemis)
        {
            // On cherche tous les objets actifs qui ont le tag "Enemy"
            GameObject[] ennemisRestants = GameObject.FindGameObjectsWithTag("Enemy");

            if (ennemisRestants.Length > 0)
            {
                Debug.Log("Porte fermée ! Il reste " + ennemisRestants.Length + " ennemis.");

                // Ici, tu pourrais jouer un son "Bruit de porte verrouillée"
                // ou faire clignoter la porte en rouge
                return; // On arrête tout, on ne lance pas la transition
            }
        }

        // Si on arrive ici, c'est que la voie est libre
        StartCoroutine(TransitionRoutine());
    }

    IEnumerator TransitionRoutine()
    {
        estEnTransition = true;
        Debug.Log("Transition autorisée. Chargement de : " + sceneName);

        // TODO : Lancer le Fade Out ici

        yield return new WaitForSeconds(waitDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f) asyncLoad.allowSceneActivation = true;
            yield return null;
        }
    }
}