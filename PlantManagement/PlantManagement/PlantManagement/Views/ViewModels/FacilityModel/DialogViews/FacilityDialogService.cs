using System.Windows;
using PlantManagement.ViewItems;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.FacilityModel.DialogViews;

public class FacilityDialogService : IFacilityDialogService
{
    public FacilityViewItems? ShowAddFacilityDialog()
    {
        var addFacilityViewModel = new AddFacilityViewModel();
        var addWindow = new AddFacilityWindow(addFacilityViewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
            return null;

        return new FacilityViewItems()
        {
            IsChecked = false,
            Name = addFacilityViewModel.FacilityName,
            Maker = addFacilityViewModel.Maker,
            Purpose = addFacilityViewModel.Purpose
        };


    }
}