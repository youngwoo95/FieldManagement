using System.Collections.ObjectModel;
using System.ComponentModel;
using PlantManagement.ViewItems;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class CustomerViewModel
{
    private readonly ObservableCollection<CustomerViewItems> _customers = new();
    private ICollectionView _filteredCustomers = null!;
    
    private string _searchKeyword = string.Empty;

    public ObservableCollection<CustomerViewItems> Customers => _customers;

    public ICollectionView FilteredCustomers => _filteredCustomers;

    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (_searchKeyword == value)
            {
                return;
            }
            
            _searchKeyword = value;
            
            OnPropertyChanged();
            _filteredCustomers.Refresh();
        }
    }
}
