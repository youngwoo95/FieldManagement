using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Service.v1.Works;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels;
using PlantManagement.Views.ViewModels.WorkStatusModel.Dialog;

namespace PlantManagement.Views.ViewModels.WorkStatusModel;

public partial class WorkStatusViewModel : BaseViewModel, IReloadableViewModel
{
    private readonly IWorkDialogService _workDialogService;
    private readonly IWorkService _workService;

    private static readonly Uri BlankPdfUri = new("about:blank");

    public ICommand ClosePdfPanelCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public WorkStatusViewModel(
        IWorkDialogService workDialogService,
        IWorkService workService)
    {
        _workDialogService = workDialogService;
        _workService = workService;

        ClosePdfPanelCommand = new RelayCommand(_ => ClosePdfPanel());
        SearchCommand = new RelayCommand(_ => _ = SearchAsync());
        ResetCommand = new RelayCommand(_ => _ = ResetAsync());
        AddCommand = new RelayCommand(_ => _ = AddAsync());

        _filteredWorkStatus = CollectionViewSource.GetDefaultView(_workstatus);
        _filteredWorkStatus.Filter = FilterWorkStatus;

        _ = ReloadAsync();
    }

    public async Task ReloadAsync()
    {
        var rows = await _workService.GetWorksListService() ?? [];

        _workstatus.Clear();
        foreach (var item in rows)
        {
            _workstatus.Add(new WorkStatusViewItems
            {
                WorkOrderNo = item.workSeq ?? string.Empty,
                MachineName = item.facilityName ?? string.Empty,
                CustomerName = item.customerName ?? string.Empty,
                Status = item.statusName ?? string.Empty,
                WorkDate = item.startDt.ToString("yyyy-MM-dd"),
                PdfFileName = item.attach ?? string.Empty
            });
        }

        _filteredWorkStatus.Refresh();
    }

    private bool FilterWorkStatus(object item)
    {
        if (item is not WorkStatusViewItems workstatus)
        {
            return false;
        }

        var workOrderKeyword = SearchKeyword?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(workOrderKeyword) &&
            !workstatus.WorkOrderNo.Contains(workOrderKeyword, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var machineKeyword = MachineKeyword?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(machineKeyword) &&
            !workstatus.MachineName.Contains(machineKeyword, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var customerKeyword = CustomerKeyword?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(customerKeyword) &&
            !workstatus.CustomerName.Contains(customerKeyword, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (DateTime.TryParse(workstatus.WorkDate, out var workDate))
        {
            if (StartDateFilter.HasValue && workDate.Date < StartDateFilter.Value.Date)
            {
                return false;
            }

            if (EndDateFilter.HasValue && workDate.Date > EndDateFilter.Value.Date)
            {
                return false;
            }
        }

        var selectedStatus = SelectedStatusFilter?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(selectedStatus))
        {
            var hasRealStatus =
                _workstatus.Any(x => string.Equals(x.Status, selectedStatus, StringComparison.OrdinalIgnoreCase));

            if (hasRealStatus &&
                !string.Equals(workstatus.Status, selectedStatus, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;
        SelectedWorkStatus = null;
        SelectedPdfPath = null;
    }

    private async Task SearchAsync()
    {
        await ReloadAsync();
        _filteredWorkStatus.Refresh();
    }

    private async Task ResetAsync()
    {
        SearchKeyword = string.Empty;
        MachineKeyword = string.Empty;
        CustomerKeyword = string.Empty;
        StartDateFilter = null;
        EndDateFilter = null;
        SelectedStatusFilter = string.Empty;

        SelectedWorkStatus = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;

        await ReloadAsync();
    }

    private async Task AddAsync()
    {
        var newOrder = await _workDialogService.ShowAddWorkStatusDialogAsync();
        if (newOrder is null)
            return;

        await ReloadAsync();
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
