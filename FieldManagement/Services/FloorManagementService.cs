using System.IO;
using System.Text.Json;
using FieldManagement.Models;

namespace FieldManagement.Services;

public sealed class FloorManagementService
{
    private sealed class FloorManagementStore
    {
        public int NextFloorId { get; set; } = 1;
        public List<FloorModel> Floors { get; set; } = new();
        public List<string> Equipments { get; set; } = new();
        public Dictionary<int, List<string>> FloorEquipments { get; set; } = new();
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly object _sync = new();
    private readonly string _storePath;
    private FloorManagementStore _store = new();

    public static FloorManagementService Instance { get; } = new();

    private FloorManagementService()
    {
        _storePath = Path.Combine(AppContext.BaseDirectory, "Data", "floor-management.json");
        Load();
        EnsureSeedData();
    }

    public IReadOnlyList<string> GetFloorNames()
    {
        lock (_sync)
        {
            return _store.Floors
                .OrderBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase)
                .Select(x => x.Name)
                .ToList();
        }
    }

    public IReadOnlyList<string> SearchFloorNames(string keyword)
    {
        lock (_sync)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetFloorNames();

            var trimmed = keyword.Trim();
            return _store.Floors
                .Where(x => x.Name.Contains(trimmed, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(x => x.Name, StringComparer.CurrentCultureIgnoreCase)
                .Select(x => x.Name)
                .ToList();
        }
    }

    public string? FindExactFloorName(string name)
    {
        lock (_sync)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var floor = _store.Floors.FirstOrDefault(x =>
                string.Equals(x.Name, name.Trim(), StringComparison.CurrentCultureIgnoreCase));

            return floor?.Name;
        }
    }

    public string CreateFloor(string name)
    {
        lock (_sync)
        {
            var floorName = NormalizeFloorName(name);

            var existing = _store.Floors.FirstOrDefault(x =>
                string.Equals(x.Name, floorName, StringComparison.CurrentCultureIgnoreCase));
            if (existing is not null)
                return existing.Name;

            var created = new FloorModel
            {
                Id = _store.NextFloorId++,
                Name = floorName
            };

            _store.Floors.Add(created);
            _store.FloorEquipments.TryAdd(created.Id, new List<string>());
            Save();
            return created.Name;
        }
    }

    public IReadOnlyList<string> GetEquipmentsForFloor(string floorName)
    {
        lock (_sync)
        {
            var floor = FindFloorLocked(floorName);
            if (floor is null)
                return Array.Empty<string>();

            if (!_store.FloorEquipments.TryGetValue(floor.Id, out var equipments))
                return Array.Empty<string>();

            return equipments
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }
    }

    public IReadOnlyList<string> GetEquipmentsExceptFloor(string floorName)
    {
        lock (_sync)
        {
            var selected = new HashSet<string>(
                GetEquipmentsForFloor(floorName),
                StringComparer.CurrentCultureIgnoreCase);

            return _store.Equipments
                .Where(x => !selected.Contains(x))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }
    }

    public void SaveFloorEquipments(string floorName, IEnumerable<string> floorEquipments)
    {
        lock (_sync)
        {
            var floor = FindFloorLocked(floorName);
            if (floor is null)
                throw new InvalidOperationException("대상 층이 없습니다.");

            var newEquipments = floorEquipments
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();

            foreach (var list in _store.FloorEquipments.Values)
            {
                list.RemoveAll(x => newEquipments.Contains(x, StringComparer.CurrentCultureIgnoreCase));
            }

            _store.FloorEquipments[floor.Id] = newEquipments;
            Save();
        }
    }

    private FloorModel? FindFloorLocked(string floorName)
    {
        if (string.IsNullOrWhiteSpace(floorName))
            return null;

        return _store.Floors.FirstOrDefault(x =>
            string.Equals(x.Name, floorName.Trim(), StringComparison.CurrentCultureIgnoreCase));
    }

    private static string NormalizeFloorName(string name)
    {
        var floorName = name?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(floorName))
            throw new InvalidOperationException("층명을 입력해주세요.");

        return floorName;
    }

    private void EnsureSeedData()
    {
        lock (_sync)
        {
            if (_store.Equipments.Count == 0)
            {
                _store.Equipments.AddRange(new[] { "A장비", "B장비", "C장비", "D장비", "E장비" });
            }

            if (_store.Floors.Count == 0)
            {
                var floor1 = new FloorModel { Id = _store.NextFloorId++, Name = "1층" };
                var floor2 = new FloorModel { Id = _store.NextFloorId++, Name = "2층" };
                _store.Floors.Add(floor1);
                _store.Floors.Add(floor2);
                _store.FloorEquipments[floor1.Id] = new List<string> { "D장비" };
                _store.FloorEquipments[floor2.Id] = new List<string>();
            }

            foreach (var floor in _store.Floors)
            {
                _store.FloorEquipments.TryAdd(floor.Id, new List<string>());
            }

            Save();
        }
    }

    private void Load()
    {
        lock (_sync)
        {
            try
            {
                if (!File.Exists(_storePath))
                {
                    _store = new FloorManagementStore();
                    return;
                }

                var json = File.ReadAllText(_storePath);
                var loaded = JsonSerializer.Deserialize<FloorManagementStore>(json, JsonOptions);
                _store = loaded ?? new FloorManagementStore();
            }
            catch
            {
                _store = new FloorManagementStore();
            }
        }
    }

    private void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_storePath)!);
        var json = JsonSerializer.Serialize(_store, JsonOptions);
        File.WriteAllText(_storePath, json);
    }
}
