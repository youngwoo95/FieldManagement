using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public class EquipmentDataService : IEquipmentDataService
{
    private readonly List<EquipmentViewItems> _allEquipments =
    [
        new() { Id = 101, Name = "M-101" },
        new() { Id = 102, Name = "M-102" },
        new() { Id = 103, Name = "M-103" },
        new() { Id = 201, Name = "M-201" },
        new() { Id = 202, Name = "M-202" },
        new() { Id = 203, Name = "M-203" },
        new() { Id = 301, Name = "M-301" },
        new() { Id = 302, Name = "M-302" },
        new() { Id = 303, Name = "M-303" }
    ];

    private readonly Dictionary<string, List<int>> _floorEquipmentMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["1F"] = [101, 102, 103],
        ["2F"] = [201, 202, 203],
        ["3F"] = [301, 302, 303]
    };

    public event EventHandler? DataChanged;

    public IReadOnlyList<string> Floors =>
        _floorEquipmentMap
            .Keys
            .OrderBy(static floor => floor)
            .ToList();

    public IReadOnlyList<EquipmentViewItems> GetAllEquipments()
    {
        return _allEquipments;
    }

    public IReadOnlyList<EquipmentViewItems> GetFloorEquipments(string floorName)
    {
        if (!_floorEquipmentMap.TryGetValue(floorName, out var equipmentIds))
        {
            return [];
        }

        var equipmentLookup = _allEquipments.ToDictionary(item => item.Id);
        var result = new List<EquipmentViewItems>(equipmentIds.Count);

        foreach (var equipmentId in equipmentIds)
        {
            if (equipmentLookup.TryGetValue(equipmentId, out var equipment))
            {
                result.Add(equipment);
            }
        }

        return result;
    }

    public bool TryAddFloor(string floorName, out string message)
    {
        var normalizedFloor = floorName.Trim();
        if (string.IsNullOrWhiteSpace(normalizedFloor))
        {
            message = "층명을 입력해 주세요.";
            return false;
        }

        if (_floorEquipmentMap.ContainsKey(normalizedFloor))
        {
            message = $"이미 등록된 층입니다: {normalizedFloor}";
            return false;
        }

        _floorEquipmentMap[normalizedFloor] = [];
        RaiseDataChanged();

        message = $"층이 생성되었습니다: {normalizedFloor}";
        return true;
    }

    public bool UpdateFloorEquipments(string floorName, IEnumerable<EquipmentViewItems> equipments, out string message)
    {
        if (!_floorEquipmentMap.ContainsKey(floorName))
        {
            message = $"존재하지 않는 층입니다: {floorName}";
            return false;
        }

        var validIds = new HashSet<int>(_allEquipments.Select(item => item.Id));
        var updatedIds = equipments
            .Select(item => item.Id)
            .Where(validIds.Contains)
            .Distinct()
            .ToList();

        _floorEquipmentMap[floorName] = updatedIds;
        RaiseDataChanged();

        message = $"{floorName} 장비 구성이 저장되었습니다.";
        return true;
    }

    private void RaiseDataChanged()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
}
