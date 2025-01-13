using Raylib_cs;
using RayGame;
using System.Numerics;
using FishGame.Game;
using FishGame.Game.Scenes;


var window = new Window("Fish game", onInit: () =>
    {
        // GameplayState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        // GameplayState.Camera.Target = new Vector2(0, 0);
    })
    .Size(800, 600)
    .TargetFps(60);

var menu = new MainMenu(window);

var logo = new Scene<LogoSceneData>(new(window.GetTargetFps() * 3), data =>
{
    data.Timeout--;
    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
    {
        data.Timeout = 10;
    }
    if (data.Timeout <= 0)
        window.SetCurrentScene(menu);

    Drawing.Start(draw =>
    {
        draw.Clear(GruvboxColors.ForegroundLight);
        draw.Text("Art of defiance", new Vector2(10, 10), GruvboxColors.Background, fontSize: 50);
        draw.Rectangle(
            new Rectangle
            {
                Position = new Vector2(10 + (data.DefaultTimeout - data.Timeout) * 4, 10),
                Size = new Vector2(1000, 50)
            }, GruvboxColors.ForegroundLight);
#if DEBUG
        draw.Text($"Timeout: {data.Timeout / window.GetTargetFps()}s", new Vector2(10, 80), GruvboxColors.Background);
#endif
    });
});


window.SetCurrentScene(logo);
window.Present();


class LogoSceneData
{
    public int DefaultTimeout { get; }

    public LogoSceneData(int defaultTimeout)
    {
        DefaultTimeout = defaultTimeout;
        Timeout = DefaultTimeout;
    }

    public int Timeout;
}

