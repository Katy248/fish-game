using Raylib_cs;
using RayGame;
using System.Numerics;
using FishGame.Game;
using FishGame.Game.Scenes;


var window = new Window("Fish game", onInit: () =>
    {
        GlobalState.Camera.Offset = new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2);
        GlobalState.Camera.Target = new Vector2(0, 0);
    })
    .Size(800, 600)
    .TargetFps(60);

var gameplay = new Gameplay();

var logo = new Scene<LogoSceneData>(new(window.GetTargetFps() * 3), data =>
{
    data.Timeout--;
    if (data.Timeout <= 0)
        window.SetCurrentScene(gameplay);

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

static class GlobalState
{
    public static Camera2D Camera = new Camera2D { Zoom = 1 };
    public static List<Fish> FishCollection = [Fish.WithPosition(new Vector2(150))];
    public static Toolbar Menu = new Toolbar();
    public static bool DarkModeEnabled = false;
}