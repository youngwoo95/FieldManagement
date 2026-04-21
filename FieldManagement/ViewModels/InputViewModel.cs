using System.Collections.ObjectModel;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class InputViewModel : BaseViewModel
{
    private const string DefaultFloorText = "선택해주세요";
    private readonly FloorManagementService _floorManagementService = FloorManagementService.Instance;
    private readonly IFloorEditDialogService _floorEditDialogService;

    public ObservableCollection<MarkerPosition> Markers { get; } = new();
    public ObservableCollection<string> Logs { get; } = new();
    public ObservableCollection<string> Floors { get; } = new();

    private string? _selectedFloor;
    public string? SelectedFloor
    {
        get => _selectedFloor;
        set
        {
            if (_selectedFloor == value)
                return;

            _selectedFloor = value;
            OnPropertyChanged();
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand EditCommand { get; }

    public InputViewModel(IFloorEditDialogService floorEditDialogService)
    {
        _floorEditDialogService = floorEditDialogService;

        SaveCommand = new RelayCommand(_ => Save());
        EditCommand = new RelayCommand(_ => OpenEdit());

        RefreshFloors();
        SelectedFloor = DefaultFloorText;

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

    public void RefreshFloors(string? preferredFloorName = null)
    {
        Floors.Clear();
        Floors.Add(DefaultFloorText);

        foreach (var floorName in _floorManagementService.GetFloorNames())
            Floors.Add(floorName);

        if (!string.IsNullOrWhiteSpace(preferredFloorName) && Floors.Contains(preferredFloorName))
        {
            SelectedFloor = preferredFloorName;
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedFloor) || !Floors.Contains(SelectedFloor))
            SelectedFloor = DefaultFloorText;
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
        var selectedFloorName = _floorEditDialogService.ShowFloorEditor(SelectedFloor);
        if (string.IsNullOrWhiteSpace(selectedFloorName))
            return;

        RefreshFloors(selectedFloorName);
    }
}
