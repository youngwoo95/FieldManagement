using System.Windows;
using FieldManagement.Models;
using FieldManagement.Windows;

namespace FieldManagement.Services;

public class FacilityDialogService : IFacilityDialogService
{
    public FacilityModel? ShowAddFacilityDialog()
    {
        var dialog = new AddFacilityWindow
        {
            Owner = Application.Current?.MainWindow
        };

        var result = dialog.ShowDialog();
        if (result != true)
            return null;

        if (string.IsNullOrWhiteSpace(dialog.FacilityName))
            return null;

        return new FacilityModel
        {
            IsChecked = false,
            Name = dialog.FacilityName,
            Maker = dialog.Maker,
            Purpose = dialog.Purpose
        };
    }
}
