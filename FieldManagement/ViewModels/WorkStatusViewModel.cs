using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class WorkStatusViewModel : BaseViewModel
{
    private static readonly Uri BlankPdfUri = new("about:blank");
    private readonly IWorkStatusDialogService _workStatusDialogService;

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

    public WorkStatusViewModel(IWorkStatusDialogService workStatusDialogService)
    {
        _workStatusDialogService = workStatusDialogService;

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
            new()
            {
                WorkOrderNo = "WO-20260420-001",
                MachineName = "MA-0001",
                CustomerName = "Samsung SDI",
                Status = "In Progress",
                WorkDate = "2026-04-20"
            }
        };
    }

    private void Search()
    {
        // TODO: apply filter by machine/customer/date/status.
    }

    private void Reset()
    {
        SelectedWorkOrder = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
    }

    private void Add()
    {
        var created = _workStatusDialogService.ShowAddWorkStatusDialog();
        if (created is null)
            return;

        WorkOrders.Insert(0, created);
        SelectedWorkOrder = created;
    }

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        SelectedWorkOrder = null;
        SelectedPdfPath = null;
    }

    private void UpdatePdfPreviewState(string? pdfPath)
    {
        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            return;
        }

        var fullPath = Path.GetFullPath(pdfPath);
        if (!File.Exists(fullPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            return;
        }

        SelectedPdfUri = new Uri(fullPath, UriKind.Absolute);
        IsPdfFallbackVisible = false;
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
