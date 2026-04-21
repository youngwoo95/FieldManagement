using System.Windows;
using System.Windows.Controls;

namespace FieldManagement.View;

public partial class FacilityView : UserControl
{
    public FacilityView()
    {
        InitializeComponent();
        
    }

    private void CustomerCheckBox_Click(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox cb &&
            ItemsControl.ContainerFromElement(FailictyGrid, cb) is DataGridRow row)
        {
            row.IsSelected = false;
        }

        FailictyGrid.SelectedItem = null;
    }
}