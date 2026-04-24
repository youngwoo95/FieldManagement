using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlantManagement.Comm;

namespace PlantManagement.Views.ViewModels.CustomerModel;

public partial class AddWorkStatusViewModel : BaseViewModel
{
    public event Action<bool?>? RequestClose;
   
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddWorkStatusViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save());
        CancelCommand = new RelayCommand(_ => Cancel());
    }
    
    private void Save()
    {
        if (string.IsNullOrWhiteSpace(CustomerName))
        {
            ValidationMessage = "고객사명은 필수입니다.";
            return;
        }

        ValidationMessage = string.Empty;
        RequestClose?.Invoke(true);
    }

    private void Cancel()
    {
        RequestClose?.Invoke(false);  
    }

    public void SetCustomerNames(IEnumerable<string> customerNames)
    {
        CustomerNames.Clear();
        foreach (var customerName in customerNames)
        {
            CustomerNames.Add(customerName);
        }
    }

    public void SetFacilityNames(IEnumerable<string> facilityNames)
    {
        FacilityNames.Clear();
        foreach (var facilityName in facilityNames)
        {
            FacilityNames.Add(facilityName);
        }
    }
}
