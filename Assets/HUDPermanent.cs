using UnityEngine;
using UnityEngine.UI;

public class HUDPermanent : MonoBehaviour
{
    public GameObject BarreBienfaits;
    public GameObject ModeleIcone;

    public void AjouterIconeAuHUD(Sprite imageBienfait)
    {
        if (imageBienfait == null)
        {
            Debug.LogError("ATTENTION : L'image du bienfait est vide (NULL) !");
            return;
        }

        // 1. Création
        GameObject nouvelleIcone = Instantiate(ModeleIcone, BarreBienfaits.transform);

        // 2. Activation
        nouvelleIcone.SetActive(true);

        // 3. --- CORRECTION ECHELLE ---
        // On force la taille à être normale (1,1,1)
        nouvelleIcone.transform.localScale = Vector3.one;
        // On s'assure qu'il est bien à la profondeur 0 par rapport à la barre
        nouvelleIcone.transform.localPosition = new Vector3(nouvelleIcone.transform.localPosition.x, nouvelleIcone.transform.localPosition.y, 0);

        // 4. Assignation Image
        Image imgComponent = nouvelleIcone.GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = imageBienfait;
            // Optionnel : Force l'image à ne pas être transparente (Alpha = 1)
            Color c = imgComponent.color;
            c.a = 1f;
            imgComponent.color = c;
        }

        Debug.Log("Icône créée et ajoutée au HUD !");
    }
}