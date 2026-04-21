using System;
using System.Windows;
using System.Windows.Controls;

namespace FieldManagement.Windows;

public partial class AddWorkStatusWindow : Window
{
    public string WorkOrderNo { get; private set; } = string.Empty;
    public string MachineName { get; private set; } = string.Empty;
    public string CustomerName { get; private set; } = string.Empty;
    public string Status { get; private set; } = "In Progress";
    public string WorkDate { get; private set; } = DateTime.Today.ToString("yyyy-MM-dd");

    public AddWorkStatusWindow()
    {
        InitializeComponent();
        WorkDatePicker.SelectedDate = DateTime.Today;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        WorkOrderNo = WorkOrderNoTextBox.Text.Trim();
        MachineName = MachineNameTextBox.Text.Trim();
        CustomerName = CustomerNameTextBox.Text.Trim();
        Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "In Progress";
        WorkDate = (WorkDatePicker.SelectedDate ?? DateTime.Today).ToString("yyyy-MM-dd");

        if (string.IsNullOrWhiteSpace(WorkOrderNo) || string.IsNullOrWhiteSpace(MachineName))
        {
            MessageBox.Show("작업지시번호와 장비명은 필수입니다.", "입력 확인", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
