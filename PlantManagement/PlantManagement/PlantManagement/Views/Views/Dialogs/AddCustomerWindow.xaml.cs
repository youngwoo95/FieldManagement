using System.Windows;

namespace PlantManagement.Views.Views.Dialogs;

public partial class AddCustomerWindow : Window
{
    public string CustomerName => CustomerNameTextBox.Text.Trim();
    
    public AddCustomerWindow()
    {
        InitializeComponent();
    }
}