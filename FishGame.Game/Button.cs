using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game;

class Button
{
    public const int Width = 100;
    public const int Height = 40;
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
        var padding = 15;
        var bgColor = _hover ? GruvboxColors.YellowDim : GruvboxColors.Yellow;
        var fgColor = _hover ? GruvboxColors.Background : GruvboxColors.Foreground;
        draw.Rectangle(_rect, bgColor);
        draw.Text(_text, _startPosition + new Vector2(padding), fgColor);
    }
    public void Update()
    {
        var cursor = Raylib.GetMousePosition();

        _hover = Raylib.CheckCollisionPointRec(cursor, _rect);

        if (_hover && Raylib.IsMouseButtonPressed(MouseButton.Left)) { _onClick(); }
    }

}
