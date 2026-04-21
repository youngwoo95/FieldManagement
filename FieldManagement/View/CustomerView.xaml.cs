using System.Windows;
using System.Windows.Controls;
using FieldManagement.Models;
using FieldManagement.ViewModels;
using FieldManagement.Windows;

namespace FieldManagement.View;

public partial class CustomerView : UserControl
{
    public CustomerView()
    {
        InitializeComponent();
        DataContext = new CustomerViewModel();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddCustomerWindow
        {
            Owner = Window.GetWindow(this)
        };

        var result = addWindow.ShowDialog();
        if (result != true)
            return;

        if (DataContext is not CustomerViewModel vm)
            return;

        var customerName = addWindow.CustomerName;
        if (string.IsNullOrWhiteSpace(customerName))
            return;

        if (vm.Customers.Any(x => string.Equals(x.Name, customerName, StringComparison.CurrentCultureIgnoreCase)))
            return;
        
        vm.Customers.Add(new CustomerModel
        {
            IsChecked = false,
            Name = customerName,
            Manager = addWindow.ManagerName,
            Gubun = addWindow.CustomerGubun,
            Tel = addWindow.PhoneNumber,
            Address = addWindow.Address
        });
    }

    private void CustomerCheckBox_Click(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox cb &&
            ItemsControl.ContainerFromElement(CustomerGrid, cb) is DataGridRow row)
        {
            row.IsSelected = false;
        }

        CustomerGrid.SelectedItem = null;
    }
}
