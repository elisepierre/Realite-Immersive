using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/RalentissementEnnemis")]
public class BienfaitRalentissementEnnemis : Bienfait
{
    [Range(0.1f, 1f)]
    public float multiplicateur = 0.2f; // 50% de vitesse par défaut

    public override void Appliquer(Player joueur)
    {
        joueur.multiplicateurVitesseEnnemis = multiplicateur;
        Debug.Log($"Bienfait appliqué : {nom} -> ennemis ralentis à {multiplicateur * 100}%");
    }

    public override void Retirer(Player joueur)
    {
        if (!estPermanent)
            joueur.multiplicateurVitesseEnnemis = 1f;
    }
}
