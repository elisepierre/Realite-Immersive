using UnityEngine;

public abstract class Bienfait : ScriptableObject
{
    [Header("Infos Visuelles")] // <-- Ajout pour faire joli
    public string nom;
    public Sprite icone; // <-- AJOUTE CETTE LIGNE (L'image du bienfait)

    [TextArea] public string description;

    [Header("Règles")]
    public bool estPermanent = false;
    public bool estUnique = true;

    public abstract void Appliquer(Player joueur);
    public abstract void Retirer(Player joueur);
}