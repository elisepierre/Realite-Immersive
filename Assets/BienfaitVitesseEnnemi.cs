using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/Ralentissement Ennemis")]
public class BienfaitRalentissement : Bienfait
{
    [Tooltip("Pourcentage de ralentissement (0.1 = 10%)")]
    public float pourcentage = 0.1f;

    public override void Appliquer(Player joueur)
    {
        // On réduit le multiplicateur (Ex: 1.0 - 0.1 = 0.9)
        joueur.multiplicateurVitesseEnnemis -= pourcentage;
        Debug.Log("Ennemis ralentis de " + (pourcentage * 100) + "%");
    }

    public override void Retirer(Player joueur)
    {
        // On remet le multiplicateur comme avant
        joueur.multiplicateurVitesseEnnemis += pourcentage;
    }
}