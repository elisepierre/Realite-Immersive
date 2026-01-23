using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HadesAI : MonoBehaviour
{
    [Header("Démarrage")]
    public float delaiAvantCombat = 21.57f; // Temps d'attente avant de commencer (en secondes)
    
    [Header("Cibles")]
    public Transform joueur;

    [Header("Santé & Dégâts")]
    public int pointsDeVieMax = 100;
    private int vieActuelle;
    private bool estMort = false;
    
    // Noms des Triggers pour la douleur et la mort
    public string animHit = "GetHit";
    public string animDie = "Die";

    [Header("Feedback (Barre de vie & Son)")]
    public Slider barreDeVie;       // Glisse ton Slider ici
    public AudioSource audioSource; // Glisse l'AudioSource de Hades ici
    public AudioClip sonDouleur;    // Glisse le fichier son (WAV/MP3) ici

    [Header("Réglages Combat")]
    public float tempsEntreAttaques = 3f; // Temps de pause après une attaque
    public float distanceAttaque = 2.5f;  // Distance pour taper au corps à corps
    public float vitesseDeplacement = 3.5f;

    [Header("Animations")]
    public Animator animator;
    public string animCoupDroit = "AttackRight";
    public string animCoupDevant = "AttackForward";
    public string animMeteorite = "CastSpell";

    [Header("Météorite")]
    public GameObject meteoritePrefab; 
    public GameObject zoneDangerPrefab; // <--- LE PREFAB DU CERCLE ROUGE
    public float hauteurMeteorite = 10f;

    private bool estEnCombat = false; 
    private bool estEnTrainDAttaquer = false;

    void Start()
    {
        // On remplit la vie au début
        vieActuelle = pointsDeVieMax;

        // Initialisation de la barre de vie
        if (barreDeVie != null)
        {
            barreDeVie.maxValue = pointsDeVieMax;
            barreDeVie.value = vieActuelle;
        }

        if (animator == null) animator = GetComponent<Animator>();

        // --- LA MODIFICATION EST ICI ---
        // On lance le compte à rebours
        StartCoroutine(CompteAReboursDemarrage());
    }

    // Nouvelle fonction qui gère l'attente
    IEnumerator CompteAReboursDemarrage()
    {
        // On s'assure qu'il ne bouge pas au début
        estEnCombat = false; 

        // On attend le temps défini dans l'inspecteur
        yield return new WaitForSeconds(delaiAvantCombat);

        // C'est fini, on lance la bagarre !
        estEnCombat = true;
    }

    void Update()
    {
        if (estMort) return;
        if (!estEnCombat || joueur == null) return;

        RegarderJoueur();

        if (estEnTrainDAttaquer) return;

        float distance = Vector3.Distance(transform.position, joueur.position);

        if (distance > distanceAttaque)
        {
            transform.position = Vector3.MoveTowards(transform.position, joueur.position, vitesseDeplacement * Time.deltaTime);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            StartCoroutine(LancerUneAttaque());
        }
    }

    public void PrendreDegats(int degats)
    {
        if (estMort) return;

        vieActuelle -= degats;
        // Debug.Log("Hades a mal ! Vie restante : " + vieActuelle);

        if (barreDeVie != null) barreDeVie.value = vieActuelle;
        if (audioSource != null && sonDouleur != null) audioSource.PlayOneShot(sonDouleur);

        if (vieActuelle <= 0)
        {
            Mourir();
        }
        else
        {
            animator.SetTrigger(animHit);
            StopAllCoroutines(); 
            estEnTrainDAttaquer = false; 
            animator.ResetTrigger(animCoupDroit);
            animator.ResetTrigger(animCoupDevant);
            animator.ResetTrigger(animMeteorite);
        }
    }

    void Mourir()
    {
        estMort = true;
        estEnCombat = false;
        animator.SetBool("IsWalking", false);
        animator.SetTrigger(animDie);
        
        Collider col = GetComponent<Collider>();
        //if(col != null) col.enabled = false;

        if(barreDeVie != null) barreDeVie.gameObject.SetActive(false);

        Debug.Log("HADES EST VAINCU !");
    }

    public void ActiverCombat()
    {
        estEnCombat = true;
    }

    // --- C'EST ICI QUE TOUT CHANGE POUR LA METEORITE ---
IEnumerator LancerUneAttaque()
    {
        estEnTrainDAttaquer = true;

        int choix = Random.Range(0, 3); 

        if (choix == 0)
        {
            animator.SetTrigger(animCoupDroit);
            yield return new WaitForSeconds(tempsEntreAttaques);
        }
        else if (choix == 1)
        {
            animator.SetTrigger(animCoupDevant);
            yield return new WaitForSeconds(tempsEntreAttaques);
        }
        else
        {
            // --- ATTAQUE SPECIALE : PLUIE DE METEORITES (MODE LENT) ---
            
            animator.SetTrigger(animMeteorite);
            
            // On laisse le temps à l'animation de bien se jouer
            yield return new WaitForSeconds(1.0f);

            // --- REGLAGES DE TEMPS ---
            int nombreMeteorites = 4;
            float rayonAttaque = 8f; 
            float delaiAvantImpact = 4.0f; // <--- ICI : 4 secondes pour fuir ! (Change ce chiffre si tu veux)

            List<Vector3> positionsCibles = new List<Vector3>();

            for (int i = 0; i < nombreMeteorites; i++)
            {
                // Calcul position aléatoire
                Vector2 pointHazard = Random.insideUnitCircle * rayonAttaque;
                Vector3 pos = transform.position + new Vector3(pointHazard.x, 0, pointHazard.y);
                
                // On colle au sol (niveau des pieds de Hades)
                pos.y = transform.position.y + 0.05f; 

                positionsCibles.Add(pos);

                // Apparition Zone Rouge
                if (zoneDangerPrefab != null)
                {
                    GameObject zone = Instantiate(zoneDangerPrefab, pos, Quaternion.Euler(0, 0, 0));
                    
                    // IMPORTANT : On force la zone à rester visible 4 secondes
                    EffetZoneDanger scriptZone = zone.GetComponent<EffetZoneDanger>();
                    if (scriptZone != null)
                    {
                        scriptZone.dureeDeVie = delaiAvantImpact; 
                    }
                }
            }

            // On attend 4 secondes (le joueur court partout !)
            yield return new WaitForSeconds(delaiAvantImpact);

            // LE BOMBARDEMENT
            foreach (Vector3 cible in positionsCibles)
            {
                InvoquerMeteorite(cible);
                // On attend 0.5s entre chaque pierre pour bien sentir les impacts
                yield return new WaitForSeconds(0.5f);
            }
            
            // Pause finale après l'attaque
            yield return new WaitForSeconds(tempsEntreAttaques);
        }

        estEnTrainDAttaquer = false;
    }
    // --- MODIFIEE POUR ACCEPTER UNE POSITION CIBLE ---
void InvoquerMeteorite(Vector3 cibleSol)
    {
        if (meteoritePrefab != null)
        {
            // On garde X et Z de la cible, mais on force Y à 15m de haut
            Vector3 positionSpawn = new Vector3(cibleSol.x, cibleSol.y + 15f, cibleSol.z);
            Instantiate(meteoritePrefab, positionSpawn, Quaternion.identity);
        }
    }
    void RegarderJoueur()
    {
        if(joueur == null) return;
        
        Vector3 direction = (joueur.position - transform.position).normalized;
        direction.y = 0; 
        Quaternion rotationVoulue = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, Time.deltaTime * 5f);
    }
}