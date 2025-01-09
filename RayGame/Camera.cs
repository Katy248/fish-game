using Raylib_cs;

namespace RayGame;

public static class CameraExtensions
{
    public static void Draw(this Camera2D camera, Action drawing)
    {
        Raylib.BeginMode2D(camera);
        drawing();
        Raylib.EndMode2D();
    }
}
