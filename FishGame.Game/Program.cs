using Raylib_cs;
using RayGame;
using System.Numerics;
using FishGame.Game;

const float MinZoom = 2f;
const float MaxZoom = 0.5f;

var window = new Window("Fish game", onInit: () =>
    {
        GlobalState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        GlobalState.Camera.Target = new Vector2(0, 0);
    })
    .Size(800, 600)
    .TargetFps(60);

Vector2 lastCursorPosition = Raylib.GetMousePosition();

var gameplay = new Scene<string>("Dummy string data", obj =>
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

        draw.Text($"Camera zoom: {GlobalState.Camera.Zoom}", new Vector2(10, 10), Color.Red);
        GlobalState.Menu.Draw(draw);
    });
});

var logoTimeout = window.GetTargetFps() * 5;
var logo = new Scene<LogoSceneData>(new() { Timeout = logoTimeout, }, data =>
{
    data.Timeout--;
    if (data.Timeout <= 0)
        window.SetCurrentScene(gameplay);

    Drawing.Start(draw =>
    {
        draw.Clear(GruvboxColors.ForegroundLight);
        draw.Text("Art of defiance", new Vector2(10, 10), GruvboxColors.Background, fontSize: 50);
        draw.Rectangle(
            new Rectangle
            {
                Position = new Vector2(10 + (logoTimeout - data.Timeout) * 4, 10),
                Size = new Vector2(1000, 50)
            }, GruvboxColors.ForegroundLight);
#if DEBUG
        draw.Text($"Timeout: {data.Timeout / window.GetTargetFps()}s", new Vector2(10, 80), GruvboxColors.Background);
#endif
    });
});


window.SetCurrentScene(logo);
window.Present();


void MovingCamera(Vector2 lastCursorPosition)
{
    if (Raylib.IsMouseButtonDown(MouseButton.Middle) ||
        Raylib.IsKeyDown(KeyboardKey.Space) && Raylib.IsMouseButtonDown(MouseButton.Left))
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

    if (GlobalState.Camera.Zoom < MaxZoom)
        GlobalState.Camera.Zoom = MaxZoom;
    if (GlobalState.Camera.Zoom > MinZoom)
        GlobalState.Camera.Zoom = MinZoom;
}

class LogoSceneData
{
    public int Timeout;
}

static class GlobalState
{
    public static Camera2D Camera = new Camera2D { Zoom = 1 };
    public static List<Fish> FishCollection = [Fish.WithPosition(new Vector2(150))];
    public static Toolbar Menu = new Toolbar();
    public static bool DarkModeEnabled = false;
}