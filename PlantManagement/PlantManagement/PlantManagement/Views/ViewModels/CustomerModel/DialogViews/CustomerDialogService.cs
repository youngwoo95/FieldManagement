using System.Windows;
using PlantManagement.ViewItems;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

public class CustomerDialogService : ICustomerDialogService
{
    public CustomerViewItems? ShowAddCustomerDialog()
    {
        var addCustomerViewModel = new AddCustomerViewModel();
        var addWindow = new AddCustomerWindow(addCustomerViewModel)
        {
            Owner = Application.Current?.MainWindow
        };
        
        var result = addWindow.ShowDialog();
        if (result != true)
            return null;

        var customerName = addCustomerViewModel.CustomerName;
        if (string.IsNullOrWhiteSpace(customerName))
            return null;

        return new CustomerViewItems()
        {
            IsChecked = false,
            Name = customerName,
            Manager = addCustomerViewModel.ManagerName,
            Gubun = addCustomerViewModel.CustomerGubun,
            Tel = addCustomerViewModel.PhoneNumber,
            Address = addCustomerViewModel.Address
        };
    }
}
