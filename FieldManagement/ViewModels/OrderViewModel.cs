using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class OrderViewModel : BaseViewModel
{
    private ObservableCollection<OrderModel> _orderItems = new();

    public ObservableCollection<OrderModel> OrderItems
    {
        get => _orderItems;
        set
        {
            _orderItems = value;
            OnPropertyChanged();
        }
    }

    private OrderModel? _selectedOrder;

    public OrderModel? SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            _selectedOrder = value;
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

    public OrderViewModel()
    {
        ClosePdfPanelCommand = new RelayCommand(_ => ClosePdfPanel());
        SearchCommand = new RelayCommand(_ => Search());
        ResetCommand = new RelayCommand(_ => Reset());
        AddCommand = new RelayCommand(_ => Add());

        LoadSampleData();
    }

    private void LoadSampleData()
    {
        OrderItems = new ObservableCollection<OrderModel>
        {
            new OrderModel
            {
                Customer = "삼성 SDS",
                OrderQty = 30,
                StartDt = "2026-03-31",
                EndDt = "2027-01-21"
            }
        };
    }
    
    private void Search()
    {
        // 나중에 조회 조건 기반 검색 로직
    }

    private void Reset()
    {
        SelectedOrder = null;
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
        SelectedOrder = null;
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