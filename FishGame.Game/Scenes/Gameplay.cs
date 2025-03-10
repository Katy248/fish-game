using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game.Scenes;

class GameplayState
{
    public GameplayState(Action<List<Fish>> addFish)
    {
        Toolbar = new(this.FishCollection, addFish);
    }

    public Camera2D Camera = new Camera2D { Zoom = 1 };
    public List<Fish> FishCollection = new();
    public Toolbar Toolbar;
    public bool DarkModeEnabled = false;

    public enum GameplayAction
    {
        None,
        HoldingFish,
        AddingFish,
    }

    public GameplayAction CurrentAction = GameplayAction.None;
}

public class Gameplay : IScene
{
    private const float MinZoom = 2f;
    private const float MaxZoom = 0.5f;

    private readonly GameplayState _state;

    private Vector2 _lastCursorPosition = Raylib.GetMousePosition();
    private Gamepad _gamepad = new();

    public Gameplay()
    {
        var getCamera = () =>
        {
            return _state?.Camera ?? throw new Exception("GameplayState object is null");
            // ?? throw new Exception("Camera field is not init in GameplayState object");
        };
        var addFish = (List<Fish> fish) =>
        {
            fish.Add(
                new Fish(
                    $"Cool fish {Raylib.GetRandomValue(1, 100)}",
                    new Vector2(Raylib.GetRandomValue(50, 200), Raylib.GetRandomValue(50, 200)),
                    getCamera,
                    _gamepad,
                    this._state
                )
            );
        };
        _state = new(addFish);
        _state.FishCollection.Add(
            new Fish("First fish", new Vector2(150), getCamera, _gamepad, _state)
        );
        _lastCursorPosition = Raylib.GetMousePosition();
    }

    public void Update()
    {
        _gamepad.Update();
        foreach (var f in _state.FishCollection)
        {
            f.Update(_state.Camera);
        }

        _state.Toolbar.Update();

        MovingCamera(_lastCursorPosition);
        ZoomCamera();

        _lastCursorPosition = Raylib.GetMousePosition();
        Drawing.Start(draw =>
        {
            draw.Clear(GruvboxColors.Background);

            _state.Camera.Draw(() =>
            {
                draw.Text("Some txt", new Vector2(10, 10), Color.Red);

                foreach (var f in _state.FishCollection)
                {
                    f.Draw(draw);
                }
            });
#if DEBUG
            draw.Text($"Camera zoom: {_state.Camera.Zoom}", new Vector2(10, 10), Color.Red);
            draw.Text(
                _gamepad.Enabled ? "Gamepad enabled" : "Gamepad disabled",
                new Vector2(10, 30),
                Color.Red
            );
#endif
            _state.Toolbar.Draw(draw);
        });
    }

    private const int DefaultGamepad = 0;
    private float _deadZone = 0.1f;
    private const float GamepadMoveSensetivity = 3.5f;

    void MovingCamera(Vector2 lastCursorPosition)
    {
        //gamepad
        if (_gamepad.Enabled)
        {
            _state.Camera.Target +=
                _gamepad.LeftStickDelta * GamepadMoveSensetivity / _state.Camera.Zoom;
        }
        //

        if (
            Raylib.IsMouseButtonDown(MouseButton.Middle)
            || Raylib.IsKeyDown(KeyboardKey.Space) && Raylib.IsMouseButtonDown(MouseButton.Left)
        )
        {
            _state.Camera.Target +=
                (lastCursorPosition - Raylib.GetMousePosition()) / _state.Camera.Zoom;
            _state.Camera.Offset = new Vector2(
                Raylib.GetScreenWidth() / 2,
                Raylib.GetScreenHeight() / 2
            );

            Raylib.SetMouseCursor(MouseCursor.ResizeAll);
        }
        else
        {
            Raylib.SetMouseCursor(MouseCursor.Arrow);
        }
    }

    void ZoomCamera()
    {
        if (_gamepad.Enabled)
        {
            _state.Camera.Zoom += -_gamepad.RightStickDelta.Y * 0.02f;
        }

        _state.Camera.Zoom += Raylib.GetMouseWheelMove() * 0.05f;

        if (_state.Camera.Zoom < MaxZoom)
            _state.Camera.Zoom = MaxZoom;
        if (_state.Camera.Zoom > MinZoom)
            _state.Camera.Zoom = MinZoom;
    }

    public void Dispose()
    {
        foreach (var f in _state.FishCollection)
        {
            f.Dispose();
        }
    }
}
