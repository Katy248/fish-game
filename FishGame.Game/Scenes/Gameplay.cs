using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game.Scenes;

public class Gameplay : IScene
{
    private const float MinZoom = 2f;
    private const float MaxZoom = 0.5f;
    
    private Vector2 _lastCursorPosition = Raylib.GetMousePosition();
    public void Update()
    {
        foreach (var f in GlobalState.FishCollection)
        {
            f.Update(GlobalState.Camera);
        }

        GlobalState.Menu.Update();

        MovingCamera(_lastCursorPosition);
        ZoomCamera();

        _lastCursorPosition = Raylib.GetMousePosition();
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
    }
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
}