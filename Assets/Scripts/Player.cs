using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats Globales")]
    public float multiplicateurVitesseEnnemis = 1.0f; // 1 = vitesse normale, 0.5 = moitié vitesse
    public float multiplicateurEnnemis = 1.0f; // pour d'autres effets si nécessaire

    [Header("Vie et dégâts")]
    public float vieMax = 100;
    public float vie;
    public float degatsBase = 5;
    public float bonusDegats = 20;

    [Header("Vitesse et Dash")]
    public float vitesseDeplacement = 10f;
    public float bonusVitesse = 10f;
    public float cooldownDash = 10f;
    public int nombreDash = 0;

    [Header("Bienfaits")]
    public List<Bienfait> bienfaitsActifs = new List<Bienfait>();
    public static List<string> historiqueDesBienfaits = new List<string>();

    private void Awake()
    {
        vie = vieMax;
    }

    public void AjouterBienfait(Bienfait b)
    {
        if (b == null) return;

        b.Appliquer(this);
        bienfaitsActifs.Add(b);

        Debug.Log($"Bienfait ajouté : {b.nom} (Permanent: {b.estPermanent})");

        if (!historiqueDesBienfaits.Contains(b.nom))
            historiqueDesBienfaits.Add(b.nom);
    }

    public void Mourir()
    {
        Debug.Log("Mort du joueur. Nettoyage des bienfaits temporaires...");

        for (int i = bienfaitsActifs.Count - 1; i >= 0; i--)
        {
            Bienfait b = bienfaitsActifs[i];
            if (!b.estPermanent)
            {
                b.Retirer(this);
                bienfaitsActifs.RemoveAt(i);
            }
        }

        vie = vieMax;

        // Ici tu peux ajouter un code pour re-spawner le joueur ou recharger la scène
    }
}
