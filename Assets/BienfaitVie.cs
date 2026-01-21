using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/VieInvincibilite")]
public class BienfaitVie : Bienfait
{


    public override void Appliquer(Player joueur)
    {
        // Effet immédiat (Potion) : On rend invincible maintenant
        joueur.invincible = true;
        joueur.debut_invincibilite = Time.time;

    }

    public override void Retirer(Player joueur)
    {
        // Note : On ne retire pas l'invincibilité ici car c'était un effet instantané lié au temps
    }
}