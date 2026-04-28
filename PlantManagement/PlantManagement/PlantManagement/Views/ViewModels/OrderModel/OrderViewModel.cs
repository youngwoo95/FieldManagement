using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Service.v1.Orders;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.OrderModel.Dialog;

namespace PlantManagement.Views.ViewModels.OrderModel;

public partial class OrderViewModel : BaseViewModel
{
    private readonly IOrderDialogService _orderDialogService;
    private readonly IOrderService _orderService;
    
    private static readonly Uri BlankPdfUri = new("about:blank");
    public ICommand ClosePdfPanelCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand AddCommand { get; }

    public OrderViewModel(IOrderDialogService orderDialogService,
        IOrderService orderService)
    {
        _orderDialogService = orderDialogService;
        _orderService = orderService;
        
        ClosePdfPanelCommand = new RelayCommand(_ => ClosePdfPanel());
        EditCommand = new RelayCommand(_ => _ = EditAsync());
        ResetCommand = new RelayCommand(_ => Reset());
        AddCommand = new RelayCommand(_ => _ = AddAsync());

        _filteredOrders = CollectionViewSource.GetDefaultView(_orders);
        _filteredOrders.Filter = FilterOrder;

        _ = ReloadAsync();
        
        _filteredOrders.Refresh();
    }
    
    public async Task ReloadAsync()
    {
        await LoadOrders();
        _filteredOrders.Refresh();
    }

    private async Task LoadOrders()
    {
        var model = await _orderService.GetOrderService();
        var rows = model ?? [];
        
        _orders.Clear();
        foreach (var item in rows)
        {
            _orders.Add(new OrderViewItems
            {
                orderSeq = item.orderSeq,
                CustomerSeq = item.customerSeq,
                Customer = item.name,
                StartDt = item.startDt,
                EndDt = item.endDt,
                OrderQty = item.orderQty,
                PdfFileName = item.attach
            });
        }
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

    private async Task AddAsync()
    {
        var newOrder = _orderDialogService.ShowAddOrderDialog();
        if (newOrder is null)
        {
            return;
        }

        await ReloadAsync();
    }

    private async Task EditAsync()
    {
        if (SelectedOrder is null)
        {
            return;
        }

        var edited = _orderDialogService.ShowEditOrderDialog(SelectedOrder);
        if (edited is null)
        {
            return;
        }

        await ReloadAsync();
    }

    private void UpdatePdfPreviewState(string? pdfPath)
    {
        if (pdfPath is null)
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = "Select an order to preview PDF.";
            return;
        }

        if (string.IsNullOrWhiteSpace(pdfPath))
        {
            SelectedPdfUri = BlankPdfUri;
            IsPdfFallbackVisible = true;
            PdfFallbackMessage = "첨부파일이 없습니다.";
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
                return fullPath;
        }

        return Path.GetFullPath(candidates[0]);
    }
}
