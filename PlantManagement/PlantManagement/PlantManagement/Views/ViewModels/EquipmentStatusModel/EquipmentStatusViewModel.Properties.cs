using System.Collections.ObjectModel;
using PlantManagement.Comm.Behaviors;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public partial class EquipmentStatusViewModel
{
    private string _selectedFloor = string.Empty;
    private bool _isEditMode;

    public ObservableCollection<string> Floors { get; private set; } = new();
    public ObservableCollection<MarkerPosition> Markers { get; private set; } = new();
    public ObservableCollection<string> Logs { get; private set; } = new();

    public string SelectedFloor
    {
        get => _selectedFloor;
        set
        {
            if (!SetField(ref _selectedFloor, value))
            {
                return;
            }

            LoadMarkersForFloor(value);
        }
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        private set => SetField(ref _isEditMode, value);
    }
}
