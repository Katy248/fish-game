using System.Numerics;
using Raylib_cs;

namespace RayGame;

public class Draw
{
    public const int DefaultFontSize = 16;
    public const int DefaultFontSpacing = 1;
    public void Text(string text, Vector2 position, Color color, Font font = default, int fontSize = DefaultFontSize, int spacing = DefaultFontSpacing)
    {
        Raylib.DrawTextEx(font, text, position, fontSize, spacing, color);
    }
    // private void DrawTextWithFont(text, Vector2 position, Color color, Font font, int fontSize = DefaultFontSize) { }

    public void Clear(Color color)
    {
        Raylib.ClearBackground(color);
    }
    public void Circle(Vector2 position, float radius, Color color)
    {
        Raylib.DrawCircleV(position, radius, color);
    }

    public void Rectangle(Rectangle rect, Color color)
    {
        Raylib.DrawRectangleRec(rect, color);
    }
}

public static class Drawing
{
    public static void Start(Action<Draw> drawAction)
    {
        Raylib.BeginDrawing();
        var draw = new Draw();
        drawAction(draw);
        Raylib.EndDrawing();
    }
}
