using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFlameDisappear : MonoBehaviour
{
    public Material dissolveMat;
    public float duration = 1.5f;
    private bool isPlaying = false;

    public void TriggerDisappear()
    {
        if (!isPlaying)
            StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        isPlaying = true;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(0, 1, t / duration);

            dissolveMat.SetFloat("_DissolveAmount", v);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
