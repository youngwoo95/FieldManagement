using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using PlantManagement.Comm;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class CustomerViewModel : BaseViewModel
{
    public ICommand SearchCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }

    public CustomerViewModel()
    {
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
        _customers.Add(new CustomerViewItems
        {
            Name = "신규 고객",
            Manager = "담당자",
            Gubun = "일반",
            Tel = "02-0000-0000",
            Address = "주소 입력"
        });
        
        // Dialog 열어야함
        /*
        var newCustomer = _customerDialogService.ShowAddCustomerDialog();
        if (newCustomer is null)
            return;

        if (_allCustomers.Any(x => string.Equals(x.Name, newCustomer.Name, StringComparison.CurrentCultureIgnoreCase)))
            return;

        _allCustomers.Add(newCustomer);
        ApplyCustomerFilter(); 
         */
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
