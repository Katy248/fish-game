using Raylib_cs;
using RayGame;
using System.Numerics;

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

class Menu
{
    private int _menuMargin = 20;
    private int _menuPadding = 5;
    private int _menuHeight = 100;
    private int _menuContentMargin = 5;

    private List<Button> _buttons = new();

    public Menu()
    {
        _buttons.Add(new Button("Add", () =>
        {
            GlobalState.Fish.Add(Fish.WithPosition(new Vector2(Raylib.GetRandomValue(50, 150))));
            if (GlobalState.Fish.Count == 1) throw new Exception();
        }));
        _buttons.Add(new Button("Remove", () =>
        {
            GlobalState.Fish.RemoveAt(GlobalState.Fish.Count - 1);
        }));
    }

    public void Draw(Draw draw)
    {
        var screenWidth = Raylib.GetScreenWidth();
        var screenHeight = Raylib.GetScreenHeight();
        var outerRect = new Rectangle
        {
            Size = new Vector2(screenWidth - _menuMargin * 2, _menuHeight),
            Position = new Vector2(_menuMargin, screenHeight - _menuMargin - _menuHeight),
        };
        var color = Color.Blue;
        color.A = 50;
        draw.Rectangle(outerRect, color);

        var innerRect = new Rectangle
        {
            Size = outerRect.Size - new Vector2(_menuPadding * 2),
            Position = outerRect.Position + new Vector2(_menuPadding),
        };

        color = Color.White;
        color.A = 200;

        draw.Rectangle(innerRect, color);

        // draw.Text("Dummy text only for demonstration", innerRect.Position + new Vector2(_menuContentMargin), Color.SkyBlue);

        int gap = 10;
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];

            btn.SetPosition(innerRect.Position + new Vector2(_menuContentMargin + gap * i + i * Button.Width, _menuContentMargin));
            btn.Draw(draw);
        }
    }

    public void Update()
    {
        foreach (var btn in _buttons)
        {
            btn.Update();
        }
    }
}

class Button
{
    public const int Width = 100;
    public const int Height = 80;
    private readonly string _text;
    private readonly Action _onClick;

    private Rectangle _rect;
    private Vector2 _startPosition;
    private bool _hover;

    public Button(string text, Action onClick, Vector2 startPosition = default)
    {
        _text = text;
        _onClick = onClick;
        _startPosition = startPosition;
        EvalRect();
    }

    private void EvalRect()
    {
        _rect = new Rectangle
        {
            Position = _startPosition,
            Size = new Vector2(Width, Height),
        };

    }

    public void SetPosition(Vector2 position) { _startPosition = position; EvalRect(); }

    public void Draw(Draw draw)
    {
        var padding = 20;
        var bgColor = _hover ? Color.Yellow : Color.Gold;
        draw.Rectangle(_rect, bgColor);
        draw.Text(_text, _startPosition + new Vector2(padding), Color.Black);
    }
    public void Update()
    {
        var cursor = Raylib.GetMousePosition();

        _hover = Raylib.CheckCollisionPointRec(cursor, _rect);

        if (_hover && Raylib.IsMouseButtonPressed(MouseButton.Left)) { _onClick(); }
    }

}

class Fish
{
    private Vector2 _position = new Vector2(200, 200);
    private float _radius = 18;
    private bool _captured;
    private bool _hover;

    private int _moveDirectionX = 1;
    private int _moveDirectionY = 1;

    private float _speed = 0.4f;

    const int ChangeDirectionTimeout = 50;
    private int _changeDirectionTimeout = ChangeDirectionTimeout;

    public static Fish WithPosition(Vector2 position)
    {
        return new Fish
        {
            _position = position,
        };
    }

    public void Draw(Draw d)
    {
        var drawRadius = _radius;
        var drawColor = Color.Red;
        if (_captured)
        {
            drawRadius += 12;
            drawColor = Color.Orange;
        }
        else if (_hover)
        {
            drawRadius += 2;
        }
        d.Circle(_position, drawRadius, drawColor);
    }

    public void Update(Camera2D cam)
    {
        var cursor = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cam);
        _hover = Raylib.CheckCollisionPointCircle(cursor, _position, _radius);

        if (!_captured && _hover && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            _captured = true;
            return;
        }

        if (_captured && _hover && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            _captured = false;
            return;
        }

        if (_captured)
        {
            _position = cursor;
        }
        else
        {
            int changeDirection(int direction)
            {
                if (_changeDirectionTimeout >= 0) return direction;

                _changeDirectionTimeout = ChangeDirectionTimeout;

                if (Random.Shared.Next() % 2 == 0)
                    return direction * -1;
                return direction;
            }
            _changeDirectionTimeout--;

            _moveDirectionX = changeDirection(_moveDirectionX);
            // _moveDirectionY = changeDirection(_moveDirectionY);

            _position.X += _speed * _moveDirectionX;
            // _position.Y += (float)Random.Shared.NextDouble() * _moveDirectionY;
        }
    }
}
