using System.Collections;
using UnityEngine;
using TMPro; // Nécessaire pour TextMeshPro

public class NotificationUI : MonoBehaviour
{
    public TextMeshProUGUI texteAffiche;
    public float dureeAffichage = 3.0f;
    public Canvas canvas; // Pour pouvoir cacher tout le canvas

    void Start()
    {
        // Au démarrage, on cache le texte
        if (canvas != null) canvas.enabled = false;
        if (texteAffiche != null) texteAffiche.text = "";
    }

    public void AfficherMessage(string message)
    {
        // On lance la coroutine (le chrono)
        StartCoroutine(RoutineMessage(message));
    }

    IEnumerator RoutineMessage(string message)
    {
        // 1. On affiche
        if (texteAffiche != null) texteAffiche.text = message;
        if (canvas != null) canvas.enabled = true;

        // 2. On attend
        yield return new WaitForSeconds(dureeAffichage);

        // 3. On cache
        if (canvas != null) canvas.enabled = false;
    }
}