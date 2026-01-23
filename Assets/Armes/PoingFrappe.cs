using UnityEngine;

public class PoingFrappe : MonoBehaviour
{
    public float forceDuCoup = 5f;
    public AudioSource sourceAudio; // Optionnel : pour faire un bruit de "Paf"

    private void OnCollisionEnter(Collision collision)
    {
        // On cherche si l'objet qu'on tape a un Rigidbody
        Rigidbody rbTouche = collision.gameObject.GetComponent<Rigidbody>();

        if (rbTouche != null)
        {
            // On calcule la direction du coup (Du poing vers l'objet)
            Vector3 direction = collision.contacts[0].point - transform.position;

            // On ajoute une impulsion brutale
            rbTouche.AddForce(direction.normalized * forceDuCoup, ForceMode.Impulse);

            // Petit bruit si configuré
            if (sourceAudio != null) sourceAudio.Play();
        }
    }
}