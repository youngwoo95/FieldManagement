using PlantManagement.Service.v1.Facility;
using PlantManagement.Service.v1.Floor;
using PlantManagement.Service.v1.Lamp;
using PlantManagement.Dto.v1.Floor;
using PlantManagement.ViewItems;
using System.Windows;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public class EquipmentDataService : IEquipmentDataService
{
    private readonly ILampService _lampService;
    private readonly IFacilityService _facilityService;
    private readonly IFloorService _floorService;
    private readonly SemaphoreSlim _loadLock = new(1, 1);
    private readonly Dictionary<int, EquipmentViewItems> _allLampsPool = new(); // lampSeq → item (모든 램프)
    private readonly List<FacilityViewItems> _facilities = [];
    private bool _isLoaded;

    private readonly Dictionary<string, List<int>> _floorEquipmentMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _floorFacilityMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> _floorSeqMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Dictionary<int, (double X, double Y)>> _floorPositionMap = new(StringComparer.OrdinalIgnoreCase);

    public EquipmentDataService(
        ILampService lampService,
        IFacilityService facilityService,
        IFloorService floorService)
    {
        _lampService = lampService;
        _facilityService = facilityService;
        _floorService = floorService;
    }

    public event EventHandler? DataChanged;

    public async Task InitializeAsync()
    {
        if (_isLoaded)
        {
            return;
        }

        await _loadLock.WaitAsync();
        try
        {
            if (_isLoaded)
            {
                return;
            }

            var lampTask = _lampService.GetLampService();
            var facilityTask = _facilityService.GetFacilityService();
            var floorTask = _floorService.GetFloorService();
            await Task.WhenAll(lampTask, facilityTask, floorTask).ConfigureAwait(false);

            var lamps = lampTask.Result ?? [];
            var facilities = facilityTask.Result ?? [];
            var floors = floorTask.Result ?? [];

            _facilities.Clear();
            foreach (var facility in facilities)
            {
                _facilities.Add(new FacilityViewItems
                {
                    Seq = facility.facilitySeq,
                    Name = facility.facilityName ?? string.Empty,
                    Maker = facility.maker ?? string.Empty,
                    Purpose = facility.purpose ?? string.Empty
                });
            }

            // 각 층에 배정된 램프 목록을 순차적으로 조회 (동일 DB 연결 공유로 동시 실행 불가)
            var validFloors = floors
                .Where(floor => !string.IsNullOrWhiteSpace(floor.name))
                .ToList();

            // 전체 램프 풀 구성: 미배정 + 각 층 배정 램프 모두
            _allLampsPool.Clear();
            foreach (var lamp in lamps)
            {
                _allLampsPool[lamp.lampSeq] = new EquipmentViewItems
                {
                    Id = lamp.lampSeq,
                    Name = lamp.lampName ?? string.Empty
                };
            }

            _floorEquipmentMap.Clear();
            _floorSeqMap.Clear();
            _floorPositionMap.Clear();
            foreach (var floor in validFloors)
            {
                var floorName = floor.name!.Trim();
                var floorLamps = await _lampService.GetFloorLampService(floor.floorSeq).ConfigureAwait(false) ?? [];

                foreach (var lamp in floorLamps)
                {
                    _allLampsPool.TryAdd(lamp.lampSeq, new EquipmentViewItems
                    {
                        Id = lamp.lampSeq,
                        Name = lamp.lampName ?? string.Empty
                    });
                }

                _floorEquipmentMap[floorName] = floorLamps.Select(l => l.lampSeq).ToList();
                _floorSeqMap[floorName] = floor.floorSeq;
                _floorPositionMap[floorName] = floorLamps.ToDictionary(
                    l => l.lampSeq,
                    l => (l.positionX, l.positionY));
            }

            _floorFacilityMap.Clear();
            if (_facilities.Count > 0)
            {
                var defaultFacilitySeq = _facilities[0].Seq;
                foreach (var floor in _floorEquipmentMap.Keys)
                {
                    _floorFacilityMap[floor] = defaultFacilitySeq;
                }
            }

            _isLoaded = true;
        }
        finally
        {
            _loadLock.Release();
        }
    }

    public IReadOnlyList<string> Floors =>
        _floorEquipmentMap
            .Keys
            .OrderBy(static floor => floor)
            .ToList();

    public IReadOnlyList<EquipmentViewItems> GetAllEquipments()
    {
        if (!_isLoaded)
        {
            return [];
        }

        var assignedAnywhere = _floorEquipmentMap.Values
            .SelectMany(ids => ids)
            .ToHashSet();

        return _allLampsPool.Values
            .Where(e => !assignedAnywhere.Contains(e.Id))
            .ToList();
    }

    public IReadOnlyList<FacilityViewItems> GetFacilities()
    {
        if (!_isLoaded)
        {
            return [];
        }

        return _facilities;
    }

    public IReadOnlyList<EquipmentViewItems> GetFloorEquipments(string floorName)
    {
        if (!_isLoaded)
        {
            return [];
        }

        if (!_floorEquipmentMap.TryGetValue(floorName, out var equipmentIds))
        {
            return [];
        }

        var result = new List<EquipmentViewItems>(equipmentIds.Count);
        foreach (var id in equipmentIds)
        {
            if (_allLampsPool.TryGetValue(id, out var item))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public int? GetFloorFacilitySeq(string floorName)
    {
        if (!_isLoaded)
        {
            return null;
        }

        if (_floorFacilityMap.TryGetValue(floorName, out var facilitySeq))
        {
            return facilitySeq;
        }

        return null;
    }

    public async Task<(bool IsSuccess, string Message)> TryAddFloorAsync(string floorName, int facilitySeq, string? attach)
    {
        var normalizedFloor = floorName.Trim();
        if (string.IsNullOrWhiteSpace(normalizedFloor))
        {
            return (false, "Please enter a floor name.");
        }

        if (_floorEquipmentMap.ContainsKey(normalizedFloor))
        {
            return (false, $"Floor already exists: {normalizedFloor}");
        }

        if (!_facilities.Any(item => item.Seq == facilitySeq))
        {
            return (false, $"Facility does not exist: {facilitySeq}");
        }

        var isAdded = await _floorService.AddFloorService(new AddFloorDto
        {
            floorName = normalizedFloor,
            attach = string.IsNullOrWhiteSpace(attach) ? null : attach.Trim()
        }).ConfigureAwait(false);
        if (!isAdded)
        {
            return (false, $"Failed to create floor: {normalizedFloor}");
        }

        _floorEquipmentMap[normalizedFloor] = [];
        _floorFacilityMap[normalizedFloor] = facilitySeq;
        RaiseDataChanged();

        var facilityName = _facilities.First(item => item.Seq == facilitySeq).Name;
        return (true, $"Created floor: {normalizedFloor} (Facility: {facilityName})");
    }

    public async Task<(bool IsSuccess, string Message)> UpdateFloorEquipmentsAsync(string floorName, IEnumerable<EquipmentViewItems> equipments)
    {
        if (!_isLoaded)
        {
            return (false, "Equipment list is loading. Try again.");
        }

        if (!_floorEquipmentMap.ContainsKey(floorName))
        {
            return (false, $"Floor does not exist: {floorName}");
        }

        if (!_floorSeqMap.TryGetValue(floorName, out var floorSeq))
        {
            return (false, $"Floor seq not found: {floorName}");
        }

        var updatedIds = equipments
            .Select(item => item.Id)
            .Where(_allLampsPool.ContainsKey)
            .Distinct()
            .ToList();

        _floorPositionMap.TryGetValue(floorName, out var posMap);
        var entries = updatedIds.Select(id =>
        {
            var pos = posMap != null && posMap.TryGetValue(id, out var p) ? p : (X: 0.0, Y: 0.0);
            return (id, pos.X, pos.Y);
        }).ToList();

        var saved = await _lampService.UpdateFloorLampService(floorSeq, entries).ConfigureAwait(false);
        if (!saved)
        {
            return (false, $"저장 실패: {floorName}");
        }

        _floorEquipmentMap[floorName] = updatedIds;
        RaiseDataChanged();

        return (true, $"{floorName} 램프 배정이 저장되었습니다.");
    }

    public IReadOnlyDictionary<int, (double X, double Y)> GetFloorPositions(string floorName)
    {
        if (_floorPositionMap.TryGetValue(floorName, out var posMap))
        {
            return posMap;
        }

        return new Dictionary<int, (double X, double Y)>();
    }

    public async Task<bool> SaveFloorPositionsAsync(string floorName, IEnumerable<(int Id, double X, double Y)> positions)
    {
        if (!_isLoaded) return false;
        if (!_floorSeqMap.TryGetValue(floorName, out var floorSeq)) return false;
        if (!_floorEquipmentMap.TryGetValue(floorName, out var assignedIds)) return false;

        var posDict = positions.ToDictionary(p => p.Id, p => (p.X, p.Y));

        var entries = assignedIds.Select(id =>
        {
            var pos = posDict.TryGetValue(id, out var p) ? p : (X: 0.0, Y: 0.0);
            return (id, pos.X, pos.Y);
        }).ToList();

        var saved = await _lampService.UpdateFloorLampService(floorSeq, entries).ConfigureAwait(false);
        if (saved)
        {
            _floorPositionMap[floorName] = posDict;
        }

        return saved;
    }

    private void RaiseDataChanged()
    {
        var handler = DataChanged;
        if (handler is null)
        {
            return;
        }

        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher is null || dispatcher.CheckAccess())
        {
            handler(this, EventArgs.Empty);
            return;
        }

        dispatcher.Invoke(() => handler(this, EventArgs.Empty));
    }
}
