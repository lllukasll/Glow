using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereColorsModel
{
    public SphereColorsModel(Color _color, Color _emissionColor)
    {
        Color = _color;
        EmissionColor = _emissionColor;
    }

    public Color Color { get; set; }
    public Color EmissionColor { get; set; }
}

public static class Colors
{
    // Start is called before the first frame update

    public static List<SphereColorsModel> colors = new List<SphereColorsModel>()
    {
        new SphereColorsModel(new Color(0, 0.7058824f, 0.3039038f, 1),new Color(0, 1.42f, 0,4288905)),
        new SphereColorsModel(new Color(1, 0.9103774f, 0.9245797f, 1), new Color(0.3396226f, 0.2494503f, 0.241901f, 1)),
        new SphereColorsModel(new Color(1, 0.8745098f, 0, 1), new Color(0.6588235f, 0.509804f, 0.1882353f)),
        new SphereColorsModel(new Color(1, 0.3019608f, 0, 0.00392156f), new Color(0.9137255f, 0.07058824f, 0)),
        new SphereColorsModel(new Color(1, 0.9098039f, 0.9254902f, 0.003921569f), new Color(0.3411765f, 0.2509804f, 0.2431373f))
    };

    
}
