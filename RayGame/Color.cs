using Raylib_cs;

namespace RayGame;

public static class ColorExtensions
{
    public static Color Red(this Color c, byte red)
    {
        c.R = red;
        return c;
    }
    public static Color FromHex(this Color c, string hex)
    {
        var systemColor = System.Drawing.ColorTranslator.FromHtml(hex);

        c.R = systemColor.R;
        c.G = systemColor.G;
        c.B = systemColor.B;
        c.A = systemColor.A;

        return c;
    }
}
