using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionPieces : MonoBehaviour
{
    [Header("Configuration Scène")]
    [Tooltip("Nom exact de la scène à charger")]
    [SerializeField] private string sceneName;

    [Tooltip("Temps d'attente avant le chargement (Laisse le temps de lire le bonus)")]
    [SerializeField] private float waitDuration = 3.0f;

    [Header("Conditions de Sortie")]
    [Tooltip("Si coché, la porte ne s'ouvre que s'il n'y a plus d'ennemis")]
    public bool doitEliminerEnnemis = true;

    private bool estEnTransition = false;

    [Header("Visuel Activation")]
    [Tooltip("Glisse ici l'objet 3D de la porte (celui qui a le MeshRenderer)")]
    public Renderer renduPorte;

    [Header("Récompenses (Loot)")]
    [Tooltip("Cochez si cette porte donne une récompense aléatoire")]
    public bool donneUneRecompense = true;

    [Tooltip("Glissez ici tous les bienfaits que le joueur peut gagner via cette porte")]
    public List<Bienfait> poolDeBienfaits;

    [Tooltip("Choisis un rouge très vif. Coche HDR pour plus d'intensité.")]
    [ColorUsage(true, true)] // Permet de choisir une couleur HDR (Brillante)
    public Color couleurActive = new Color(1f, 0f, 0f, 1f) * 4;

    private bool estAllumee = false;
    private float timerVerification = 0f;

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
                // Optionnel : Jouer un son "Porte Verrouillée" ici
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


    void Start()
    {
        // Si on ne doit pas éliminer d'ennemis, on allume la porte tout de suite
        if (!doitEliminerEnnemis)
        {
            AllumerPorte();
        }
    }

    void Update()
    {
        // Si la porte est déjà allumée ou si on n'a pas besoin de tuer des ennemis, on ne fait rien
        if (estAllumee || !doitEliminerEnnemis) return;

        // OPTIMISATION : On ne vérifie qu'une fois par seconde (pas à chaque image)
        timerVerification += Time.deltaTime;
        if (timerVerification >= 1.0f)
        {
            timerVerification = 0f;
            VerifierEnnemis();
        }
    }

    void VerifierEnnemis()
    {
        // Si aucun ennemi n'est trouvé
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            AllumerPorte();
        }
    }

    void AllumerPorte()
    {
        estAllumee = true;

        if (renduPorte != null)
        {
            // On active l'émission du matériau
            renduPorte.material.EnableKeyword("_EMISSION");
            renduPorte.material.SetColor("_EmissionColor", couleurActive);

            // Petit son ou particule optionnel ici
            Debug.Log("Salle nettoyée ! La porte brille.");
        }
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
                // On vérifie si le joueur a DÉJÀ eu ce bienfait dans l'histoire de sa partie
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

                // Application des stats
                playerScript.AjouterBienfait(bienfaitGagne);

                // --- ETAPE 3 : Affichage UI (Automatique) ---
                // On cherche le script NotificationUI sur le joueur ou ses enfants (Camera)
                NotificationUI uiDuJoueur = playerObj.GetComponentInChildren<NotificationUI>(true);

                if (uiDuJoueur != null)
                {
                    uiDuJoueur.AfficherMessage("Bienfait obtenu :\n" + bienfaitGagne.nom, bienfaitGagne.icone);
                }
                else
                {
                    Debug.LogWarning("Pas de NotificationUI trouvé sur le Player !");
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

        // TODO : Lancer le Fade Out ici (Fondu au noir)

        // On attend que le joueur ait le temps de lire le message
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