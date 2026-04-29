using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public interface IEquipmentDataService
{
    event EventHandler? DataChanged;

    Task InitializeAsync();

    IReadOnlyList<string> Floors { get; }

    IReadOnlyList<EquipmentViewItems> GetAllEquipments();
    IReadOnlyList<FacilityViewItems> GetFacilities();

    IReadOnlyList<EquipmentViewItems> GetFloorEquipments(string floorName);
    int? GetFloorFacilitySeq(string floorName);

    Task<(bool IsSuccess, string Message)> TryAddFloorAsync(string floorName, int facilitySeq, string? attach);

    Task<(bool IsSuccess, string Message)> UpdateFloorEquipmentsAsync(string floorName, IEnumerable<EquipmentViewItems> equipments);

    Task<bool> SaveFloorPositionsAsync(string floorName, IEnumerable<(int Id, double X, double Y)> positions);

    IReadOnlyDictionary<int, (double X, double Y)> GetFloorPositions(string floorName);
}
