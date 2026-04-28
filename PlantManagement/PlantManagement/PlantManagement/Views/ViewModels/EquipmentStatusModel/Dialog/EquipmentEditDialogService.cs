using System.Windows;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.EquipmentStatusModel.Dialog;

public class EquipmentEditDialogService(IEquipmentDataService equipmentDataService) : IEquipmentEditDialogService
{
    public bool? ShowFloorEditorDialog()
    {
        var editorWindow = new EditWindows(equipmentDataService)
        {
            Owner = Application.Current?.MainWindow
        };

        return editorWindow.ShowDialog();
    }
}
