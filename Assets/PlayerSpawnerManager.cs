using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Si besoin de manipuler le locomotion system

public class PlayerSpawnManager : MonoBehaviour
{
    void OnEnable()
    {
        // On s'abonne à l'événement de chargement de scène pour se replacer à chaque fois
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Cette fonction est appelée automatiquement à chaque fois qu'une scène finit de charger
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        MovePlayerToSpawnPoint();
    }

    void MovePlayerToSpawnPoint()
    {
        // 1. Trouver l'objet qui a le tag "Respawn"
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");

        if (spawnPoint != null)
        {
            // 2. Déplacer le XR Origin (cet objet) sur le SpawnPoint
            // On désactive le CharacterController momentanément s'il y en a un pour éviter les conflits physiques
            CharacterController charController = GetComponent<CharacterController>();
            if (charController != null) charController.enabled = false;

            // Déplacement et Rotation
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;

            // Réactiver la physique
            if (charController != null) charController.enabled = true;

            Debug.Log("Joueur téléporté au SpawnPoint : " + spawnPoint.name);
        }
        else
        {
            Debug.LogWarning("Aucun objet avec le tag 'Respawn' trouvé dans cette scène !");
        }
    }
}

/*Dans chaque scène (ex: Salle 2) :

    Crée un Empty GameObject.

    Nomme-le SpawnPoint.

    Place-le exactement devant la porte d'entrée, la flèche bleue (Z) pointant vers l'intérieur de la salle.

    Important : En haut à droite de l'inspecteur, assigne-lui le Tag : Respawn. (Ce tag existe par défaut dans Unity).
*/
