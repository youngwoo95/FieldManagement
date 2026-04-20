using System.Collections.ObjectModel;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class InputViewModel : BaseViewModel
{
    public ObservableCollection<MarkerPosition> Markers { get; } = new();
    public ObservableCollection<string> Logs { get; } = new();

    private string? _selectedFloor;
    public string? SelectedFloor
    {
        get => _selectedFloor;
        set
        {
            _selectedFloor = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Floors { get; } = new()
    {
        "선택해주세요",
        "1층",
        "2층"
    };

    public ICommand SaveCommand { get; }
    public ICommand EditCommand { get; }

    public InputViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save());
        EditCommand = new RelayCommand(_ => OpenEdit());

        SelectedFloor = "선택해주세요";

        Markers.Add(new MarkerPosition { Id = 1, Text = "1", X = 120, Y = 80 });
        Markers.Add(new MarkerPosition { Id = 2, Text = "2", X = 260, Y = 150 });
        Markers.Add(new MarkerPosition { Id = 3, Text = "3", X = 420, Y = 220 });

        Logs.Add("2026-06-01 11:23:00 [1번] 생산완료");
        Logs.Add("2026-06-01 11:55:00 [10번] 생산완료");
        Logs.Add("2026-06-01 12:23:00 [2번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
        Logs.Add("2026-06-01 13:03:00 [5번] 생산완료");
    }

    private void Save()
    {
        foreach (var marker in Markers)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Id={marker.Id}, Text={marker.Text}, X={marker.X}, Y={marker.Y}");
        }
    }

    private void OpenEdit()
    {
        // 팝업 여는 건 View에서 처리하거나,
        // 나중에 DialogService로 분리 가능
    }
}