using System.Collections.ObjectModel;
using System.IO;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public class WorkOrderInfoOption
{
    public int OrderSeq { get; init; }
    public int CustomerSeq { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string PdfFileName { get; init; } = string.Empty;
    public string DisplayText => $"[{OrderSeq}] {CustomerName}";
}

public partial class AddWorkStatusViewModel
{
    private static readonly Uri BlankPdfUri = new("about:blank");

    private readonly ObservableCollection<string> _facilityNames = new();
    private readonly ObservableCollection<string> _customerNames = new();
    private readonly ObservableCollection<WorkOrderInfoOption> _orderInfos = new();

    private string _workOrderNo = string.Empty;
    private string _machineName = string.Empty;
    private string _customerName = string.Empty;
    private string _status = "InProgress";
    private string _workDate = DateTime.Today.ToString("yyyy-MM-dd");
    private string _attachmentFilePath = string.Empty;
    private string _validationMessage = string.Empty;
    private WorkOrderInfoOption? _selectedOrderInfo;

    private string? _selectedPdfPath;
    private Uri _selectedPdfUri = BlankPdfUri;
    private bool _isPdfFallbackVisible = true;
    private string _pdfFallbackMessage = "Select an order to preview PDF.";
    private string _selectedOrderSummary = string.Empty;
    
    public ObservableCollection<string> FacilityNames => _facilityNames;
    public ObservableCollection<string> CustomerNames => _customerNames;
    public ObservableCollection<WorkOrderInfoOption> OrderInfos => _orderInfos;

    public string WorkOrderNo
    {
        get => _workOrderNo;
        set => SetField(ref _workOrderNo, value);
    }

    public string MachineName
    {
        get => _machineName;
        set => SetField(ref _machineName, value);
    }

    public string CustomerName
    {
        get => _customerName;
        set => SetField(ref _customerName, value);
    }

    public string Status
    {
        get => _status;
        set => SetField(ref _status, value);
    }

    public string WorkDate
    {
        get => _workDate;
        set => SetField(ref _workDate, value);
    }

    public string AttachmentFilePath
    {
        get => _attachmentFilePath;
        set => SetField(ref _attachmentFilePath, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetField(ref _validationMessage, value);
    }

    public WorkOrderInfoOption? SelectedOrderInfo
    {
        get => _selectedOrderInfo;
        set
        {
            if (!SetField(ref _selectedOrderInfo, value))
            {
                return;
            }

            if (value is null)
            {
                CustomerName = string.Empty;
                AttachmentFilePath = string.Empty;
                SelectedOrderSummary = string.Empty;
                SelectedPdfPath = null;
                return;
            }

            CustomerName = value.CustomerName;
            AttachmentFilePath = value.PdfFileName;
            SelectedOrderSummary = $"Order: {value.OrderSeq} / Customer: {value.CustomerName}";
            SelectedPdfPath = ResolvePdfPath(value.PdfFileName);
        }
    }

    public string? SelectedPdfPath
    {
        get => _selectedPdfPath;
        set
        {
            if (!SetField(ref _selectedPdfPath, value))
            {
                return;
            }

            UpdatePdfPreviewState(value);
        }
    }

    public Uri SelectedPdfUri
    {
        get => _selectedPdfUri;
        private set => SetField(ref _selectedPdfUri, value);
    }

    public bool IsPdfFallbackVisible
    {
        get => _isPdfFallbackVisible;
        private set => SetField(ref _isPdfFallbackVisible, value);
    }

    public string PdfFallbackMessage
    {
        get => _pdfFallbackMessage;
        private set => SetField(ref _pdfFallbackMessage, value);
    }

    public string SelectedOrderSummary
    {
        get => _selectedOrderSummary;
        private set => SetField(ref _selectedOrderSummary, value);
    }

    private void UpdatePdfPreviewState(string? pdfPath)
    {
        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = "No PDF is attached for this order.";
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
        if (string.IsNullOrWhiteSpace(pdfFileName))
        {
            return string.Empty;
        }

        var fileName = pdfFileName.Trim();

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
            {
                return fullPath;
            }
        }

        return Path.GetFullPath(candidates[0]);
    }
 
}
