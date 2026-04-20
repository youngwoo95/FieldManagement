using System.Windows;

namespace FieldManagement.Windows;

public partial class EditWindows : Window
{
    public EditWindows()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}