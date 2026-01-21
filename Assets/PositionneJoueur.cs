using UnityEngine;

public class PositionneJoueur : MonoBehaviour
{
    void Start()
    {
        GameObject pointDeSpawn = GameObject.FindGameObjectWithTag("Respawn");

        if (pointDeSpawn != null)
        {
            // On récupère la position cible
            Vector3 positionCible = pointDeSpawn.transform.position;

            // CORRECTIF : On ajoute un tout petit peu de hauteur (0.05f = 5cm)
            // pour être sûr de ne pas être coincé dans le sol
            positionCible.y += 0.05f;

            transform.position = positionCible;

            // On garde la rotation (mais on s'assure de ne pas être penché)
            // On ne prend que la rotation Y (gauche/droite) du spawn
            Vector3 rotationCible = pointDeSpawn.transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotationCible.y, 0);
        }
    }
}