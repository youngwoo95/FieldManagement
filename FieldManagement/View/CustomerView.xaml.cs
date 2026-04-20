using System.Windows;
using System.Windows.Controls;
using FieldManagement.ViewModels;

namespace FieldManagement.View;

public partial class CustomerView : UserControl
{
    public CustomerView()
    {
        InitializeComponent();
        DataContext = new CustomerViewModel();
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
