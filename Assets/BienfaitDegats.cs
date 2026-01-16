using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusDegats")]
public class BienfaitDegats : Bienfait
{

    public override void Appliquer(Player joueur)
    {
        joueur.degatsBase += joueur.bonusDegats;
    }
}