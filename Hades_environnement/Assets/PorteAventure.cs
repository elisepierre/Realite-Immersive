using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PorteAventure : MonoBehaviour
{
    [Header("Réglages Porte")]
    public float hauteurOuverture = 3f;
    public float vitesse = 2f;
    public GameObject texteAventure;

    private bool estEnOuverture = false;
    private Vector3 positionFinale;

    void Start()
    {
        positionFinale = transform.position + Vector3.up * hauteurOuverture;
    }

    void Update()
    {
        if (estEnOuverture)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionFinale, vitesse * Time.deltaTime);
            if (transform.position == positionFinale) enabled = false; 
        }
    }

    // --- CETTE FONCTION SERT POUR LA SOURIS ---
    private void OnMouseDown()
    {
        LancerAventure();
    }
    // ------------------------------------------

    public void LancerAventure()
    {
        if (!estEnOuverture)
        {
            estEnOuverture = true;
            if (texteAventure != null) texteAventure.SetActive(false);
        }
    }
}