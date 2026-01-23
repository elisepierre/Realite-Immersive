using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
// Note: Si vous êtes sur XRI version 3, ajoutez: using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArmeFlottanteAméliorée : MonoBehaviour
{
    [Header("Réglages Flottement")]
    public float vitesseRotation = 50f;
    public float amplitudeHautBas = 0.1f;
    public float vitesseHautBas = 2f;

    private Vector3 positionDepart;
    private bool estRamasse = false;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // On sauvegarde la position initiale
        positionDepart = transform.position;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        // FORCE le mode "Déco/Flottant" au démarrage pour éviter le clignotement
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true; // Important : coupe la physique
        }
    }

    void OnEnable()
    {
        if (grabInteractable != null)
        {
            // On écoute quand la main attrape ou lâche l'objet
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }

    void Update()
    {
        // Si l'objet est dans la main ou tombé au sol, on arrête de le faire flotter
        if (estRamasse) return;

        // Animation douce
        transform.Rotate(0, vitesseRotation * Time.deltaTime, 0);
        float newY = positionDepart.y + Mathf.Sin(Time.time * vitesseHautBas) * amplitudeHautBas;

        // On applique la position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        estRamasse = true;

        // On prépare la physique pour l'interaction
        if (rb != null)
        {
            rb.isKinematic = false; // La main et la physique prennent le relais
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Quand on lâche, on ne remet PAS le flottement (sinon l'arme reviendrait à sa place de départ)
        // On active la gravité pour qu'elle tombe au sol
        estRamasse = true;

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}