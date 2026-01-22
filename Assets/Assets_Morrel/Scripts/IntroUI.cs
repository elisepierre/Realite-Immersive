using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject CanvasIntro;   // Ton panel entier
    public Material dissolveMat;     // Matériau avec shader FlameDissolve
    public float duration = 1.5f;    // Durée de la brûlure

    private Image imageUI;           // Image du panel

    void Awake()
    {
        if (CanvasIntro != null)
        {
            imageUI = CanvasIntro.GetComponent<Image>();
            if (imageUI == null)
                imageUI = CanvasIntro.AddComponent<Image>();

            if (dissolveMat != null)
                imageUI.material = dissolveMat; // Assigner le shader au panel
        }
    }

    public void FermerIntro()
    {
        if (dissolveMat != null)
            StartCoroutine(DissolveCoroutine());
        else
            CanvasIntro.SetActive(false); // fallback si pas de shader
    }

    IEnumerator DissolveCoroutine()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, t / duration);
            dissolveMat.SetFloat("_DissolveAmount", value); // anime la brûlure
            yield return null;
        }

        CanvasIntro.SetActive(false); // désactive le panel après
    }
}
