using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.Service.v1.Customer;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class CustomerViewModel : BaseViewModel
{
    private readonly ICustomerDialogService _customerDialogService;
    private readonly ICustomerService _customerService;

    public ICommand EditCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }

    public CustomerViewModel(
        ICustomerDialogService customerDialogService,
        ICustomerService customerService)
    {
        _customerDialogService = customerDialogService;
        _customerService = customerService;

        EditCommand = new RelayCommand(_ => EditCustomers());
        AddCommand = new RelayCommand(_ => AddCustomer());
        RemoveCommand = new RelayCommand(_ => RemoveCustomers());

        _filteredCustomers = CollectionViewSource.GetDefaultView(_customers);
        _filteredCustomers.Filter = FilterCustomer;

        _ = InitializeAsync();
        _filteredCustomers.Refresh();
    }

    public async Task ReloadAsync()
    {
        await LoadCustomers();
        _filteredCustomers.Refresh();
    }

    private async Task InitializeAsync()
    {
        await ReloadAsync();
    }

    private bool FilterCustomer(object item)
    {
        if (item is not CustomerViewItems customer)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(SearchKeyword))
        {
            return true;
        }

        return customer.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || customer.Manager.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || customer.Tel.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase);
    }

    private async Task LoadCustomers()
    {
        var model = await _customerService.GetCustomerService();
        var rows = model ?? [];

        _customers.Clear();
        foreach (var item in rows)
        {
            _customers.Add(new CustomerViewItems
            {
                Seq = item.customerSeq,
                IsChecked = false,
                Name = item.customerName ?? string.Empty,
                Manager = item.managerName ?? string.Empty,
                Address = item.address ?? string.Empty,
                Gubun = item.gubun ?? string.Empty,
                Tel = item.tel ?? string.Empty,
                Department = item.department ?? string.Empty,
                Email = item.email ?? string.Empty,
                Memo = item.memo ?? string.Empty
            });
        }
    }

    private void AddCustomer()
    {
        var customer = _customerDialogService.ShowAddCustomerDialog();
        if (customer is null)
        {
            return;
        }

        _customers.Add(customer);
        _filteredCustomers.Refresh();
    }
    
    public void EditCustomers()
    {
        var targets = _customers.Where(item => item.IsChecked).ToList();
        if (targets.Count != 1)
        {
            return;
        }

        var target = targets[0];
        var edited = _customerDialogService.ShowEditCustomerDialog(target);
        if (edited is null)
        {
            return;
        }

        target.Name = edited.Name;
        target.Manager = edited.Manager;
        target.Gubun = edited.Gubun;
        target.Tel = edited.Tel;
        target.Address = edited.Address;
        target.Department = edited.Department;
        target.Email = edited.Email;
        target.Memo = edited.Memo;

        _filteredCustomers.Refresh();
    }


    private async Task RemoveCustomers()
    {
        var ids = _customers
            .Where(x => x.IsChecked)
            .Select(x => x.Seq)
            .ToList();

        if (ids.Count == 0)
        {
            return;
        }

        var ok = await _customerService.RemoveCustomerService(ids);
        if (!ok)
        {
            // 실패 메시지 처리
            return;
        }

        // 성공 시 UI 반영(로컬 제거 또는 재조회)
        var toRemove = _customers.Where(x => ids.Contains(x.Seq)).ToList();
        foreach (var item in toRemove)
        {
            _customers.Remove(item);
        }

        _filteredCustomers.Refresh();
    }
}
