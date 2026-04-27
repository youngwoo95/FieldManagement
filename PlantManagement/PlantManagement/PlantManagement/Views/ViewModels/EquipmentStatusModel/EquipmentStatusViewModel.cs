using System.Collections.ObjectModel;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Comm.Behaviors;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel;

public partial class EquipmentStatusViewModel : BaseViewModel
{
    public ICommand EditCommand { get; }
    public ICommand SaveCommand { get; }

    public EquipmentStatusViewModel()
    {
        Floors = new ObservableCollection<string> { "1F", "2F", "3F" };
        Markers = new ObservableCollection<MarkerPosition>();
        Logs = new ObservableCollection<string>();

        EditCommand = new RelayCommand(_ => SetEditMode(true));
        SaveCommand = new RelayCommand(_ => SetEditMode(false));

        SelectedFloor = Floors[0];
        AddLog("설비 현황판을 불러왔습니다.");
    }

    private void SetEditMode(bool enabled)
    {
        IsEditMode = enabled;
        AddLog(enabled ? "편집 모드로 전환했습니다." : "저장 후 조회 모드로 전환했습니다.");
    }

    private void AddLog(string message)
    {
        Logs.Insert(0, $"{DateTime.Now:HH:mm:ss}  {message}");
    }

    private void LoadMarkersForFloor(string floor)
    {
        Markers.Clear();

        var floorMarkers = floor switch
        {
            "2F" => new[]
            {
                new MarkerPosition { Id = 201, Text = "M-201", X = 120, Y = 110 },
                new MarkerPosition { Id = 202, Text = "M-202", X = 310, Y = 170 },
                new MarkerPosition { Id = 203, Text = "M-203", X = 500, Y = 220 }
            },
            "3F" => new[]
            {
                new MarkerPosition { Id = 301, Text = "M-301", X = 150, Y = 120 },
                new MarkerPosition { Id = 302, Text = "M-302", X = 360, Y = 200 },
                new MarkerPosition { Id = 303, Text = "M-303", X = 560, Y = 250 }
            },
            _ => new[]
            {
                new MarkerPosition { Id = 101, Text = "M-101", X = 100, Y = 100 },
                new MarkerPosition { Id = 102, Text = "M-102", X = 280, Y = 160 },
                new MarkerPosition { Id = 103, Text = "M-103", X = 470, Y = 210 }
            }
        };

        foreach (var marker in floorMarkers)
        {
            Markers.Add(marker);
        }

        AddLog($"{floor} 배치도를 표시했습니다.");
    }
}
