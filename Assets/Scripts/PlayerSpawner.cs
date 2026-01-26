using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        string targetName = GameManager.nextSpawnPointName;

        if (!string.IsNullOrEmpty(targetName))
        {
            GameObject target = GameObject.Find(targetName);

            if (target != null)
            {
                transform.position = target.transform.position;
                transform.rotation = target.transform.rotation;

                Debug.Log("Joueur téléporté sur : " + targetName);
            }
            else
            {
                Debug.LogWarning("Impossible de trouver le point de spawn : " + targetName);
            }
        }

        GameManager.nextSpawnPointName = "";
    }
}