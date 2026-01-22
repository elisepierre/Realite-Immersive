using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Singleton pour accéder aux stats depuis n'importe quelle arme facilement
    public static PlayerStats Instance;

    [Header("Statistiques du Personnage")]
    public int force = 10;      // Augmente les dégâts bruts
    public int precision = 5;   // Pourrait servir pour les coups critiques (optionnel)

    private void Awake()
    {
        // Création du lien unique (Singleton)
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}