using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Dto.v1.Customer;
using PlantManagement.Service.v1.Customer;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddCustomerViewModel : BaseViewModel
{
    private readonly ICustomerService _customerService;
    private bool _isEditMode;
    private int _editingCustomerSeq;

    public event Action<bool?>? RequestClose;

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddCustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        SaveCommand = new RelayCommand(_ => _ = SaveAsync());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    public void PrepareForAdd()
    {
        _isEditMode = false;
        _editingCustomerSeq = 0;

        CustomerName = string.Empty;
        ManagerName = string.Empty;
        CustomerGubun = string.Empty;
        DepartmentName = string.Empty;
        PhoneNumber = string.Empty;
        Email = string.Empty;
        Address = string.Empty;
        Memo = string.Empty;
        ValidationMessage = string.Empty;
    }

    public void PrepareForEdit(CustomerViewItems target)
    {
        _isEditMode = true;
        _editingCustomerSeq = target.Seq;

        CustomerName = target.Name;
        ManagerName = target.Manager;
        CustomerGubun = target.Gubun;
        DepartmentName = target.Department;
        PhoneNumber = target.Tel;
        Email = target.Email;
        Address = target.Address;
        Memo = target.Memo;
        ValidationMessage = string.Empty;
    }

    public CustomerViewItems BuildCustomerViewItem()
    {
        return new CustomerViewItems
        {
            Seq = _editingCustomerSeq,
            IsChecked = false,
            Name = CustomerName,
            Manager = ManagerName,
            Gubun = CustomerGubun,
            Tel = PhoneNumber,
            Address = Address,
            Department = DepartmentName,
            Email = Email,
            Memo = Memo
        };
    }

    private async Task SaveAsync()
    {
        if (!ValidateInput())
        {
            return;
        }

        if (_isEditMode)
        {
            var editDto = new EditCustomerDto
            {
                customerSeq = _editingCustomerSeq,
                customerName = CustomerName,
                managerName = ManagerName,
                gubun = CustomerGubun,
                department = DepartmentName,
                tel = PhoneNumber,
                email = Email,
                address = Address,
                memo = Memo
            };

            var editResult = await _customerService.EditCustomerService(editDto);
            if (!editResult)
            {
                ValidationMessage = "수정에 실패했습니다.";
                return;
            }

            RequestClose?.Invoke(true);
            return;
        }

        var dtoModel = new AddCustomerDto
        {
            customerName = CustomerName,
            managerName = ManagerName,
            address = Address,
            department = DepartmentName,
            email = Email,
            gubun = CustomerGubun,
            memo = Memo,
            tel = PhoneNumber
        };

        var addResult = await _customerService.AddCustomerService(dtoModel);
        if (!addResult)
        {
            ValidationMessage = "저장에 실패했습니다.";
            return;
        }

        RequestClose?.Invoke(true);
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            ValidationMessage = "고객사명은 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ManagerName))
        {
            ValidationMessage = "담당자명은 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Address))
        {
            ValidationMessage = "주소는 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(DepartmentName))
        {
            ValidationMessage = "부서는 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ValidationMessage = "이메일은 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(CustomerGubun))
        {
            ValidationMessage = "구분값은 필수입니다.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ValidationMessage = "전화번호는 필수입니다.";
            return false;
        }

        ValidationMessage = string.Empty;
        return true;
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);
    }
}
