using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class OrderViewModel : BaseViewModel
{
    private static readonly Uri BlankPdfUri = new("about:blank");
    private readonly IOrderDialogService _orderDialogService;

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

            if (value is null)
                return;

            SelectedPdfPath = ResolvePdfPath();
            IsPdfPanelOpen = true;
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
            UpdatePdfPreviewState(value);
        }
    }

    private Uri _selectedPdfUri = BlankPdfUri;
    public Uri SelectedPdfUri
    {
        get => _selectedPdfUri;
        private set
        {
            _selectedPdfUri = value;
            OnPropertyChanged();
        }
    }

    private bool _isPdfFallbackVisible;
    public bool IsPdfFallbackVisible
    {
        get => _isPdfFallbackVisible;
        private set
        {
            _isPdfFallbackVisible = value;
            OnPropertyChanged();
        }
    }

    private string _pdfFallbackMessage = "PDF preview is unavailable.";
    public string PdfFallbackMessage
    {
        get => _pdfFallbackMessage;
        private set
        {
            _pdfFallbackMessage = value;
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
            OnPropertyChanged(nameof(PdfPanelWidth));
        }
    }

    public double PdfPanelWidth => IsPdfPanelOpen ? 500 : 0;

    public ICommand ClosePdfPanelCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public OrderViewModel(IOrderDialogService orderDialogService)
    {
        _orderDialogService = orderDialogService;

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
            new()
            {
                Customer = "Samsung SDS",
                OrderQty = 30,
                StartDt = "2026-03-31",
                EndDt = "2027-01-21"
            }
        };
    }

    private void Search()
    {
        // TODO: apply filter by customer/start/end date.
    }

    private void Reset()
    {
        SelectedOrder = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
    }

    private void Add()
    {
        var created = _orderDialogService.ShowAddOrderDialog();
        if (created is null)
            return;

        OrderItems.Insert(0, created);
        SelectedOrder = created;
    }

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        SelectedOrder = null;
        SelectedPdfPath = null;
    }

    private void UpdatePdfPreviewState(string? pdfPath)
    {
        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = "Select an order to preview PDF.";
            return;
        }

        var fullPath = Path.GetFullPath(pdfPath);
        if (!File.Exists(fullPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = $"PDF file not found.{Environment.NewLine}{fullPath}";
            return;
        }

        SelectedPdfUri = new Uri(fullPath, UriKind.Absolute);
        IsPdfFallbackVisible = false;
        PdfFallbackMessage = string.Empty;
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
