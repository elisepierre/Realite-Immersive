using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttaqueSouris : MonoBehaviour
{
    public float vitesseFrappe = 1000f; // Vitesse de rotation très rapide
    private bool attaqueEnCours = false;
    private Quaternion rotationDepart;

    void Start()
    {
        rotationDepart = transform.localRotation;
    }

    void Update()
    {
        // Quand on clique gauche
        if (Input.GetMouseButtonDown(0) && !attaqueEnCours)
        {
            StartCoroutine(DonnerUnCoup());
        }
    }

    System.Collections.IEnumerator DonnerUnCoup()
    {
        attaqueEnCours = true;

        // 1. On baisse l'épée très vite (pour générer de la vitesse physique)
        float temps = 0;
        while(temps < 0.1f)
        {
            temps += Time.deltaTime;
            // On fait pivoter l'objet sur l'axe X localement
            transform.Rotate(Vector3.right * vitesseFrappe * Time.deltaTime);
            yield return null;
        }

        // 2. On attend un micro instant
        yield return new WaitForSeconds(0.1f);

        // 3. On remet l'épée en place
        transform.localRotation = rotationDepart;
        attaqueEnCours = false;
    }
}