using UnityEngine;
using UnityEngine.UI; // Nécessaire pour la Barre de Vie
using TMPro;          // Nécessaire si votre texte utilise TextMeshPro

public class MannequinIA : MonoBehaviour
{
    [Header("Liens Indispensables")]
    public Animator leCerveau;      
    public Transform leJoueur;
    public Slider barreDeVie;           // <--- NOUVEAU : Glissez le Slider ici
    public GameObject texteDegatPrefab; // <--- NOUVEAU : Glissez le Prefab DamagePopup ici

    [Header("Réglages Distances")]
    public float distanceVision = 35.0f;
    public float distanceAttaque = 8f;

    [Header("Réglages Combat & Vie")]
    public float maxHealth = 5f;        // Vie totale
    private float currentHealth;
    public float delaiEntreCoups = 1.0f;
    private float tempsDernierCoup = 0f;
    
    // Pour la régénération
    private bool estEnRegeneration = false;
    private Rigidbody rb; // Pour le recul physique

    void Start()
    {
        // Initialisation de la vie
        currentHealth = maxHealth;
        if(barreDeVie != null) 
        {
            barreDeVie.maxValue = maxHealth;
            barreDeVie.value = currentHealth;
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- 1. GESTION DE LA RÉGÉNÉRATION (Prioritaire) ---
        if (estEnRegeneration)
        {
            RegenererVie();
            return; // On arrête le script ici tant qu'il récupère
        }

        // --- 2. LOGIQUE DE MOUVEMENT (Votre code original) ---
        float distance = Vector3.Distance(transform.position, leJoueur.position);

        if (distance < distanceVision)
        {
            leCerveau.SetBool("IsGuarding", true);
            // On fige Y pour qu'il ne penche pas la tête
            Vector3 positionCible = new Vector3(leJoueur.position.x, transform.position.y, leJoueur.position.z);
            transform.LookAt(positionCible);
        }
        else
        {
            leCerveau.SetBool("IsGuarding", false);
        }

        // --- 3. DÉTECTION DE COUP (Votre code + Dégâts) ---
        if (distance < distanceAttaque && Time.time > tempsDernierCoup + delaiEntreCoups)
        {
            RecevoirCoup(); // J'ai déplacé le code dans une fonction propre en bas
        }
    }

    // Fonction qui regroupe tout ce qui se passe quand il a mal
    void RecevoirCoup()
    {
        // A. Animation et Chrono
        leCerveau.SetTrigger("GetHit");
        tempsDernierCoup = Time.time;
        Debug.Log("Paf ! Coup reçu.");

        // B. Baisser la vie
        currentHealth -= 1;
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        // C. Faire apparaître le texte (Pop-up)
        if(texteDegatPrefab != null)
        {
            // On le fait apparaître un peu au dessus de la tête (+10m en hauteur)
            Vector3 positionPopUp = transform.position+ Vector3.up * 10f; 
            Instantiate(texteDegatPrefab, positionPopUp, Quaternion.identity);
        }

        // D. Petit recul physique (Knockback)
        if(rb != null)
        {
            Vector3 directionRecul = (transform.position - leJoueur.position).normalized;
            rb.AddForce(directionRecul * 2f, ForceMode.Impulse);
        }

        // E. Vérifier la mort (KO)
        if (currentHealth <= 0)
        {
            estEnRegeneration = true;
            leCerveau.SetBool("IsGuarding", false);
        }
    }

    void RegenererVie()
    {
        currentHealth += Time.deltaTime * 2; // Remonte doucement
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            estEnRegeneration = false;
        }
    }
}