using System.Globalization;
using System.Windows;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Comm.Logger;
using PlantManagement.Dto.v1.Orders;
using PlantManagement.Service.v1.Orders;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddOrderViewModel : BaseViewModel
{
    private readonly IOrderService _orderService;
    private readonly ILogService _logService;
    private List<GetCustomerDto> _customers = [];
    private bool _isEditMode;
    private int _editingOrderSeq;
    private int _editingCustomerSeq;

    public event Action<bool?>? RequestClose;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public bool IsCustomerEditable => !_isEditMode;

    public AddOrderViewModel(IOrderService orderService, ILogService logService)
    {
        _orderService = orderService;
        _logService = logService;
        SaveCommand = new RelayCommand(_ => _ = SaveAsync());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    public void PrepareForAdd()
    {
        _isEditMode = false;
        _editingOrderSeq = 0;
        _editingCustomerSeq = 0;
        OnPropertyChanged(nameof(IsCustomerEditable));

        CustomerName = string.Empty;
        OrderQty = 0;
        StartDt = null;
        EndDt = null;
        AttachmentFilePath = string.Empty;
        ValidationMessage = string.Empty;
    }

    public void PrepareForEdit(OrderViewItems target)
    {
        _isEditMode = true;
        _editingOrderSeq = target.orderSeq;
        _editingCustomerSeq = target.CustomerSeq;
        OnPropertyChanged(nameof(IsCustomerEditable));

        CustomerNames.Clear();
        if (!string.IsNullOrWhiteSpace(target.Customer))
        {
            CustomerNames.Add(target.Customer);
        }

        CustomerName = target.Customer;
        OrderQty = target.OrderQty;
        StartDt = ParseDate(target.StartDt);
        EndDt = ParseDate(target.EndDt);
        AttachmentFilePath = target.PdfFileName ?? string.Empty;
        ValidationMessage = string.Empty;
    }

    public async Task LoadCustomerNamesAsync()
    {
        var rows = await _orderService.GetCustomerService();
        _customers = rows ?? [];

        var names = _customers
            .Where(x => x is not null)
            .Select(x => x.customerName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        void ApplyNames()
        {
            CustomerNames.Clear();
            foreach (var name in names)
            {
                CustomerNames.Add(name);
            }
        }

        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher is null || dispatcher.CheckAccess())
        {
            ApplyNames();
        }
        else
        {
            await dispatcher.InvokeAsync(ApplyNames);
        }
    }

    private async Task SaveAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                ValidationMessage = "고객명은 필수입니다.";
                return;
            }

            if (OrderQty <= 0)
            {
                ValidationMessage = "요청 수량은 1 이상이어야 합니다.";
                return;
            }

            if (StartDt is null)
            {
                ValidationMessage = "시작일자를 확인해주세요.";
                return;
            }

            if (EndDt is null)
            {
                ValidationMessage = "종료일자를 확인해주세요.";
                return;
            }

            var startDt = StartDt.Value;
            var endDt = EndDt.Value;
            if (startDt > endDt)
            {
                ValidationMessage = "시작일자는 종료일자보다 클 수 없습니다.";
                return;
            }

            var customerSeq = _isEditMode ? _editingCustomerSeq : 0;
            GetCustomerDto? customer = null;

            if (customerSeq <= 0)
            {
                if (_customers.Count == 0)
                {
                    await LoadCustomerNamesAsync();
                }

                var selectedName = CustomerName.Trim();
                customer = _customers.FirstOrDefault(x =>
                    x is not null &&
                    string.Equals((x.customerName ?? string.Empty).Trim(), selectedName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                customer = new GetCustomerDto
                {
                    customerSeq = customerSeq,
                    customerName = CustomerName
                };
            }

            if (customer is null)
            {
                ValidationMessage = "선택한 고객명을 찾을 수 없습니다.";
                return;
            }

            var ok = _isEditMode
                ? await _orderService.EditOrderService(new EditOrderDto
                {
                    orderSeq = _editingOrderSeq,
                    customerSeq = customer.customerSeq,
                    orderQty = OrderQty,
                    startDt = startDt,
                    endDt = endDt,
                    attach = AttachmentFilePath ?? string.Empty
                })
                : await _orderService.AddOrderService(new AddOrderDto
                {
                    customerSeq = customer.customerSeq,
                    orderQty = OrderQty,
                    startDt = startDt,
                    endDt = endDt,
                    attach = AttachmentFilePath ?? string.Empty
                });

            if (!ok)
            {
                ValidationMessage = _isEditMode
                    ? "수정에 실패했습니다."
                    : "저장에 실패했습니다.";
                return;
            }

            ValidationMessage = string.Empty;
            RequestClose?.Invoke(true);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            ValidationMessage = $"저장 중 오류가 발생했습니다: {ex.Message}";
        }
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }

    public OrderViewItems BuildOrderViewItem()
    {
        return new OrderViewItems
        {
            orderSeq = _editingOrderSeq,
            CustomerSeq = _editingCustomerSeq,
            Customer = CustomerName,
            OrderQty = OrderQty,
            StartDt = StartDt?.ToString("yyyy-MM-dd") ?? string.Empty,
            EndDt = EndDt?.ToString("yyyy-MM-dd") ?? string.Empty,
            PdfFileName = AttachmentFilePath
        };
    }

    private static DateTime? ParseDate(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var parsed))
        {
            return parsed;
        }

        if (DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
        {
            return parsed;
        }

        return null;
    }
}
