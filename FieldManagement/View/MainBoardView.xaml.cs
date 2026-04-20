using System.Windows.Controls;
using FieldManagement.ViewModels;

namespace FieldManagement.View;

public partial class MainBoardView : UserControl
{
    public MainBoardView()
    {
        InitializeComponent();
        DataContext = new MainBoardViewModel();
    }
}
