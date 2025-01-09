using Raylib_cs;
using RayGame;
using System.Numerics;
using FishGame.Game;

const int MaxZoom = 3;

Vector2 lastCursorPosition = Raylib.GetMousePosition();

var window = new Window("Fish game", () =>
{
    foreach (var f in GlobalState.Fish)
    {
        f.Update(GlobalState.Camera);
    }
    GlobalState.Menu.Update();

    // Move camera
    if (Raylib.IsMouseButtonDown(MouseButton.Middle) || Raylib.IsKeyDown(KeyboardKey.Space) && Raylib.IsMouseButtonDown(MouseButton.Left))
    {
        GlobalState.Camera.Target += (lastCursorPosition - Raylib.GetMousePosition()) / GlobalState.Camera.Zoom;
        GlobalState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);

        Raylib.SetMouseCursor(MouseCursor.ResizeAll);
    }
    else
    {
        Raylib.SetMouseCursor(MouseCursor.Arrow);
    }
    // zoom camera
    GlobalState.Camera.Zoom += Raylib.GetMouseWheelMove() * 0.05f;
    lastCursorPosition = Raylib.GetMousePosition();
    Drawing.Start(draw =>
    {
        draw.Clear(Color.Gray);

        Raylib.BeginMode2D(GlobalState.Camera);
        {
            draw.Text("Some txt", new Vector2(10, 10), Color.Red);

            foreach (var f in GlobalState.Fish)
            {
                f.Draw(draw);
            }
        }
        Raylib.EndMode2D();

        GlobalState.Menu.Draw(draw);
    });
})
.Size(800, 600)
.TargetFps(60);

GlobalState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
GlobalState.Camera.Target = new Vector2(0, 0);

window.Present();

static class GlobalState
{
    public static Camera2D Camera = new Camera2D { Zoom = 1 };
    public static List<Fish> Fish = [new Fish()];
    public static Menu Menu = new Menu();
}



