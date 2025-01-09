using Raylib_cs;
using RayGame;
using System.Numerics;

var window = new Window(800, 600, "Fish game", () =>
{
    Drawing.Start(draw =>
    {
        draw.Clear(Color.Gray);
        draw.Text("Some txt", new Vector2(10, 10), Color.Red);
    });
});

window.Present();
