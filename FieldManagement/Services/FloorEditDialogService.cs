using System.Windows;
using FieldManagement.Windows;

namespace FieldManagement.Services;

public class FloorEditDialogService : IFloorEditDialogService
{
    public string? ShowFloorEditor(string? preferredFloorName)
    {
        var dialog = new EditWindows(preferredFloorName)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = dialog.ShowDialog();
        if (result != true)
        {
            return null;
        }

        return dialog.SelectedFloorName;
    }
}
