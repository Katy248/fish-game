using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game;

class Fish : IDisposable
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

    private Image _image;
    private Texture2D _texture;

    public Fish()
    {
        _image = Raylib.LoadImage("./Assets/fish.png");
        _texture = Raylib.LoadTextureFromImage(_image);
        _texture.Width *= 2;
        _texture.Height *= 2;
    }

    public static Fish WithPosition(
        Vector2 position,
        Func<Camera2D> getCamera,
        Gamepad gamepad,
        string name = "Fish name"
    )
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

        //         if (_moveDirectionX > 0) // move to right
        //         {
        //             _texture.Width = -_texture.Height;
        // #if DEBUG
        //             d.Text($"Should move right", new Vector2(10, 70), GruvboxColors.Blue);
        // #endif
        //         }
        //         else
        //         {
        // #if DEBUG
        //             d.Text($"Should move left", new Vector2(10, 70), GruvboxColors.Blue);
        // #endif
        //         }

        //         d.Text($"Move direction: {_moveDirectionX}", new Vector2(10, 50), GruvboxColors.Orange);
        // d.Texture(_texture, _position, scale: _moveDirectionX > 0 ? -1 : 1);
        d.TextureRec(
            _texture,
            new Rectangle
            {
                Size = new Vector2(_texture.Width * -_moveDirectionX, _texture.Height),
            },
            new Vector2(_position.X - _texture.Width / 2, _position.Y - _texture.Height / 2)
        );

        if (_camera().Zoom >= 1.4 || _hover)
        {
            d.Text(
                _name,
                new Vector2(_position.X - _texture.Width / 2 - 20, _position.Y + 40),
                GruvboxColors.Foreground
            );
        }
    }

    private Vector2 GetCursor()
    {
        if (_gamepad.Enabled)
        {
            return Raylib.GetScreenToWorld2D(
                new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2),
                _camera()
            );
        }
        else
        {
            return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera());
        }
    }

    public void Update(Camera2D cam)
    {
        var cursor = GetCursor();
        _hover = Raylib.CheckCollisionPointRec(
            cursor,
            new Rectangle
            {
                Size = new Vector2(_texture.Width, _texture.Height),
                Position = new Vector2(
                    _position.X - _texture.Width / 2,
                    _position.Y - _texture.Height / 2
                ),
            }
        );

        if (
            !_captured
            && _hover
            && (
                Raylib.IsMouseButtonPressed(MouseButton.Left)
                || Raylib.IsGamepadButtonPressed(
                    Gamepad.DefaultGamepad,
                    GamepadButton.RightFaceDown
                )
                || Raylib.IsGamepadButtonPressed(
                    Gamepad.DefaultGamepad,
                    GamepadButton.RightTrigger2
                )
            )
        )
        {
            _captured = true;
            return;
        }

        if (
            _captured
            && _hover
            && (
                Raylib.IsMouseButtonPressed(MouseButton.Left)
                || Raylib.IsGamepadButtonPressed(
                    Gamepad.DefaultGamepad,
                    GamepadButton.RightFaceDown
                )
                || Raylib.IsGamepadButtonPressed(
                    Gamepad.DefaultGamepad,
                    GamepadButton.RightTrigger2
                )
            )
        )
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
                if (_changeDirectionTimeout >= 0)
                    return direction;

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

    public void Dispose()
    {
        Raylib.UnloadImage(_image);
        Raylib.UnloadTexture(_texture);
    }
}
