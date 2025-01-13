using System.Numerics;
using Raylib_cs;

namespace RayGame;

public class Draw
{
    public const int DefaultFontSize = 18;
    public const int DefaultFontSpacing = 1;

    public void Text(
        string text,
        Vector2 position,
        Color color,
        Font font = default,
        int fontSize = DefaultFontSize,
        int spacing = DefaultFontSpacing
    )
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

    public void Texture(Texture2D t, Vector2 position, float rotation = 0f, float scale = 1f)
    {
        Raylib.DrawTextureEx(t, position, rotation, scale, Color.White);
    }

    public void TextureRec(Texture2D t, Rectangle rec, Vector2 position)
    {
        Raylib.DrawTextureRec(t, rec, position, Color.White);
    }

    public void TexturePro(
        Texture2D t,
        Rectangle source,
        Rectangle dest,
        Vector2 origin,
        float rotation = 0
    )
    {
        Raylib.DrawTexturePro(t, source, dest, origin, rotation, Color.Orange);
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
