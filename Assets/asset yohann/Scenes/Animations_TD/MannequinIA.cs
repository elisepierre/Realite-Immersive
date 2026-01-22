using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MannequinIA : MonoBehaviour
{
    [Header("Liens Indispensables")]
    public Animator leCerveau;
    public Transform leJoueur;
    public Slider barreDeVie;
    public GameObject texteDegatPrefab;

    [Header("Réglages Distances")]
    public float distanceVision = 35.0f;
    // J'ai retiré distanceAttaque car c'est l'arme qui décide de la portée maintenant

    [Header("Réglages Combat & Vie")]
    public float maxHealth = 100f; // J'ai augmenté la vie pour tester les gros dégâts
    private float currentHealth;
    
    [Header("Résistances (0.5 = Résistant, 2.0 = Faible)")]
    public float resEpee = 1.0f;     // Normal
    public float resLance = 1.5f;    // Faible contre les lances
    public float resBouclier = 0.5f; // Résistant aux coups de bouclier

    // Pour la régénération
    private bool estEnRegeneration = false;
    private Rigidbody rb;

    void Start()
    {
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
        // 1. RÉGÉNÉRATION
        if (estEnRegeneration)
        {
            RegenererVie();
            return;
        }

        // 2. LOGIQUE DE MOUVEMENT (Regarder le joueur)
        float distance = Vector3.Distance(transform.position, leJoueur.position);
        if (distance < distanceVision)
        {
            leCerveau.SetBool("IsGuarding", true);
            Vector3 positionCible = new Vector3(leJoueur.position.x, transform.position.y, leJoueur.position.z);
            transform.LookAt(positionCible);
        }
        else
        {
            leCerveau.SetBool("IsGuarding", false);
        }
        
        // NOTE : J'ai supprimé la partie "Distance < Attaque" ici.
        // C'est maintenant le script de l'épée qui déclenche les dégâts lors du choc.
    }

    // Cette fonction est appelée par le script de l'arme (VRWeapon)
    public void PrendreDegats(int degatsBruts, WeaponType typeArme)
    {
        if (estEnRegeneration) return; // On ne tape pas une ambulance

        // A. Calcul des résistances
        float multiplicateur = 1.0f;
        switch (typeArme)
        {
            case WeaponType.Epee: multiplicateur = resEpee; break;
            case WeaponType.Lance: multiplicateur = resLance; break;
            case WeaponType.Bouclier: multiplicateur = resBouclier; break;
        }

        int degatsFinaux = Mathf.RoundToInt(degatsBruts * multiplicateur);

        // B. Appliquer les dégâts
        currentHealth -= degatsFinaux;
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        // C. Animation et Feedback
        leCerveau.SetTrigger("GetHit");
        Debug.Log($"Aïe ! Reçu {degatsFinaux} dégâts ({typeArme}). Vie: {currentHealth}");

        // D. Pop-up de dégâts
        if(texteDegatPrefab != null)
        {
            Vector3 positionPopUp = transform.position + Vector3.up * 2f; 
            GameObject popup = Instantiate(texteDegatPrefab, positionPopUp, Quaternion.identity);
            
            // Si ton prefab a un composant TextMeshPro, on met le chiffre à jour
            TMP_Text tmp = popup.GetComponentInChildren<TMP_Text>();
            if(tmp != null) tmp.text = degatsFinaux.ToString();
        }

        // E. Recul physique
        /*
        if(rb != null)
        {
            Vector3 directionRecul = (transform.position - leJoueur.position).normalized;
            rb.AddForce(directionRecul * 3f, ForceMode.Impulse);
        }
        */
        // F. Mort / KO
        if (currentHealth <= 0)
        {
            estEnRegeneration = true;
            leCerveau.SetBool("IsGuarding", false);
            // Optionnel : Jouer anim de mort ici
        }
    }

    void RegenererVie()
    {
        currentHealth += Time.deltaTime * 20; // Remonte vite pour le test
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            estEnRegeneration = false;
            //leCerveau.SetTrigger("Revive"); // Si tu as une anim de réveil
        }
    }
}