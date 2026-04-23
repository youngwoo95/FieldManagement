using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.OrderModel.Dialog;

namespace PlantManagement.Views.ViewModels.OrderModel;

public partial class OrderViewModel : BaseViewModel
{
    private readonly IOrderDialogService _orderDialogService;

    private static readonly Uri BlankPdfUri = new("about:blank");
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

        _filteredOrders = CollectionViewSource.GetDefaultView(_orders);
        _filteredOrders.Filter = FilterOrder;

        LoadOrders();
        _filteredOrders.Refresh();
    }

    private void LoadOrders()
    {
        _orders.Clear();
        _orders.Add(new OrderViewItems
        {
            Customer = "Hanwha Systems",
            OrderQty = 20,
            StartDt = "2026-04-01",
            EndDt = "2026-04-30",
            PdfFileName = "pdf1.pdf"
        });
        _orders.Add(new OrderViewItems
        {
            Customer = "SK Interfaces",
            OrderQty = 50,
            StartDt = "2026-03-01",
            EndDt = "2026-03-30",
            PdfFileName = "pdf2.pdf"
        });
    }

    private bool FilterOrder(object item)
    {
        if (item is not OrderViewItems order) return false;
        var keyword = SearchKeyword?.Trim() ?? string.Empty;

        var matchesKeyword =
            string.IsNullOrWhiteSpace(keyword) ||
            (!string.IsNullOrWhiteSpace(order.Customer) &&
             order.Customer.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        var matchesDate =
            DateTime.TryParse(order.StartDt, out var startDate) &&
            startDate.Date >= DateTime.Today;

        return matchesKeyword || matchesDate;
    }

    private void ClosePdfPanel()
    {
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;
        SelectedOrder = null;
        SelectedPdfPath = null;
    }

    private void Search()
    {
        _filteredOrders.Refresh();
    }

    private void Reset()
    {
        SelectedOrder = null;
        SelectedPdfPath = null;
        IsPdfPanelOpen = false;
        PdfPanelWidth = 0;
    }

    private void Add()
    {
        var newOrder = _orderDialogService.ShowAddOrderDialog();
        if (newOrder is null)
            return;

        if (string.IsNullOrWhiteSpace(newOrder.PdfFileName))
            newOrder.PdfFileName = "pdf1.pdf";

        _orders.Add(newOrder);
        _filteredOrders.Refresh();
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
