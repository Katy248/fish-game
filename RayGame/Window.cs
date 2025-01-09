using Raylib_cs;

namespace RayGame;

public class Window
{
    private readonly int width;
    private readonly int height;
    private readonly string title;
    private readonly Action beforeClose;
    private readonly Action? onClose;

    public Window(int width, int height, string title, Action beforeClose, Action? onClose = null)
    {
        this.width = width;
        this.height = height;
        this.title = title;
        this.beforeClose = beforeClose;
        this.onClose = onClose;
    }
    public void Present()
    {
        Raylib.InitWindow(width, height, title);
        while (!Raylib.WindowShouldClose())
        {
            beforeClose();
        }
        onClose?.Invoke();
        Raylib.CloseWindow();
    }
}
