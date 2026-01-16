using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/Dash")]
public class BienfaitDash : Bienfait
{
    public int dashAjoutes = 3; // Je te conseille de mettre une variable pour pouvoir changer le nombre dans l'inspecteur

    public override void Appliquer(Player joueur)
    {
        joueur.nombreDash += dashAjoutes;
    }

    public override void Retirer(Player joueur)
    {
        joueur.nombreDash -= dashAjoutes;
    }
}