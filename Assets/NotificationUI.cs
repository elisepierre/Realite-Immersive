using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // <-- TRES IMPORTANT pour gérer les Images UI

public class NotificationUI : MonoBehaviour
{
    [Header("Elements UI")]
    public TextMeshProUGUI texteAffiche;
    public Image imageIcone; // <-- NOUVELLE VARIABLE pour l'image

    [Header("Réglages")]
    public float dureeAffichage = 3.0f;
    private Canvas canvasParent;

    void Start()
    {
        canvasParent = GetComponent<Canvas>();
        CacherMessage();
    }

    // LA FONCTION CHANGE : Elle demande maintenant un Sprite en plus du message
    public void AfficherMessage(string message, Sprite nouvelleIcone)
    {
        StopAllCoroutines(); // Sécurité si on enchaîne deux messages vite
        StartCoroutine(RoutineMessage(message, nouvelleIcone));
    }

    IEnumerator RoutineMessage(string message, Sprite nouvelleIcone)
    {
        // 1. Mise à jour du contenu
        if (texteAffiche != null) texteAffiche.text = message;

        if (imageIcone != null)
        {
            imageIcone.sprite = nouvelleIcone;
            // Si le bienfait n'a pas d'icône, on cache l'image pour ne pas avoir un carré blanc moche
            imageIcone.gameObject.SetActive(nouvelleIcone != null);
        }

        // 2. Affichage
        if (canvasParent != null) canvasParent.enabled = true;

        // 3. Attente
        yield return new WaitForSeconds(dureeAffichage);

        // 4. On cache
        CacherMessage();
    }

    private void CacherMessage()
    {
        if (canvasParent != null) canvasParent.enabled = false;
    }
}