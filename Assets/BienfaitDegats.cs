using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusDegats")]
public class BienfaitDegats : Bienfait
{
    
    public override void Appliquer(Player joueur)
    {
        // Ajoute la valeur de 'bonusDegats' aux dégâts de base
        joueur.degatsBase += joueur.bonusDegats;
    }

    public override void Retirer(Player joueur)
    {
        // Retire exactement ce qu'on a ajouté
        joueur.degatsBase -= joueur.bonusDegats;
    }
}