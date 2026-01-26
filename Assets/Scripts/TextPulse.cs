using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPulse : MonoBehaviour
{
    public TMP_Text texte;
    public float speed = 2f;
    public float amplitude = 5f;

    private Vector2 basePos;

    void Start()
    {
        basePos = texte.rectTransform.anchoredPosition;
    }

    void Update()
    {
        texte.rectTransform.anchoredPosition =
            basePos + new Vector2(0, Mathf.Sin(Time.time * speed) * amplitude);
    }
}
