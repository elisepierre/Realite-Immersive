using UnityEngine;

public abstract class Bienfait : ScriptableObject
{
    public string nom;
    public abstract void Appliquer(Player joueur);
}
