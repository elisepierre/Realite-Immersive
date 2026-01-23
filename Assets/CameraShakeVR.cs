using UnityEngine;
using System.Collections;

public class CameraShakeVR : MonoBehaviour
{
    public Transform cameraOffset; // L'objet parent de la caméra
    
    // Singleton pour pouvoir l'appeler facilement depuis n'importe où
    public static CameraShakeVR instance;

    void Awake()
    {
        instance = this;
    }

    public void Secouer(float duree, float force)
    {
        if (cameraOffset != null)
        {
            StartCoroutine(Tremblement(duree, force));
        }
    }

    IEnumerator Tremblement(float duree, float force)
    {
        Vector3 positionOriginale = cameraOffset.localPosition;
        float tempsEcoule = 0f;

        while (tempsEcoule < duree)
        {
            // On génère un petit déplacement aléatoire
            float x = Random.Range(-1f, 1f) * force;
            float y = Random.Range(-1f, 1f) * force;

            // On l'applique au PARENT de la caméra
            cameraOffset.localPosition = positionOriginale + new Vector3(x, y, 0);

            tempsEcoule += Time.deltaTime;
            yield return null;
        }

        // TRES IMPORTANT : On remet tout à zéro à la fin
        cameraOffset.localPosition = positionOriginale;
    }
}