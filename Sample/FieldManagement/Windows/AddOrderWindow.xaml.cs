using System;
using System.Globalization;
using System.Windows;

namespace FieldManagement.Windows;

public partial class AddOrderWindow : Window
{
    public string Customer { get; private set; } = string.Empty;
    public int OrderQty { get; private set; }
    public string StartDt { get; private set; } = string.Empty;
    public string EndDt { get; private set; } = string.Empty;

    public AddOrderWindow()
    {
        InitializeComponent();
        StartDatePicker.SelectedDate = DateTime.Today;
        EndDatePicker.SelectedDate = DateTime.Today;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Customer = CustomerTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(Customer))
        {
            MessageBox.Show("고객명을 입력해주세요.", "입력 확인", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(OrderQtyTextBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var qty) || qty <= 0)
        {
            MessageBox.Show("요청수량은 1 이상의 숫자여야 합니다.", "입력 확인", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        OrderQty = qty;
        StartDt = (StartDatePicker.SelectedDate ?? DateTime.Today).ToString("yyyy-MM-dd");
        EndDt = (EndDatePicker.SelectedDate ?? DateTime.Today).ToString("yyyy-MM-dd");
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
