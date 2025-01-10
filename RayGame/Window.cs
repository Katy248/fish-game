using Raylib_cs;

namespace RayGame;

public class Window
{
    public const int DefaultWidth = 300;
    public const int DefaultHeight = 300;

    private int width;
    private int height;
    private readonly string title;
    private readonly Action? onInit;
    private readonly Action? onClose;
    
    private IScene _currentScene;
    private int _fps;

    public Window(string title,
                  int width = DefaultWidth,
                  int height = DefaultHeight,
                  Action? onInit = null,
                  Action? onClose = null)
    {
        this.width = width;
        this.height = height;
        this.title = title;
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
    
    public void SetCurrentScene(IScene scene)
    {
        _currentScene = scene;
    }
    public Window TargetFps(int fps)
    {
        _fps = fps;
        Raylib.SetTargetFPS(fps);
        return this;
    }
    public void Present()
    {
        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowState(ConfigFlags.ResizableWindow | ConfigFlags.AlwaysRunWindow);
        onInit?.Invoke();
        
        while (!Raylib.WindowShouldClose())
        {
            _currentScene?.Update();
        }
        onClose?.Invoke();
        Raylib.CloseWindow();
    }

    public int GetTargetFps()
    {
        return _fps;
    }
}
