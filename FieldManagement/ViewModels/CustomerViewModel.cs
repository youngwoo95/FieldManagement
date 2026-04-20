using System.Collections.ObjectModel;
using FieldManagement.Models;

namespace FieldManagement.ViewModels;

public class CustomerViewModel : BaseViewModel
{
    private ObservableCollection<Customers> _customers = new();
    
    
    public ObservableCollection<Customers> Customers
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
        Customers = new ObservableCollection<Customers>
        {
            new Customers
            {
                IsChecked = true,
                Name = "삼성 SDS"
            },
            new Customers
            {
                IsChecked = true,
                Name = "현대자동차"
            }
        };
    }
    
    
}