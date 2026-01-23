using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class HypnosDialogue : MonoBehaviour
{
    [Header("UI Links")]
    public GameObject dialogueCanvas;
    public TMP_Text textePNJ;
    public Button boutonContinuer;

    [Header("Settings")]
    public Transform leJoueur;
    public float distanceInteraction = 3f;
    
    [TextArea(3, 10)]
    public string[] lignesDeDialogue;

    private int indexPhrase = 0;
    private bool estOuvert = false;

    // step by step
    private bool boutonVREtaitAppuye = false; 

    void Start()
    {
        dialogueCanvas.SetActive(false);
        if (boutonContinuer != null)
            boutonContinuer.onClick.AddListener(LireProchainePhrase);
    }

    void Update()
    {
        if (leJoueur == null) return;
        bool inputVRActif = false;
        
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
        bool isPressed = false;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed))
        {
            if (isPressed && !boutonVREtaitAppuye)
            {
                inputVRActif = true;
            }
            boutonVREtaitAppuye = isPressed; // On memorise l'etat pour la frame suivante
        }

        // COMBINAISON CLAVIER + VR 
        bool actionValidee = Input.GetKeyDown(KeyCode.E) || inputVRActif;


        // LOGIQUE DIALOGUE
        float distance = Vector3.Distance(transform.position, leJoueur.position);

        if (distance <= distanceInteraction && actionValidee && !estOuvert)
        {
            DemarrerDialogue();
        }
        else if (estOuvert)
        {
            if (actionValidee || Input.GetKeyDown(KeyCode.Space))
            {
                LireProchainePhrase();
            }

            // Oriente le Canvas vers le joueur
            Transform cam = Camera.main.transform;
            dialogueCanvas.transform.LookAt(dialogueCanvas.transform.position  + cam.rotation * Vector3.forward,cam.rotation * Vector3.up
                                            );
        }
    }

    void DemarrerDialogue()
    {
        estOuvert = true;
        dialogueCanvas.SetActive(true);
        indexPhrase = 0;
        
        if (lignesDeDialogue.Length > 0)
            textePNJ.text = lignesDeDialogue[0];
    }

    public void LireProchainePhrase()
    {
        indexPhrase++;

        if (indexPhrase < lignesDeDialogue.Length)
        {
            textePNJ.text = lignesDeDialogue[indexPhrase];
        }
        else
        {
            FermerDialogue();
        }
    }

    void FermerDialogue()
    {
        estOuvert = false;
        dialogueCanvas.SetActive(false);
        indexPhrase = 0;
    }
}