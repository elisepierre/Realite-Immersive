using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // On crée une instance statique pour pouvoir l'appeler depuis n'importe où
    public static CameraShake instance;

    private Vector3 positionOriginale;
    private Transform objetASecouer;

    void Awake()
    {
        instance = this;
        // En VR, on secoue souvent le XR Origin (le parent de la caméra)
        objetASecouer = transform; 
    }

    public void Secouer(float duree, float force)
    {
        StartCoroutine(ActionSecouer(duree, force));
    }

    IEnumerator ActionSecouer(float duree, float force)
    {
        positionOriginale = objetASecouer.localPosition;
        float tempsEcoule = 0f;

        while (tempsEcoule < duree)
        {
            // On génère une petite vibration aléatoire
            float x = Random.Range(-1f, 1f) * force;
            float y = Random.Range(-1f, 1f) * force;

            objetASecouer.localPosition = positionOriginale + new Vector3(x, y, 0);

            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        // On remet tout en place proprement
        objetASecouer.localPosition = positionOriginale;
    }
}