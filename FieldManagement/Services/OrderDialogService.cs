using System.Windows;
using FieldManagement.Models;
using FieldManagement.Windows;

namespace FieldManagement.Services;

public class OrderDialogService : IOrderDialogService
{
    public OrderModel? ShowAddOrderDialog()
    {
        var dialog = new AddOrderWindow
        {
            Owner = Application.Current?.MainWindow
        };

        var result = dialog.ShowDialog();
        if (result != true)
            return null;

        if (string.IsNullOrWhiteSpace(dialog.Customer) || dialog.OrderQty <= 0)
            return null;

        return new OrderModel
        {
            Customer = dialog.Customer,
            OrderQty = dialog.OrderQty,
            StartDt = dialog.StartDt,
            EndDt = dialog.EndDt
        };
    }
}
