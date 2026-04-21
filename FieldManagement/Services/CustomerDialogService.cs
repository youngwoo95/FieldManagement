using System.Windows;
using FieldManagement.Models;
using FieldManagement.Windows;

namespace FieldManagement.Services;

public class CustomerDialogService : ICustomerDialogService
{
    public CustomerModel? ShowAddCustomerDialog()
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

        return new CustomerModel
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
