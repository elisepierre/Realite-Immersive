using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ELEMENT DE BASE
    public float vie = 100;
    public float degatsBase = 5;
    public float bonusDegats = 20;

    // VIE //
    public float vieMax = 100;
    public float reductionDegats = 0; // en %
    public bool invincible = false;
    public float duree_invincibilite = 20;
    public float debut_invincibilite;

    // DASH //
    public int nombreDash = 0;
    public float cooldownDash = 10;
    public float vitesseDeplacement = 10;

    public List<Bienfait> bienfaitsActifs = new List<Bienfait>();


    public void AjouterBienfait(Bienfait b)
    {
        b.Appliquer(this);
        bienfaitsActifs.Add(b);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (invincible)
        {
            if (Time.time - debut_invincibilite > duree_invincibilite)
            {
                invincible = false;
            }
        }    
        
        // Evenement d'acquisition d'un bienfait : 
/*
        AjouterBienfait(b);
*/
    }
}
