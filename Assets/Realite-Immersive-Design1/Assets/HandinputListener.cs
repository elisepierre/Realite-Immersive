    using System.Collections;
    using System.Collections.Generic;
using UnityEngine;
    using UnityEngine.InputSystem;

    public class HandinputListener : MonoBehaviour
    {

        public InputActionProperty gripAction;
    //C'est la connexion avec la manette. Dans l'inspecteur Unity, tu vas y assigner l'action "Select" ou "Grip" (ex: XRI RightHand Interaction/Select Value). Cela permet de savoir à quel point tu appuies (de 0 à 1).

        public Animator handAnimator;
    //C'est la référence au composant Animator qui est sur ton modèle 3D de main. C'est lui qui gère les transitions d'animation (Main ouverte <-> Poing fermé).


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
        void Update()
        {
            float gripValue = gripAction.action.ReadValue<float>();
            handAnimator.SetFloat("Grip", gripValue);
        }
    }
