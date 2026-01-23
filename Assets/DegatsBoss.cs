using UnityEngine;

public class DegatsBoss : MonoBehaviour
{
    public int degats = 15;

    // Cette fonction détecte si le collider (poing/météorite) traverse le joueur
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // On cherche le script de vie sur le joueur
            VieJoueur vie = other.GetComponent<VieJoueur>();
            
            // Si on ne le trouve pas sur l'objet touché, on cherche sur le parent (XR Origin)
            if(vie == null) vie = other.GetComponentInParent<VieJoueur>();

            if (vie != null)
            {
                vie.RecevoirDegats(degats);
            }
        }
    }
}