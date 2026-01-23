using UnityEngine;
using UnityEngine.UI;

public class HUDPermanent : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject conteneurBarre; // L'objet "BarreBienfaits"
    public GameObject modeleIcone;    // L'objet "ModeleIcone" (éteint)

    public void AjouterIconeAuHUD(Sprite imageBienfait)
    {
        if (imageBienfait == null) return; // Sécurité si pas d'image

        // 1. On crée une copie du modèle
        GameObject nouvelleIcone = Instantiate(modeleIcone, conteneurBarre.transform);

        // 2. On active la copie (car le modèle est éteint)
        nouvelleIcone.SetActive(true);

        // 3. On change l'image
        Image imgComponent = nouvelleIcone.GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = imageBienfait;
        }
    }
}