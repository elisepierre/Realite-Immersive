using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GantAuSol : MonoBehaviour
{
    public bool estPourMainGauche = true; // A cocher si c'est le gant gauche

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
            grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // args.interactorObject est la main qui vient d'attraper l'objet
        // On cherche le script "EquipementMain" sur le contrôleur qui attrape
        var mainDuJoueur = args.interactorObject.transform.GetComponentInParent<EquipementMain>();

        if (mainDuJoueur != null)
        {
            // Vérification de sécurité : Est-ce qu'on met le gant gauche sur la main gauche ?
            if (mainDuJoueur.estMainGauche == this.estPourMainGauche)
            {
                // 1. On dit à la main d'afficher son gant
                mainDuJoueur.EquiperLeGant();

                // 2. On détruit cet objet au sol (l'illusion est parfaite)
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("Mauvaise main ! Vous essayez de mettre un gant gauche sur une main droite.");
                // Optionnel : On force le lâcher si c'est la mauvaise main
                // interactionManager.SelectExit(args.interactorObject, grabInteractable); 
            }
        }
    }
}