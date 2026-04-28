using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public interface IEquipmentDataService
{
    event EventHandler? DataChanged;

    IReadOnlyList<string> Floors { get; }

    IReadOnlyList<EquipmentViewItems> GetAllEquipments();

    IReadOnlyList<EquipmentViewItems> GetFloorEquipments(string floorName);

    bool TryAddFloor(string floorName, out string message);

    bool UpdateFloorEquipments(string floorName, IEnumerable<EquipmentViewItems> equipments, out string message);
}
