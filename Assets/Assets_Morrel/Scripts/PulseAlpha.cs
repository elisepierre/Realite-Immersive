using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseAlpha : MonoBehaviour
{
    public Image img;
    public float speed = 2f;
    public float minA = 0.1f;
    public float maxA = 0.3f;

    void Update()
    {
        float a = Mathf.Lerp(minA, maxA, (Mathf.Sin(Time.time * speed) + 1) / 2);
        img.color = new Color(img.color.r, img.color.g, img.color.b, a);
    }
}
