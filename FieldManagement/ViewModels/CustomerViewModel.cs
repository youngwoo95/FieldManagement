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
                Name = "삼성 SDS",
                Manager = "A담당자",
                Address = "서울시 강남구 개포로619",
                Gubun = "일반",
                Tel = "02-1234-5678",
            },
            new CustomerModel
            {
                IsChecked = true,
                Name = "현대자동차",
                Manager = "B담당자",
                Address = "경기도 하남시 미사대로",
                Gubun = "협력사",
                Tel = "02-8765-4321",
            }
        };
    }
    
    
}