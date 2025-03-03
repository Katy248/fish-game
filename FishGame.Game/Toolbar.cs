using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game;

class Toolbar
{
    private readonly int _menuMargin = 20;
    private readonly int _menuPadding = 5;
    private readonly int _menuHeight = 60;
    private readonly int _menuContentMargin = 5;

    private readonly List<Fish> _fishCollection;

    private List<Button> _buttons = new();

    public Toolbar(List<Fish> fishCollection, Action<List<Fish>> addFish)
    {
        _fishCollection = fishCollection;
        _buttons.Add(new Button("Add", () => addFish(fishCollection)));
        _buttons.Add(
            new Button(
                "Remove",
                () =>
                {
                    if (_fishCollection.Count == 0)
                        return;
                    _fishCollection.RemoveAt(_fishCollection.Count - 1);
                },
                () =>
                {
                    return _fishCollection.Count > 0;
                }
            )
        );
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
        var color = GruvboxColors.Blue;
        draw.Rectangle(outerRect, color);

        var innerRect = new Rectangle
        {
            Size = outerRect.Size - new Vector2(_menuPadding * 2),
            Position = outerRect.Position + new Vector2(_menuPadding),
        };

        color = GruvboxColors.ForegroundLight;
        draw.Rectangle(innerRect, color);

        int gap = 10;
        for (int i = 0; i < _buttons.Count; i++)
        {
            var btn = _buttons[i];

            btn.SetPosition(
                innerRect.Position
                    + new Vector2(
                        _menuContentMargin + gap * i + i * Button.Width,
                        _menuContentMargin
                    )
            );
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
