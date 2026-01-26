using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/Masus Attaque Ennemis")]
public class BienfaitAttaqueEnnemi : Bienfait
{
    [Tooltip("Pourcentage de malus degat (0.1 = 10%)")]
    public float pourcentage = 0.1f;

    public override void Appliquer(Player joueur)
    {
        // On réduit le multiplicateur (Ex: 1.0 - 0.1 = 0.9)
        joueur.multiplicateurEnnemis -= pourcentage;
        Debug.Log("Malus attaque ennemis de " + (pourcentage * 100) + "%");
    }

    public override void Retirer(Player joueur)
    {
        // On remet le multiplicateur comme avant
        joueur.multiplicateurEnnemis += pourcentage;
    }
}