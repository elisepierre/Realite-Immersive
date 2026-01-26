using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/PlusVitesse")]
public class BienfaitVitesse : Bienfait
{
    public override void Appliquer(Player joueur)
    {
        // Ajoute la valeur de 'bonusDegats' aux dégâts de base
        joueur.vitesseDeplacement+= joueur.bonusVitesse;
    }

    public override void Retirer(Player joueur)
    {
        // Retire exactement ce qu'on a ajouté
        joueur.bonusVitesse -= joueur.bonusVitesse;
    }
}