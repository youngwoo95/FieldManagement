using System.Windows;

namespace FieldManagement.Windows;

public partial class AddFacilityWindow : Window
{
    public string FacilityName { get; private set; } = string.Empty;
    public string Maker { get; private set; } = string.Empty;
    public string Purpose { get; private set; } = string.Empty;

    public AddFacilityWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        FacilityName = NameTextBox.Text.Trim();
        Maker = MakerTextBox.Text.Trim();
        Purpose = PurposeTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(FacilityName))
        {
            MessageBox.Show("장비명을 입력해주세요.", "입력 확인", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
