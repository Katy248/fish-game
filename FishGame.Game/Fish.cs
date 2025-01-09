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
