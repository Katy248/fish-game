using Raylib_cs;
using RayGame;
using System.Numerics;
using FishGame.Game;

const int MaxZoom = 3;

Vector2 lastCursorPosition = Raylib.GetMousePosition();

var window = new Window("Fish game", () =>
{
    foreach (var f in GlobalState.FishCollection)
    {
        f.Update(GlobalState.Camera);
    }
    GlobalState.Menu.Update();

    MovingCamera(lastCursorPosition);
    ZoomCamera();

    lastCursorPosition = Raylib.GetMousePosition();
    Drawing.Start(draw =>
    {
        draw.Clear(GruvboxColors.Background);

        GlobalState.Camera.Draw(() =>
        {
            draw.Text("Some txt", new Vector2(10, 10), Color.Red);

            foreach (var f in GlobalState.FishCollection)
            {
                f.Draw(draw);
            }
        });

        GlobalState.Menu.Draw(draw);
    });
}, onInit: () =>
{
    GlobalState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
    GlobalState.Camera.Target = new Vector2(0, 0);
})
.Size(800, 600)
.TargetFps(60);

window.Present();


void MovingCamera(Vector2 lastCursorPosition)
{
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
}

void ZoomCamera()
{
    GlobalState.Camera.Zoom += Raylib.GetMouseWheelMove() * 0.05f;
    if (GlobalState.Camera.Zoom > MaxZoom)
        GlobalState.Camera.Zoom = 3;
}

static class GlobalState
{
    public static Camera2D Camera = new Camera2D { Zoom = 1 };
    public static List<Fish> FishCollection = [Fish.WithPosition(new Vector2(150))];
    public static Toolbar Menu = new Toolbar();
    public static bool DarkModeEnabled = false;
}

