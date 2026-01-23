using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HadesAI : MonoBehaviour
{
    [Header("Démarrage")]
    public float delaiAvantCombat = 21.57f; 
    
    [Header("Cibles")]
    public Transform joueur;

    [Header("Santé & Dégâts")]
    public int pointsDeVieMax = 100;
    private int vieActuelle;
    private bool estMort = false;
    
    public string animHit = "GetHit";
    public string animDie = "Die";

    [Header("Feedback (Barre de vie & Son)")]
    public Slider barreDeVie;
    public AudioSource audioSource;
    public AudioClip sonDouleur;

    [Header("Menu de Victoire")]
    public MenuVictoire scriptMenuVictoire; 

    [Header("Réglages Combat")]
    public float tempsEntreAttaques = 3f; 
    public float distanceAttaque = 2.5f;
    public float vitesseDeplacement = 3.5f;

    [Header("Animations")]
    public Animator animator;
    public string animCoupDroit = "AttackRight";
    public string animCoupDevant = "AttackForward";
    public string animMeteorite = "CastSpell";

    [Header("Météorite")]
    public GameObject meteoritePrefab; 
    public GameObject zoneDangerPrefab; 
    public float hauteurMeteorite = 10f;

    private bool estEnCombat = false; 
    private bool estEnTrainDAttaquer = false;

    void Start()
    {
        vieActuelle = pointsDeVieMax;

        if (barreDeVie != null)
        {
            barreDeVie.maxValue = pointsDeVieMax;
            barreDeVie.value = vieActuelle;
        }

        if (animator == null) animator = GetComponent<Animator>();

        StartCoroutine(CompteAReboursDemarrage());
    }

    IEnumerator CompteAReboursDemarrage()
    {
        estEnCombat = false; 
        yield return new WaitForSeconds(delaiAvantCombat);
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

        if (barreDeVie != null) barreDeVie.value = vieActuelle;
        if (audioSource != null && sonDouleur != null) audioSource.PlayOneShot(sonDouleur);

        if (vieActuelle <= 0)
        {
            Mourir();
        }
        else
        {
            animator.SetTrigger(animHit);
        }
    }

    void Mourir()
    {
        if (estMort) return; // pour pas re mourrir

        estMort = true;
        estEnCombat = false;
        animator.SetBool("IsWalking", false);
        animator.SetTrigger(animDie);
        
        // Cacher la barre de vie
        if(barreDeVie != null) barreDeVie.gameObject.SetActive(false);

        //Appel du menu de victoire devant le joueur ---
        if (scriptMenuVictoire != null)
        {
            scriptMenuVictoire.ApparaitreDevantJoueur();
        }

        Debug.Log("HADES EST VAINCU !");
    }

    public void ActiverCombat()
    {
        estEnCombat = true;
    }

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
            animator.SetTrigger(animMeteorite);
            yield return new WaitForSeconds(1.0f);

            int nombreMeteorites = 4;
            float rayonAttaque = 8f; 
            float delaiAvantImpact = 4.0f; 

            List<Vector3> positionsCibles = new List<Vector3>();

            for (int i = 0; i < nombreMeteorites; i++)
            {
                Vector2 pointHazard = Random.insideUnitCircle * rayonAttaque;
                Vector3 pos = transform.position + new Vector3(pointHazard.x, 0, pointHazard.y);
                pos.y = transform.position.y + 0.05f; 

                positionsCibles.Add(pos);

                if (zoneDangerPrefab != null)
                {
                    GameObject zone = Instantiate(zoneDangerPrefab, pos, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(delaiAvantImpact);

            foreach (Vector3 cible in positionsCibles)
            {
                InvoquerMeteorite(cible);
                yield return new WaitForSeconds(0.5f);
            }
            
            yield return new WaitForSeconds(tempsEntreAttaques);
        }

        estEnTrainDAttaquer = false;
    }

    void InvoquerMeteorite(Vector3 cibleSol)
    {
        if (meteoritePrefab != null)
        {
            Vector3 positionSpawn = new Vector3(cibleSol.x, cibleSol.y + 15f, cibleSol.z);
            Instantiate(meteoritePrefab, positionSpawn, Quaternion.identity);
        }
    }

    void RegarderJoueur()
    {
        if(joueur == null) return;
        
        Vector3 direction = (joueur.position - transform.position).normalized;
        direction.y = 0; 
        if (direction != Vector3.zero)
        {
            Quaternion rotationVoulue = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationVoulue, Time.deltaTime * 5f);
        }
    }
}