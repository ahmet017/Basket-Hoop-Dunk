using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ColorExtensions
{
    public static Color SetSaturation(this Color color, float newSaturation)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        s = newSaturation;
        return Color.HSVToRGB(h, s, v);
    }

    public static Color DeSaturate(this Color color, float deSaturationPercent)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        s *= deSaturationPercent;

        return Color.HSVToRGB(h, s, v);
    }

    public static float GetValue(this Color color)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        return v;
    }

    public static Color SetValue(this Color color, float newValue)
    {
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);
        v = newValue;
        return Color.HSVToRGB(h, s, v);
    }

    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    
    public static Color SetItensity(this Color color, float intensity)
    {
        return color * Mathf.Pow(intensity, 2);
    }
}
