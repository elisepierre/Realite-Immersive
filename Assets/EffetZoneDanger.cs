using UnityEngine;

public class EffetZoneDanger : MonoBehaviour
{
    public float dureeDeVie = 1.5f; // Temps avant que la météorite tombe
    private Vector3 tailleInitiale;
    private float tempsEcoule = 0f;

    void Start()
    {
        tailleInitiale = transform.localScale;
    }

    void Update()
    {
        tempsEcoule += Time.deltaTime;

        // Calcul du pourcentage de temps écoulé (de 0 à 1)
        float pourcentage = tempsEcoule / dureeDeVie;

        // On réduit la taille de la tailleInitiale jusqu'à 0
        transform.localScale = Vector3.Lerp(tailleInitiale, Vector3.zero, pourcentage);

        // (Sécurité) Si le temps est fini, on détruit l'objet
        if (tempsEcoule >= dureeDeVie)
        {
            Destroy(gameObject);
        }
    }
}