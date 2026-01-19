using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPieces : MonoBehaviour
{
    [Header("Configuration Scène")]
    [Tooltip("Nom exact de la scène à charger")]
    [SerializeField] private string sceneName;

    [Tooltip("Temps d'attente avant le chargement")]
    [SerializeField] private float waitDuration = 3.0f;

    [Header("Conditions de Sortie")]
    [Tooltip("Si coché, la porte ne s'ouvre que s'il n'y a plus d'ennemis")]
    public bool doitEliminerEnnemis = true;

    private bool estEnTransition = false;


    [Header("Récompenses (Loot)")]
    [Tooltip("Cochez si cette porte donne une récompense aléatoire")]
    public bool donneUneRecompense = true;

    [Tooltip("Glissez ici tous les bienfaits que le joueur peut gagner via cette porte")]
    public List<Bienfait> poolDeBienfaits;

    [Header("UI Feedback")]
    // Glisse ici l'objet Canvas qui contient le script NotificationUI
    public NotificationUI notificationSystem;


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

        // 1. Vérification des ennemis
        if (doitEliminerEnnemis)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                Debug.Log("Il reste des ennemis !");
                return;
            }
        }

        // 2. Donner la récompense JUSTE AVANT de partir
        if (donneUneRecompense && poolDeBienfaits.Count > 0)
        {
            DonnerBienfaitAleatoire();
        }

        StartCoroutine(TransitionRoutine());
    }

    private void DonnerBienfaitAleatoire()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player playerScript = playerObj.GetComponent<Player>();

            // --- ETAPE 1 : Filtrer avec la Mémoire Globale ---
            List<Bienfait> candidatsValides = new List<Bienfait>();

            foreach (Bienfait b in poolDeBienfaits)
            {
                // La condition stricte : 
                // Est-ce que ce nom apparaît dans le livre d'histoire ?
                // Si NON, alors on peut le gagner.
                if (!Player.historiqueDesBienfaits.Contains(b.nom))
                {
                    candidatsValides.Add(b);
                }
            }

            // --- ETAPE 2 : Piocher ---
            if (candidatsValides.Count > 0)
            {
                int indexAleatoire = Random.Range(0, candidatsValides.Count);
                Bienfait bienfaitGagne = candidatsValides[indexAleatoire];

                // Pas de visuel, juste des maths !
                playerScript.AjouterBienfait(bienfaitGagne);

                if (notificationSystem != null)
                {
                    notificationSystem.AfficherMessage("Bienfait obtenu :\n" + bienfaitGagne.nom);
                }
            }

            else
            {
                Debug.Log("Le joueur a déjà épuisé tous les bienfaits disponibles dans cette porte !");
            }
        }
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