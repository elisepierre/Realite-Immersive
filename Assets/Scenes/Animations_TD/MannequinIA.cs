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


    [Header("Réglages Combat & Vie")]
    public float maxHealth = 100f; 
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
        //RÉGÉNÉRATION
        if (estEnRegeneration)
        {
            RegenererVie();
            return;
        }

        //MOUVEMENT
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
        

    }

 
    public void PrendreDegats(int degatsBruts, WeaponType typeArme)
    {
        if (estEnRegeneration) return;

        //Calcul des résistances
        float multiplicateur = 1.0f;
        switch (typeArme)
        {
            case WeaponType.Epee: multiplicateur = resEpee; break;
            case WeaponType.Lance: multiplicateur = resLance; break;
            case WeaponType.Bouclier: multiplicateur = resBouclier; break;
        }

        int degatsFinaux = Mathf.RoundToInt(degatsBruts * multiplicateur);

        // Appliquer les degats
        currentHealth -= degatsFinaux;
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        // C. Animation et Feedback
        leCerveau.SetTrigger("GetHit");
        Debug.Log($"Aïe ! Reçu {degatsFinaux} dégâts ({typeArme}). Vie: {currentHealth}");

        // Pop-up de degats
        if(texteDegatPrefab != null)
        {
            Vector3 positionPopUp = transform.position + Vector3.up * 2f; 
            GameObject popup = Instantiate(texteDegatPrefab, positionPopUp, Quaternion.identity);
            
            
            TMP_Text tmp = popup.GetComponentInChildren<TMP_Text>();
            if(tmp != null) tmp.text = degatsFinaux.ToString();
        }

        //  Mort / KO
        if (currentHealth <= 0)
        {
            estEnRegeneration = true;
            leCerveau.SetBool("IsGuarding", false);
            //anim de mort si je l'ajoute
        }
    }

    void RegenererVie()
    {
        currentHealth += Time.deltaTime * 20;
        if(barreDeVie != null) barreDeVie.value = currentHealth;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            estEnRegeneration = false;
            //leCerveau.SetTrigger("Revive"); // Si je remet l'anim revive
        }
    }
}