using RayGame;
using Raylib_cs;
using System.Numerics;

namespace FishGame.Game;

class Fish
{
    private Vector2 _position = new Vector2(200, 200);
    private float _radius = 18;
    private bool _captured;
    private bool _hover;
    private string _name;
    private Gamepad _gamepad;

    private int _moveDirectionX = 1;
    private int _moveDirectionY = 1;

    private float _speed = 0.4f;

    const int ChangeDirectionTimeout = 50;
    private int _changeDirectionTimeout = ChangeDirectionTimeout;
    private Func<Camera2D> _camera;

    public static Fish WithPosition(Vector2 position, Func<Camera2D> getCamera, Gamepad gamepad, string name = "Fish name")
    {
        return new Fish
        {
            _position = position,
            _camera = getCamera,
            _gamepad = gamepad,
            _name = name,
        };
    }

    public void Draw(Draw d)
    {
        var drawRadius = _radius;
        var drawColor = GruvboxColors.Red;
        if (_captured)
        {
            drawRadius += 12;
            drawColor = GruvboxColors.Orange;
        }
        else if (_hover)
        {
            drawRadius += 2;
        }

        d.Circle(_position, drawRadius, drawColor);

        if (_camera().Zoom >= 1.4 || _hover)
        {
            d.Text(_name, new Vector2(_position.X - 30, _position.Y + 30), GruvboxColors.Foreground);
        }
    }

    private Vector2 GetCursor()
    {
        if (_gamepad.Enabled)
        {
            return Raylib.GetScreenToWorld2D(new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2), _camera());
        }
        else
        {
            return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera());
        }
    }

    public void Update(Camera2D cam)
    {
        var cursor = GetCursor();
        _hover = Raylib.CheckCollisionPointCircle(cursor, _position, _radius);

        if (!_captured && _hover &&
                (Raylib.IsMouseButtonPressed(MouseButton.Left) || Raylib.IsGamepadButtonPressed(Gamepad.DefaultGamepad, GamepadButton.RightFaceDown)))
        {
            _captured = true;
            return;
        }

        if (_captured && _hover &&
                (Raylib.IsMouseButtonPressed(MouseButton.Left) || Raylib.IsGamepadButtonPressed(Gamepad.DefaultGamepad, GamepadButton.RightFaceDown)))
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
