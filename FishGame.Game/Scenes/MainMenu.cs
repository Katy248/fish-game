using System.Numerics;
using RayGame;
using Raylib_cs;

namespace FishGame.Game.Scenes;

public class MainMenu : IScene
{
    private readonly Window _window;
    private readonly IScene _gameplayScene;

    public MainMenu(Window window)
    {
        _window = window;
        _menus =
        [
            new("Play", () => { _window.SetCurrentScene(new Gameplay());}),
            new("Settings", () => { }),
            new("Quit", () => { Raylib.CloseWindow(); }),
        ];
    }

    private readonly List<MenuItem> _menus;

    public void Update()
    {
        Drawing.Start(draw =>
        {
            draw.Clear(GruvboxColors.Background);
            var index = 0;
            foreach (var item in _menus)
            {
                var position = new Vector2(40, 40 + index * 40);
                item.Draw(draw, position);
                index++;
            }
        });
    }
}

internal record MenuItem(string Text, Action Action)
{
    private bool _focused = false;

    private readonly Color _color = GruvboxColors.Foreground;
    private readonly Color _focusedColor = GruvboxColors.Blue;

    public void Draw(Draw draw, Vector2 position)
    {
        var rect = new Rectangle { Position = position, Size = new Vector2(80, 20) };
        var hover = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        var color = _color;
        if (_focused || hover)
        {
            color = _focusedColor;
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Action();
            }
        }

#if DEBUG
        draw.Rectangle(rect, GruvboxColors.Red);
#endif
        draw.Text(Text, position, color);
    }
};