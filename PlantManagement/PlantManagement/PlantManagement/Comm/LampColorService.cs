namespace PlantManagement.Comm;

public class LampColorChangedEventArgs(int lampId, string color) : EventArgs
{
    public int LampId { get; } = lampId;
    public string Color { get; } = color;
}

public interface ILampColorService
{
    event EventHandler<LampColorChangedEventArgs>? ColorChanged;
    void SetColor(int lampId, string color);
    string? GetColor(int lampId);
    void RegisterLampId(int lampId);
    IReadOnlyList<int> GetRegisteredIds();
}

public class LampColorService : ILampColorService
{
    private readonly Dictionary<int, string> _colors = new();
    private readonly HashSet<int> _registeredIds = new();
    private readonly Lock _lock = new();

    public event EventHandler<LampColorChangedEventArgs>? ColorChanged;

    public void SetColor(int lampId, string color)
    {
        lock (_lock)
        {
            _colors[lampId] = color;
        }

        ColorChanged?.Invoke(this, new LampColorChangedEventArgs(lampId, color));
    }

    public string? GetColor(int lampId)
    {
        lock (_lock)
        {
            return _colors.TryGetValue(lampId, out var color) ? color : null;
        }
    }

    public void RegisterLampId(int lampId)
    {
        lock (_lock)
        {
            _registeredIds.Add(lampId);
        }
    }

    public IReadOnlyList<int> GetRegisteredIds()
    {
        lock (_lock)
        {
            return _registeredIds.ToList();
        }
    }
}
