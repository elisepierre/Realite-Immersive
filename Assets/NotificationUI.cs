using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NotificationUI : MonoBehaviour
{
    [Header("Références UI")]
    public TextMeshProUGUI messageText;
    public Image iconImage;

    [Header("Réglages")]
    public float dureeAffichage = 3.0f;

    // Cette fonction est appelée par la Porte ou le Player
    public void AfficherMessage(string message, Sprite icone = null)
    {
        // 1. On remplit les infos
        if (messageText != null) messageText.text = message;

        if (iconImage != null && icone != null)
        {
            iconImage.sprite = icone;
            iconImage.gameObject.SetActive(true);
        }
        else if (iconImage != null)
        {
            // Si pas d'icône fournie, on cache l'image pour ne pas avoir un carré blanc
            iconImage.gameObject.SetActive(false);
        }

        // 2. On s'assure que le Panel est visible (au cas où il était éteint)
        this.gameObject.SetActive(true);

        // 3. On arrête les anciens comptes à rebours s'il y en avait
        StopAllCoroutines();

        // 4. On lance le nouveau compte à rebours
        StartCoroutine(CacherApresDelai());
    }

    IEnumerator CacherApresDelai()
    {
        // On attend X secondes
        yield return new WaitForSeconds(dureeAffichage);

        // On éteint l'objet sur lequel ce script est posé (Le Panel_Notification)
        // Le Canvas et la BarreBienfaits resteront allumés !
        this.gameObject.SetActive(false);
    }
}