using UnityEngine;

public class EquipementMain : MonoBehaviour
{
    [Header("Les Objets")]
    public GameObject mainNue;
    public GameObject gantBoxe;

    [Header("Configuration")]
    public bool estMainGauche; // Cochez ceci sur le script du contrôleur GAUCHE

    private bool gantEstEquipe = false;

    void Start()
    {
        // Au départ, on n'a pas de gant
        ForcerMainNue();
    }

    // Cette fonction sera appelée par le gant au sol quand on le ramasse
    public void EquiperLeGant()
    {
        gantEstEquipe = true;
        mainNue.SetActive(false);
        gantBoxe.SetActive(true); // Le gant apparaît sur la main
    }

    public void ForcerMainNue()
    {
        gantEstEquipe = false;
        mainNue.SetActive(true);
        gantBoxe.SetActive(false);
    }
}