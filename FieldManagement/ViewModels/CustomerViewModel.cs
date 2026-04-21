using System.Collections.ObjectModel;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class CustomerViewModel : BaseViewModel
{
    private ObservableCollection<CustomerModel> _customers = new();
    
    
    public ObservableCollection<CustomerModel> Customers
    {
        get => _customers;
        set
        {
            _customers = value;
            OnPropertyChanged();
        }
    }

    public CustomerViewModel()
    {
        LoadCustomers();
    }
    
    private void LoadCustomers()
    {
        Customers = new ObservableCollection<CustomerModel>
        {
            new CustomerModel
            {
                IsChecked = true,
                Name = "삼성 SDS"
            },
            new CustomerModel
            {
                IsChecked = true,
                Name = "현대자동차"
            }
        };
    }
    
    
}