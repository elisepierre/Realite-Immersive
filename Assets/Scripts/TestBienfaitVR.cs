using UnityEngine;

public class TestBienfaitVR : MonoBehaviour
{
    public Bienfait bienfait;

    private void OnTriggerEnter(Collider other)
    {
        // En VR, c'est souvent la main ou le corps qui touche
        // On cherche le script Player sur l'objet qui entre ou ses parents
        Player p = other.GetComponentInParent<Player>();
        if(p != null)
        {
            p.AjouterBienfait(bienfait);
            gameObject.SetActive(false); // L'objet disparaît
        }
    }
}