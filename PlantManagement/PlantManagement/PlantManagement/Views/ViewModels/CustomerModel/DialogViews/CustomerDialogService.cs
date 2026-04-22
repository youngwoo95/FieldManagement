using System.Windows;
using PlantManagement.ViewItems;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

public class CustomerDialogService : ICustomerDialogService
{
    public CustomerViewItems? ShowAddCustomerDialog()
    {
        var addWindow = new AddCustomerWindow
        {
            Owner = Application.Current?.MainWindow
        };
        
        var result = addWindow.ShowDialog();
        if (result != true)
            return null;

        var customerName = addWindow.CustomerName;
        if (string.IsNullOrWhiteSpace(customerName))
            return null;

        return new CustomerViewItems()
        {
            IsChecked = false,
            Name = customerName,
            Manager = addWindow.ManagerName,
            Gubun = addWindow.CustomerGubun,
            Tel = addWindow.PhoneNumber,
            Address = addWindow.Address
        };
    }
}