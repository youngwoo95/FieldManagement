using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Comm.Behaviors;
using PlantManagement.Views.ViewModels.EquipmentStatusModel.Dialog;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public partial class EquipmentStatusViewModel : BaseViewModel
{
    private readonly IEquipmentDataService _equipmentDataService;
    private readonly IEquipmentEditDialogService _equipmentEditDialogService;

    public ICommand AddFloorCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }

    public EquipmentStatusViewModel(
        IEquipmentDataService equipmentDataService,
        IEquipmentEditDialogService equipmentEditDialogService)
    {
        _equipmentDataService = equipmentDataService;
        _equipmentEditDialogService = equipmentEditDialogService;

        _equipmentDataService.DataChanged += OnEquipmentDataChanged;

        AddFloorCommand = new RelayCommand(_ => OpenFloorEditor());
        EditCommand = new RelayCommand(_ => SetEditMode(true));
        SaveCommand = new RelayCommand(_ => SetEditMode(false));

        RefreshFloors();
        AddLog("Equipment status board loaded.");
    }

    private void SetEditMode(bool enabled)
    {
        IsEditMode = enabled;
        AddLog(enabled ? "Switched to edit mode." : "Switched to view mode.");
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

        const double originX = 90;
        const double originY = 90;
        const double xStep = 130;
        const double yStep = 160;
        const int maxPerRow = 4;

        for (var index = 0; index < floorEquipments.Count; index++)
        {
            var equipment = floorEquipments[index];

            Markers.Add(new MarkerPosition
            {
                Id = equipment.Id,
                Text = equipment.Name,
                X = originX + (index % maxPerRow) * xStep,
                Y = originY + (index / maxPerRow) * yStep
            });
        }

        AddLog($"Displayed {floor} layout.");
    }
}
