using System.Linq;
using System.Windows;
using PlantManagement.ViewItems;
using PlantManagement.Views.ViewModels.CustomerModel;
using PlantManagement.Views.Views.Dialogs;

namespace PlantManagement.Views.ViewModels.OrderModel.Dialog;

public class OrderDialogService : IOrderDialogService
{
    private readonly CustomerViewModel _customerViewModel;

    public OrderDialogService(CustomerViewModel customerViewModel)
    {
        _customerViewModel = customerViewModel;
    }

    public OrderViewItems? ShowAddOrderDialog()
    {
        var addOrderViewModel = new AddOrderViewModel();
        var customerNames = _customerViewModel.Customers
            .Select(x => x.Name)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        addOrderViewModel.SetCustomerNames(customerNames);

        var addWindow = new AddOrderWindow(addOrderViewModel)
        {
            Owner = Application.Current?.MainWindow
        };

        var result = addWindow.ShowDialog();
        if (result != true)
            return null;

        return new OrderViewItems()
        {
            Customer = addOrderViewModel.CustomerName,
            OrderQty = addOrderViewModel.OrderQty,
            StartDt = addOrderViewModel.StartDt,
            EndDt = addOrderViewModel.EndDt,
            PdfFileName = addOrderViewModel.AttachmentFilePath
        };
    }
}
