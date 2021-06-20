using UnityEngine;

public class BackgroundColorAttribute : PropertyAttribute
{
    public float r;
    public float g;
    public float b;
    public float a;
    public BackgroundColorAttribute()
    {
        r = g = b = a = 1f;
    }
    public BackgroundColorAttribute(float aR, float aG, float aB, float aA, bool isColor)
    {
        if (isColor)
        {
            r = aR;
            g = aG;
            b = aB;
            a = aA;
        }
        else
        {
            r = g = b = a = 1f;
        }
    }
    public Color color { get { return new Color(r, g, b, a); } }
}
