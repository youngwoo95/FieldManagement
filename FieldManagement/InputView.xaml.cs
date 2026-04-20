using System.Windows;
using System.Windows.Controls;
using FieldManagement.Windows;

namespace FieldManagement;

public partial class InputView : UserControl
{
    public InputView()
    {
        InitializeComponent();
    }


    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        var popup = new EditWindows();
        popup.Owner = Window.GetWindow(this); // 현재 UserControl이 들어있는 부모 Window 찾기
        popup.ShowDialog(); // 모달 팝업
    }
}