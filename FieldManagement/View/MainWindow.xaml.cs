using System.Windows;
using FieldManagement.ViewModels;

namespace FieldManagement.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
