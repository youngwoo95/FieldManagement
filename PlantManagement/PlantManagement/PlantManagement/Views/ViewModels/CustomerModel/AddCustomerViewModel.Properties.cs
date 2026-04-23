namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddCustomerViewModel
{
    private string _customerName = string.Empty;
    private string _managerName = string.Empty;
    private string _customerGubun = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _address = string.Empty;
    private string _validationMessage = string.Empty;

    public string CustomerName
    {
        get => _customerName;
        set => SetField(ref _customerName, value);
    }

    public string ManagerName
    {
        get => _managerName;
        set => SetField(ref _managerName, value);
    }

    public string CustomerGubun
    {
        get => _customerGubun;
        set => SetField(ref _customerGubun, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetField(ref _phoneNumber, value);
    }

    public string Address
    {
        get => _address;
        set => SetField(ref _address, value);
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        set => SetField(ref _validationMessage, value);
    }
}
