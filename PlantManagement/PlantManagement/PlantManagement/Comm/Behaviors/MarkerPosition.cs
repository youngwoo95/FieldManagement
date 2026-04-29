using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlantManagement.Comm.Behaviors;

/// <summary>
/// LampState 값: "red" | "amber" | "green" | "off" (기본값)
/// </summary>
public class MarkerPosition : INotifyPropertyChanged
{
    private int _id;
    private string _text = string.Empty;
    private double _x;
    private double _y;
    private string _lampState = "off";

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Id
    {
        get => _id;
        set => SetField(ref _id, value);
    }

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }

    public double X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    public double Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    /// <summary>
    /// 표시등 상태: "red" | "amber" | "green" | "off"
    /// </summary>
    public string LampState
    {
        get => _lampState;
        set => SetField(ref _lampState, value);
    }

    public override string ToString()
    {
        return $"id: {Id} Text: {Text} X: {X} Y: {Y}";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        OnPropertyChanged(propertyName);
    }
}
