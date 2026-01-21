using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusDefense")]
public class BienfaitDefense : Bienfait
{
    public float reductionAjoutee = 5; // On met la valeur ici pour être propre

    public override void Appliquer(Player joueur)
    {
        // Ajoute la valeur de 'bonusDegats' aux dégâts de base
        joueur.reductionDegats += reductionAjoutee;
    }

    public override void Retirer(Player joueur)
    {
        // Effet Passif (Armure) : On augmente la résistance
        joueur.reductionDegats += reductionAjoutee;
    }
}