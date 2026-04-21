using System.Windows;
using System.Windows.Controls;

namespace FieldManagement.Windows;

public partial class AddCustomerWindow : Window
{
    public string CustomerName => CustomerNameTextBox.Text.Trim();
    public string ManagerName => ManagerNameTextBox.Text.Trim();
    public string CustomerGubun => (CustomerTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
    public string PhoneNumber => PhoneTextBox.Text.Trim();
    public string Address => AddressTextBox.Text.Trim();

    public AddCustomerWindow()
    {
        InitializeComponent();
        CustomerTypeComboBox.SelectedIndex = 0;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(CustomerNameTextBox.Text))
        {
            ValidationText.Text = "고객사명은 필수입니다.";
            CustomerNameTextBox.Focus();
            return;
        }

        ValidationText.Text = "저장되었습니다.";
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
