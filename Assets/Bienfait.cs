using UnityEngine;

public abstract class Bienfait : ScriptableObject
{
    public string nom;
    [TextArea] public string description;

    [Tooltip("Coche cette case si le bonus doit rester après la mort (Roguelite progression).")]
    public bool estPermanent = false;

    // Appelé quand on ramasse l'objet
    public abstract void Appliquer(Player joueur);

    // Appelé quand on meurt (pour nettoyer les stats)
    public abstract void Retirer(Player joueur);
}