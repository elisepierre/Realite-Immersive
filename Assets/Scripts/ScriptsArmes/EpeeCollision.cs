using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Cela force Unity à ajouter un AudioSource si vous l'oubliez
public class EpeeCollision : MonoBehaviour
{
    [Header("Réglages Physique")]
    public float forceDeFrappe = 10f;

    [Header("Réglages Audio")]
    public AudioClip sonImpact; // Glissez votre fichier son ici

    // On garde une référence à l'AudioSource
    private AudioSource _audioSource;

    void Start()
    {
        // On récupère le composant AudioSource automatiquement au démarrage
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. La Physique : On vérifie si l'objet touché a un Rigidbody
        Rigidbody rbTouche = collision.gameObject.GetComponent<Rigidbody>();

        if (rbTouche != null)
        {
            Vector3 direction = collision.transform.position - transform.position;
            rbTouche.AddForce(direction.normalized * forceDeFrappe, ForceMode.Impulse);

            // 2. Le Son : On joue le bruitage
            if (sonImpact != null)
            {
                // Optionnel : Changer un peu le pitch pour que chaque coup sonne différemment
                _audioSource.pitch = Random.Range(0.9f, 1.1f);
                _audioSource.PlayOneShot(sonImpact);
            }
        }
    }
}