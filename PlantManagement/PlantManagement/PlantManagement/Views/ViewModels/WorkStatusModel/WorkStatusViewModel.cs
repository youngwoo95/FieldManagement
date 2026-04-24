using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

namespace PlantManagement.Views.ViewModels.WorkStatusModel;

public partial class WorkStatusViewModel : BaseViewModel
{
    private readonly IWorkDialogService _workDialogService;
    
    private static readonly Uri BlankPdfUri = new("about:blank");
    
    public ICommand ClosePdfPanelCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public WorkStatusViewModel(IWorkDialogService workDialogService)
    {
        _workDialogService = workDialogService;

        ClosePdfPanelCommand = new RelayCommand(_ => ClosePdfPanel());
        SearchCommand = new RelayCommand(_ => Search());
        ResetCommand = new RelayCommand(_ => Reset());
        AddCommand = new RelayCommand(_ => Add());

        _filteredWorkStatus = CollectionViewSource.GetDefaultView(_workstatus);
        _filteredWorkStatus.Filter = FilterWorkStatus;

        LoadWorkStatus();
        _filteredWorkStatus.Refresh();
    }

    private void LoadWorkStatus()
    {
        _workstatus.Clear();
        _workstatus.Add(new WorkStatusViewItems
        {
            WorkOrderNo = "WORK_12345",
            CustomerName = "Hanhw Systems",
            MachineName = "A설비",
            Status = "진행중",
            WorkDate = "2026-04-21",
            PdfFileName = "pdf1.pdf"
        });
    }
    

    private bool FilterWorkStatus(object item)
    {
        if (item is not WorkStatusViewItems workstatus) return false;
        var keyword = SearchKeyword?.Trim() ?? string.Empty;
        
        var matchesKeyword =
            string.IsNullOrWhiteSpace(keyword) ||
            (!string.IsNullOrWhiteSpace(workstatus.CustomerName) &&
             workstatus.CustomerName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        
        var matchesDate =
            DateTime.TryParse(workstatus.WorkDate, out var startDate) &&
            startDate.Date >= DateTime.Today;

        return matchesKeyword || matchesDate;
    }
    

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;
        SelectedWorkStatus = null;
        SelectedPdfPath = null;
    }
    
    private void Search()
    {
        _filteredWorkStatus.Refresh();
    }
    
    private void Reset()
    {
        SelectedWorkStatus = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;
    }
    
    private void Add()
    {
        var newOrder = _workDialogService.ShowAddWorkStatusDialog();
        if (newOrder is null)
            return;

        if (string.IsNullOrWhiteSpace(newOrder.PdfFileName))
            newOrder.PdfFileName = "pdf1.pdf";

        _workstatus.Add(newOrder);
        _filteredWorkStatus.Refresh();
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

    private static string ResolvePdfPath(string pdfFileName)
    {
        var fileName = string.IsNullOrWhiteSpace(pdfFileName) ? "pdf1.pdf" : pdfFileName.Trim();

        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Comm", "Pdfs", fileName),
            Path.Combine(Directory.GetCurrentDirectory(), "Comm", "Pdfs", fileName),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Comm", "Pdfs", fileName),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Comm", "Pdfs", fileName)
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