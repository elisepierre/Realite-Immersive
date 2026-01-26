using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientPulse : MonoBehaviour
{
    public Image FondEnfers;
    public Color couleurHaut = new Color(0.5f, 0, 0); // rouge foncé
    public Color couleurBas = Color.black;
    public float speed = 1f;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1) / 2;
        FondEnfers.color = Color.Lerp(couleurBas, couleurHaut, t);
    }
}
