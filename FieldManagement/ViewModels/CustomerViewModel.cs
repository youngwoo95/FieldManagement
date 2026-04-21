using System.Collections.ObjectModel;
using System.Windows.Input;
using FieldManagement.Commands;
using FieldManagement.Models;
using FieldManagement.Repository.Customer;
using FieldManagement.Services;

namespace FieldManagement.ViewModels;

public class CustomerViewModel : BaseViewModel
{
    private readonly ICustomerDialogService _customerDialogService;

    private readonly List<CustomerModel> _allCustomers = new();
    private ObservableCollection<CustomerModel> _customers = new();
    private string _searchKeyword = string.Empty;

    public ObservableCollection<CustomerModel> Customers
    {
        get => _customers;
        set
        {
            _customers = value;
            OnPropertyChanged();
        }
    }

    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (_searchKeyword == value)
                return;

            _searchKeyword = value;
            OnPropertyChanged();
        }
    }

    public ICommand SearchCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand RemoveCommand { get; }

    public CustomerViewModel(ICustomerDialogService customerDialogService, ICustomerRepository customerRepository)
    {
        _customerDialogService = customerDialogService;
        _ = customerRepository;

        SearchCommand = new RelayCommand(_ => SearchCustomers());
        AddCommand = new RelayCommand(_ => AddCustomer());
        RemoveCommand = new RelayCommand(_ => RemoveSelectedCustomers());

        LoadCustomers();
    }

    private void LoadCustomers()
    {
        _allCustomers.Clear();
        _allCustomers.AddRange(new[]
        {
            new CustomerModel
            {
                IsChecked = false,
                Name = "삼성 SDS",
                Manager = "A담당자",
                Address = "서울시 강남구 개포로619",
                Gubun = "일반",
                Tel = "02-1234-5678"
            },
            new CustomerModel
            {
                IsChecked = false,
                Name = "현대자동차",
                Manager = "B담당자",
                Address = "경기도 하남시 미사대로",
                Gubun = "협력사",
                Tel = "02-8765-4321"
            }
        });

        ApplyCustomerFilter();
    }

    private void SearchCustomers()
    {
        ApplyCustomerFilter();
    }

    private void AddCustomer()
    {
        var newCustomer = _customerDialogService.ShowAddCustomerDialog();
        if (newCustomer is null)
            return;

        if (_allCustomers.Any(x => string.Equals(x.Name, newCustomer.Name, StringComparison.CurrentCultureIgnoreCase)))
            return;

        _allCustomers.Add(newCustomer);
        ApplyCustomerFilter();
    }

    private void RemoveSelectedCustomers()
    {
        var selected = _allCustomers.Where(x => x.IsChecked).ToList();
        if (selected.Count == 0)
            return;

        foreach (var item in selected)
            _allCustomers.Remove(item);

        ApplyCustomerFilter();
    }

    private void ApplyCustomerFilter()
    {
        IEnumerable<CustomerModel> query = _allCustomers;

        if (!string.IsNullOrWhiteSpace(SearchKeyword))
        {
            var keyword = SearchKeyword.Trim();
            query = query.Where(x => x.Name.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
        }

        Customers = new ObservableCollection<CustomerModel>(query);
    }
}
