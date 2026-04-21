using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class DataViewModel : BaseViewModel
{
    private ObservableCollection<WorkerModel> _workOrders = new();
    public ObservableCollection<WorkerModel> WorkOrders
    {
        get => _workOrders;
        set
        {
            _workOrders = value;
            OnPropertyChanged();
        }
    }

    private WorkerModel? _selectedWorkOrder;
    public WorkerModel? SelectedWorkOrder
    {
        get => _selectedWorkOrder;
        set
        {
            _selectedWorkOrder = value;
            OnPropertyChanged();

            if (value is not null)
            {
                SelectedPdfPath = ResolvePdfPath();
                IsPdfPanelOpen = true;
            }
        }
    }

    private string? _selectedPdfPath;
    public string? SelectedPdfPath
    {
        get => _selectedPdfPath;
        set
        {
            _selectedPdfPath = value;
            OnPropertyChanged();
        }
    }

    private bool _isPdfPanelOpen;
    public bool IsPdfPanelOpen
    {
        get => _isPdfPanelOpen;
        set
        {
            _isPdfPanelOpen = value;
            OnPropertyChanged();
        }
    }

    public ICommand ClosePdfPanelCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public DataViewModel()
    {
        ClosePdfPanelCommand = new RelayCommand(_ => ClosePdfPanel());
        SearchCommand = new RelayCommand(_ => Search());
        ResetCommand = new RelayCommand(_ => Reset());
        AddCommand = new RelayCommand(_ => Add());

        LoadSampleData();
    }

    private void LoadSampleData()
    {
        WorkOrders = new ObservableCollection<WorkerModel>
        {
            new WorkerModel
            {
                WorkOrderNo = "WO-20260420-001",
                MachineName = "MA-0001",
                CustomerName = "장성운",
                Status = "진행중",
                WorkDate = "2026-04-20"
            }
        };
    }

    private void Search()
    {
        // 나중에 조회 조건 기반 검색 로직
    }

    private void Reset()
    {
        SelectedWorkOrder = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
    }

    private void Add()
    {
        // 나중에 등록 로직
    }

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        SelectedWorkOrder = null;
    }

    private static string ResolvePdfPath()
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Pdfs", "Notion.pdf"),
            Path.Combine(Directory.GetCurrentDirectory(), "Pdfs", "Notion.pdf"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Pdfs", "Notion.pdf")
        };

        foreach (var candidate in candidates)
        {
            var fullPath = Path.GetFullPath(candidate);
            if (File.Exists(fullPath))
                return fullPath;
        }

        return Path.GetFullPath(candidates[0]);
    }
}