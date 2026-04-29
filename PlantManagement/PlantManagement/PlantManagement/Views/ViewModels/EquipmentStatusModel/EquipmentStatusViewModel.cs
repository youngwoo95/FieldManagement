using System.Windows;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Comm.Behaviors;
using PlantManagement.Views.ViewModels.EquipmentStatusModel.Dialog;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public partial class EquipmentStatusViewModel : BaseViewModel
{
    private readonly IEquipmentDataService _equipmentDataService;
    private readonly IEquipmentEditDialogService _equipmentEditDialogService;
    private readonly ILampColorService _lampColorService;

    public ICommand AddFloorCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }

    public EquipmentStatusViewModel(
        IEquipmentDataService equipmentDataService,
        IEquipmentEditDialogService equipmentEditDialogService,
        ILampColorService lampColorService)
    {
        _equipmentDataService = equipmentDataService;
        _equipmentEditDialogService = equipmentEditDialogService;
        _lampColorService = lampColorService;

        _equipmentDataService.DataChanged += OnEquipmentDataChanged;
        _lampColorService.ColorChanged += OnLampColorChanged;

        AddFloorCommand = new RelayCommand(_ => OpenFloorEditor());
        EditCommand = new RelayCommand(_ => SetEditMode(true));
        SaveCommand = new RelayCommand(async _ => await SavePositionsAsync());

        _ = InitializeAsync();
        AddLog("Equipment status board loaded.");
    }

    private async Task InitializeAsync()
    {
        await _equipmentDataService.InitializeAsync();
        RefreshFloors();
    }

    private void SetEditMode(bool enabled)
    {
        IsEditMode = enabled;
        AddLog(enabled ? "Switched to edit mode." : "Switched to view mode.");
    }

    private async Task SavePositionsAsync()
    {
        if (string.IsNullOrWhiteSpace(_selectedFloor))
        {
            return;
        }

        var positions = Markers.Select(m => (m.Id, m.X, m.Y));
        var saved = await _equipmentDataService.SaveFloorPositionsAsync(_selectedFloor, positions);
        SetEditMode(false);
        AddLog(saved ? "위치가 저장되었습니다." : "위치 저장 실패.");
    }

    private void OpenFloorEditor()
    {
        var result = _equipmentEditDialogService.ShowFloorEditorDialog();
        if (result == true)
        {
            RefreshFloors();
            AddLog("Floor equipment assignment updated.");
        }
    }

    private void OnEquipmentDataChanged(object? sender, EventArgs e)
    {
        RefreshFloors();
    }

    private void OnLampColorChanged(object? sender, LampColorChangedEventArgs e)
    {
        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher is null) return;

        dispatcher.Invoke(() =>
        {
            var marker = Markers.FirstOrDefault(m => m.Id == e.LampId);
            if (marker is not null)
            {
                marker.LampState = e.Color.ToLowerInvariant(); // "red" | "amber" | "green" | "off"
                AddLog($"[{marker.Text}] 상태 변경: {e.Color.ToUpperInvariant()}");
            }
            else
            {
                AddLog($"[Lamp #{e.LampId}] 상태 변경: {e.Color.ToUpperInvariant()}");
            }
        });
    }

    private void RefreshFloors()
    {
        var previousFloor = _selectedFloor;

        Floors.Clear();
        foreach (var floor in _equipmentDataService.Floors)
        {
            Floors.Add(floor);
        }

        if (Floors.Count == 0)
        {
            SelectedFloor = string.Empty;
            Markers.Clear();
            return;
        }

        if (!string.IsNullOrWhiteSpace(previousFloor) && Floors.Contains(previousFloor))
        {
            SelectedFloor = previousFloor;
            return;
        }

        SelectedFloor = Floors[0];
    }

    private void AddLog(string message)
    {
        Logs.Insert(0, $"{DateTime.Now:HH:mm:ss}  {message}");
    }

    private void LoadMarkersForFloor(string floor)
    {
        Markers.Clear();
        if (string.IsNullOrWhiteSpace(floor))
        {
            return;
        }

        var floorEquipments = _equipmentDataService.GetFloorEquipments(floor);
        var positions = _equipmentDataService.GetFloorPositions(floor);

        const double originX = 90;
        const double originY = 90;
        const double xStep = 130;
        const double yStep = 160;
        const int maxPerRow = 4;

        var gridIndex = 0;
        foreach (var equipment in floorEquipments)
        {
            double x, y;
            if (positions.TryGetValue(equipment.Id, out var saved) && (saved.X != 0 || saved.Y != 0))
            {
                x = saved.X;
                y = saved.Y;
            }
            else
            {
                x = originX + (gridIndex % maxPerRow) * xStep;
                y = originY + (gridIndex / maxPerRow) * yStep;
            }

            var lampColor = _lampColorService.GetColor(equipment.Id);
            Markers.Add(new MarkerPosition
            {
                Id = equipment.Id,
                Text = equipment.Name,
                X = x,
                Y = y,
                LampState = lampColor ?? "off"
            });
            _lampColorService.RegisterLampId(equipment.Id);
            gridIndex++;
        }

        AddLog($"Displayed {floor} layout.");
    }
}
