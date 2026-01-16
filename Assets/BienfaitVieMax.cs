using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusVieMax")]
public class BienfaitVieMax : Bienfait
{
    public override void Appliquer(Player joueur)
    {
        // Ajoute la valeur de 'bonusDegats' aux dégâts de base
        joueur.vieMax += 50;
    }

    public override void Retirer(Player joueur)
    {
        // Retire exactement ce qu'on a ajouté
        joueur.vieMax -= 50;
    }
}