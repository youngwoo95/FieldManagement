using System.Collections.ObjectModel;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddOrderViewModel
{
    private readonly ObservableCollection<string> _customerNames = [];
    private string _customerName = string.Empty;
    private int _orderQty;
    private DateTime? _startDt;
    private DateTime? _endDt;
    private string _attachmentFilePath = string.Empty;
    private string _validationMessage = string.Empty;

    public ObservableCollection<string> CustomerNames => _customerNames;

    public string CustomerName
    {
        get => _customerName;
        set => SetField(ref _customerName, value);
    }

    public int OrderQty
    {
        get => _orderQty;
        set => SetField(ref _orderQty, value);
    }

    public DateTime? StartDt
    {
        get => _startDt;
        set => SetField(ref _startDt, value);
    }

    public DateTime? EndDt
    {
        get => _endDt;
        set => SetField(ref _endDt, value);
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
}
