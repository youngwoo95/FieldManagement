using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveChartsCore.SkiaSharpView.WPF;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddOrderViewModel
{
    private string _customerName = string.Empty;
    private int _orderQty;
    private string _startDt = string.Empty;
    private string _endDt = string.Empty;
    private string _validationMessage = string.Empty;

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

    public string StartDt
    {
        get => _startDt;
        set => SetField(ref _startDt, value);
    }

    public string EndDt
    {
        get => _endDt;
        set => SetField(ref _endDt, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetField(ref _validationMessage, value);
    }
    
}