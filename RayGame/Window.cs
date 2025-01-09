using Raylib_cs;

namespace RayGame;

public class Window
{
    public const int DefaultWidth = 300;
    public const int DefaultHeight = 300;

    private int width;
    private int height;
    private readonly string title;
    private readonly Action _onUpdate;
    private readonly Action? onInit;
    private readonly Action? onClose;

    public Window(string title,
                  Action onUpdate,
                  int width = DefaultWidth,
                  int height = DefaultHeight,
                  Action? onInit = null,
                  Action? onClose = null)
    {
        this.width = width;
        this.height = height;
        this.title = title;
        this._onUpdate = onUpdate;
        this.onInit = onInit;
        this.onClose = onClose;
    }
    public Window Size(int width, int height)
    {
        this.width = width;
        this.height = height;

        Raylib.SetWindowSize(width, height);

        return this;
    }
    public Window TargetFps(int fps)
    {
        Raylib.SetTargetFPS(fps);
        return this;
    }
    public void Present()
    {
        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowState(ConfigFlags.ResizableWindow);
        onInit?.Invoke();


        while (!Raylib.WindowShouldClose())
        {
            _onUpdate();
        }
        onClose?.Invoke();
        Raylib.CloseWindow();
    }
}
