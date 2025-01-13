namespace RayGame;

public class Scene<TData> : IScene
{
    private readonly TData _data;
    private readonly Action<TData> _onUpdate;

    public Scene(TData data, Action<TData> onUpdate)
    {
        _data = data;
        _onUpdate = onUpdate;
    }

    public void Dispose() { }

    public void Update()
    {
        _onUpdate(_data);
    }
}

public interface IScene : IDisposable
{
    void Update();
}
