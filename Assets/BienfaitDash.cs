using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bienfaits/Dash")]
public class BienfaitDash : Bienfait
{
    public override void Appliquer(Player joueur)
    {
        joueur.nombreDash += 3;
    }
}
