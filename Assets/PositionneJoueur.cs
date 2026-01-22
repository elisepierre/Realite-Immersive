using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionneJoueur : MonoBehaviour
{
    void OnEnable()
    {
        // On s'abonne à l'événement "Une scène a fini de charger"
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // On se désabonne pour éviter les erreurs si l'objet est détruit
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cette fonction est appelée automatiquement par Unity à chaque changement de scène
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SePlacerAuSpawn();
    }

    void SePlacerAuSpawn()
    {
        GameObject pointDeSpawn = GameObject.FindGameObjectWithTag("Respawn");

        if (pointDeSpawn != null)
        {
            // Désactiver le Character Controller le temps du téléport (sinon il résiste)
            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // Calcul de position
            Vector3 positionCible = pointDeSpawn.transform.position;
            positionCible.y += 0.05f; // Petite marge de sécurité

            // Téléportation
            transform.position = positionCible;

            // Rotation (On garde Y uniquement)
            Vector3 rotationCible = pointDeSpawn.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotationCible.y, 0);

            // Réactiver le Character Controller
            if (cc != null) cc.enabled = true;

            Debug.Log("Joueur replacé au spawn de la scène : " + SceneManager.GetActiveScene().name);
        }
    }
}