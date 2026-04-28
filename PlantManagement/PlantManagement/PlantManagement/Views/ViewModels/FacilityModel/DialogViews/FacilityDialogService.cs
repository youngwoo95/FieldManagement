using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.ViewItems;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.FacilityModel.DialogViews;

public class FacilityDialogService(IServiceProvider serviceProvider) : IFacilityDialogService
{
    public FacilityViewItems? ShowAddFacilityDialog()
    {
        var viewModel = serviceProvider.GetRequiredService<AddFacilityViewModel>();
        viewModel.PrepareForAdd();
        return ShowDialog(viewModel);
    }

    public FacilityViewItems? ShowEditFacilityDialog(FacilityViewItems target)
    {
        var viewModel = serviceProvider.GetRequiredService<AddFacilityViewModel>();
        viewModel.PrepareForEdit(target);
        return ShowDialog(viewModel);
    }

    private static FacilityViewItems? ShowDialog(AddFacilityViewModel viewModel)
    {
        var addWindow = new AddFacilityWindow(viewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
        {
            return null;
        }

        return viewModel.BuildFacilityViewItem();
    }
}
