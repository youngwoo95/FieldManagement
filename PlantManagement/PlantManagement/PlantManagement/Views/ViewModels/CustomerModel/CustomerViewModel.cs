using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class CustomerViewModel : BaseViewModel
{
    private readonly ICustomerDialogService _customerDialogService;
    public ICommand SearchCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }

    public CustomerViewModel(ICustomerDialogService customerDialogService)
    {
        _customerDialogService = customerDialogService;
        
        SearchCommand = new RelayCommand(_ => SearchCustomers());
        AddCommand = new RelayCommand(_ => AddCustomer());
        RemoveCommand = new RelayCommand(_ => RemoveCustomers());

        _filteredCustomers = CollectionViewSource.GetDefaultView(_customers);
        _filteredCustomers.Filter = FilterCustomer;

        LoadCustomers();
        _filteredCustomers.Refresh();
    }

    private bool FilterCustomer(object item)
    {
        if (item is not CustomerViewItems customer) return false;
        if (string.IsNullOrWhiteSpace(SearchKeyword)) return true;

        return customer.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || customer.Manager.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase)
               || customer.Tel.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase);
    }

    private void LoadCustomers()
    {
        _customers.Clear();
        _customers.Add(new CustomerViewItems
        {
            IsChecked = false,
            Name = "삼성 SDS",
            Manager = "A담당자",
            Address = "서울시 강남구 개포로 19",
            Gubun = "일반",
            Tel = "02-1234-5678"
        });
        _customers.Add(new CustomerViewItems
        {
            IsChecked = false,
            Name = "현대 오토에버",
            Manager = "B담당자",
            Address = "서울시 강남구 테헤란로 14",
            Gubun = "협력사",
            Tel = "02-2345-6789"
        });
    }

    public void SearchCustomers()
    {
        _filteredCustomers.Refresh();
    }

    private void AddCustomer()
    {
        _customerDialogService.ShowAddCustomerDialog();
    }

    private void RemoveCustomers()
    {
        var targets = _customers.Where(x => x.IsChecked).ToList();
        foreach (var target in targets)
        {
            _customers.Remove(target);
        }
    }
}
