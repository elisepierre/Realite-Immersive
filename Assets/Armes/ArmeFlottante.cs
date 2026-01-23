using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Nécessaire pour parler au système VR

public class ArmeFlottante : MonoBehaviour
{
    [Header("Réglages Flottement")]
    public float vitesseRotation = 50f;
    public float amplitudeHautBas = 0.1f;
    public float vitesseHautBas = 2f;

    private Vector3 positionDepart;
    private bool estRamasse = false;
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Mise à jour pour XRI 3.x

    void Awake()
    {
        positionDepart = transform.position;
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    // On s'abonne aux événements quand l'objet est activé
    void OnEnable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    // On se désabonne pour éviter les erreurs de mémoire
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
        // Si ramassé, on ne fait RIEN (on laisse la main contrôler)
        if (estRamasse) return;

        // Animation de flottement
        transform.Rotate(0, vitesseRotation * Time.deltaTime, 0);
        float newY = positionDepart.y + Mathf.Sin(Time.time * vitesseHautBas) * amplitudeHautBas;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Appelée automatiquement quand on prend l'arme
    private void OnGrab(SelectEnterEventArgs args)
    {
        estRamasse = true;
        // On s'assure que la physique ne gêne pas la main
        if (rb != null) rb.isKinematic = false; 
    }

    // Appelée automatiquement quand on lâche l'arme
    private void OnRelease(SelectExitEventArgs args)
    {
        // On laisse l'arme tomber au sol, on ne réactive PAS le flottement
        // car sinon elle se téléporterait à son point de départ !
        estRamasse = true; // Elle reste considérée comme "plus flottante"
        
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }
}