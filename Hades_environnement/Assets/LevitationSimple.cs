using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationSimple : MonoBehaviour
{
    [Header("Réglages")]
    public float amplitude = 0.2f; // De combien il monte/descend (en mètres)
    public float vitesse = 2f;     // À quelle vitesse il fait l'aller-retour

    private Vector3 positionDepart;

    void Start()
    {
        // On mémorise où il est au début pour qu'il oscille autour de ce point
        positionDepart = transform.position;
    }

    void Update()
    {
        // On calcule la nouvelle hauteur
        // Mathf.Sin crée une vague entre -1 et 1
        float newY = positionDepart.y + Mathf.Sin(Time.time * vitesse) * amplitude;

        // On applique la position (on garde X et Z, on change juste Y)
        transform.position = new Vector3(positionDepart.x, newY, positionDepart.z);
    }
}