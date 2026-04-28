using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.OrderModel.Dialog;

public class OrderDialogService(IServiceProvider serviceProvider) : IOrderDialogService
{
    public OrderViewItems? ShowAddOrderDialog()
    {
        var addOrderViewModel = serviceProvider.GetRequiredService<AddOrderViewModel>();
        addOrderViewModel.PrepareForAdd();
        return ShowDialog(addOrderViewModel);
    }

    public OrderViewItems? ShowEditOrderDialog(OrderViewItems target)
    {
        var addOrderViewModel = serviceProvider.GetRequiredService<AddOrderViewModel>();
        addOrderViewModel.PrepareForEdit(target);
        return ShowDialog(addOrderViewModel);
    }

    private static OrderViewItems? ShowDialog(AddOrderViewModel viewModel)
    {
        var addWindow = new AddOrderWindow(viewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
        {
            return null;
        }

        return viewModel.BuildOrderViewItem();
    }
}
