using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/Vie")]
public class BienfaitVie : Bienfait
{
    public override void Appliquer(Player joueur)
    {
        joueur.invincible = true;
        joueur.debut_invincibilite = Time.time;
        joueur.reductionDegats = 5;// Pourcentage de reduction de l'attaque subit par l'ennemi
    }
}
