using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HypnosDialogue : MonoBehaviour
{
    [Header("UI Links")]
    public GameObject dialogueCanvas;
    public TMP_Text textePNJ;
    public Button boutonContinuer; // Le bouton "Suite..." (ou clique n'importe où)

    [Header("Settings")]
    public Transform leJoueur;
    public float distanceInteraction = 3f;
    
    [TextArea(3, 10)] // Permet d'avoir des grandes cases dans l'inspecteur
    public string[] lignesDeDialogue; // Tes 4 phrases seront ici

    private int indexPhrase = 0;
    private bool estOuvert = false;

    void Start()
    {
        dialogueCanvas.SetActive(false);
        
        // Si on clique sur le bouton "Continuer", on passe à la suite
        if (boutonContinuer != null)
            boutonContinuer.onClick.AddListener(LireProchainePhrase);
    }

    void Update()
    {
        if (leJoueur == null) return;

        // 1. Ouvrir le dialogue avec E
        float distance = Vector3.Distance(transform.position, leJoueur.position);
        if (distance <= distanceInteraction && Input.GetKeyDown(KeyCode.E) && !estOuvert)
        {
            DemarrerDialogue();
        }
        else if (estOuvert)
        {
            // Optionnel : Appuyer sur E ou Espace pour passer le texte aussi
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            {
                LireProchainePhrase();
            }

            // Oriente le Canvas vers le joueur
            Transform cam = Camera.main.transform;
            dialogueCanvas.transform.LookAt(dialogueCanvas.transform.position + cam.rotation * Vector3.forward,
                                            cam.rotation * Vector3.up);
        }
    }

    void DemarrerDialogue()
    {
        estOuvert = true;
        dialogueCanvas.SetActive(true);
        indexPhrase = 0; // On commence au début
        
        // Affiche la première phrase
        if (lignesDeDialogue.Length > 0)
            textePNJ.text = lignesDeDialogue[0];
    }

    public void LireProchainePhrase()
    {
        // On avance d'un cran
        indexPhrase++;

        // Est-ce qu'il reste des phrases ?
        if (indexPhrase < lignesDeDialogue.Length)
        {
            textePNJ.text = lignesDeDialogue[indexPhrase];
        }
        else
        {
            // Fin du dialogue
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