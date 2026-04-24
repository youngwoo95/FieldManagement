using System.Collections.ObjectModel;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddWorkStatusViewModel
{
    private readonly ObservableCollection<string> _facilityNames = new();
    private readonly ObservableCollection<string> _customerNames = new();
    private string _workOrderNo = string.Empty;
    private string _machineName = string.Empty;
    private string _customerName = string.Empty;
    private string _status = "InProgress";
    private string _workDate = DateTime.Today.ToString("yyyy-MM-dd");
    private string _attachmentFilePath = string.Empty;
    private string _validationMessage = string.Empty;
    
    public ObservableCollection<string> FacilityNames => _facilityNames;
    public ObservableCollection<string> CustomerNames => _customerNames;

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
 
}