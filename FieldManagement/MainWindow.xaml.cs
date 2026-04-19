using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FieldManagement.Views;

namespace FieldManagement;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new MainBoardView();
    }
    
    private void MenuRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is not RadioButton radio)
            return;

        string? menu = radio.Tag?.ToString();

        UserControl view = menu switch
        {
            "Home" => new MainBoardView(),
            "Input" => new InputView()
        };

        MainContent.Content = view;
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("테마 버튼 클릭");
    }
}
