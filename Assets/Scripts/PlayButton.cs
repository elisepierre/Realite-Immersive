using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [Header("Références")]
    public Transform playerRoot;

    public Transform targetDestination;

    public void Teleport()
    {
  
        if (playerRoot != null && targetDestination != null)
        {
           
            playerRoot.position = targetDestination.position;

            playerRoot.rotation = targetDestination.rotation;
        }
        else
        {
            Debug.LogError("Attention : Le Player ou la Target n'est pas assigné dans l'inspecteur !");
        }
    }
}