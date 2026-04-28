using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.ViewItems;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.CustomerModel.DialogViews;

public class CustomerDialogService(IServiceProvider serviceProvider) : ICustomerDialogService
{
    public CustomerViewItems? ShowAddCustomerDialog()
    {
        var addCustomerViewModel = serviceProvider.GetRequiredService<AddCustomerViewModel>();
        addCustomerViewModel.PrepareForAdd();
        return ShowDialog(addCustomerViewModel);
    }

    public CustomerViewItems? ShowEditCustomerDialog(CustomerViewItems target)
    {
        var addCustomerViewModel = serviceProvider.GetRequiredService<AddCustomerViewModel>();
        addCustomerViewModel.PrepareForEdit(target);
        return ShowDialog(addCustomerViewModel);
    }

    private static CustomerViewItems? ShowDialog(AddCustomerViewModel viewModel)
    {
        var addWindow = new AddCustomerWindow(viewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
        {
            return null;
        }

        var customerName = viewModel.CustomerName;
        if (string.IsNullOrWhiteSpace(customerName))
        {
            return null;
        }

        return viewModel.BuildCustomerViewItem();
    }
}
