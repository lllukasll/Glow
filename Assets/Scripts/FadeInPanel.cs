using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInPanel : MonoBehaviour
{
    public Image panel;
    public float fadeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        panel.GetComponent<CanvasRenderer>().SetAlpha(0);
        panel.CrossFadeAlpha(1, fadeTime, true);
        //panel.CrossFadeColor(colorToFadeTo, fadeTime, true, true);
    }
}
