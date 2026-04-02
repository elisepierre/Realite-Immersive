using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float vitesse = 2f;
    public float dureeDeVie = 1f;

    void Start()
    {
        // Détruit l'objet automatiquement après 1 seconde
        Destroy(gameObject, dureeDeVie);

        // Oriente le texte vers la caméra (pour que ce soit lisible en VR)
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Corrige l'effet miroir
    }

    void Update()
    {
        // Fait monter le texte
        transform.Translate(Vector3.up * vitesse * Time.deltaTime);
    }
}