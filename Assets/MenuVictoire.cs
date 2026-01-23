using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoire : MonoBehaviour
{
    public Transform cameraJoueur; // Glisse ici la "Main Camera" du XR Origin

    public void ApparaitreDevantJoueur()
    {
        // 1. On active le menu
        gameObject.SetActive(true);

        if (cameraJoueur != null)
        {
            // 2. Calculer la position : Position de la tête + 2 mètres vers l'avant
            Vector3 positionDevant = cameraJoueur.position + (cameraJoueur.forward * 2.0f);
            
            // On ajuste la hauteur pour que ce soit au niveau des yeux
            positionDevant.y = cameraJoueur.position.y;

            transform.position = positionDevant;

            // 3. Orienter le menu pour qu'il regarde le joueur
            transform.LookAt(cameraJoueur);
            transform.Rotate(0, 180, 0); // On le retourne car LookAt montre l'arrière du Canvas
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}