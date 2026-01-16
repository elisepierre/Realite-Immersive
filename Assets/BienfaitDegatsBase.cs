using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusDegatsBase")]
public class BienfaitDegatsBase : Bienfait
{
    public float bonusDegatsBase = 5;
    public override void Appliquer(Player joueur)
    {
        // Ajoute la valeur de 'bonusDegats' aux dégâts de base
        joueur.degatsBase += bonusDegatsBase;
    }

    public override void Retirer(Player joueur)
    {
        // Retire exactement ce qu'on a ajouté
        joueur.degatsBase -= bonusDegatsBase;
    }
}