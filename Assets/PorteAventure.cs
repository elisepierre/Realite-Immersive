using UnityEngine;
using UnityEngine.SceneManagement;

public class PorteTriggerFixe : MonoBehaviour
{
    [Header("Configuration")]
    public Transform visuelPorte; // GLISSE L'ENFANT (La porte 3D) ICI
    public string nomSceneDestination;

    [Header("Animation")]
    public float hauteurOuverture = 3f;
    public float vitesse = 2f;

    private bool estOuvert = false;
    private bool chargementEnCours = false;
    private Vector3 positionFinale;

    void Start()
    {
        // On calcule la position finale de la porte VISUELLE
        if(visuelPorte != null)
        {
            positionFinale = visuelPorte.position + Vector3.up * hauteurOuverture;
        }
        else
        {
            Debug.LogError("OUBLI : Tu n'as pas glissé la porte 3D dans la case 'Visuel Porte' !");
        }
    }

    void Update()
    {
        if (estOuvert && visuelPorte != null)
        {
            // ON BOUGE SEULEMENT LE VISUEL, LE TRIGGER RESTE AU SOL
            visuelPorte.position = Vector3.MoveTowards(visuelPorte.position, positionFinale, vitesse * Time.deltaTime);
        }
    }

    // Appelé par le bouton VR
    public void LancerAventure()
    {
        if (!estOuvert)
        {
            Debug.Log("Ouverture...");
            estOuvert = true;
        }
    }

    // Détection du passage
    private void OnTriggerStay(Collider other)
    {
        // Si c'est le joueur ET que la porte est activée
        if (other.CompareTag("Player") && estOuvert)
        {
            if (!chargementEnCours)
            {
                Debug.Log("TP ACTIVÉ !");
                chargementEnCours = true;
                SceneManager.LoadScene(nomSceneDestination);
            }
        }
    }
}